#!/bin/bash

user="$1"
home="/home/$1"
echo "User: $1"
echo "Home: $home"
echo "Cluster Name: $2"
echo "Resource Group: $3"
echo "Storage Account Name: $4"
echo "Storage SAS Token: $5"
echo "APIm Name : $6"

aksClusterName=$2
resourceGroupName=$3
storageAccountName=$4
storageSasToken=$5
apimName=$6
wellKnownGatewayName="$apimName-onprem-apim-gway"

mkdir -p $home/downloads
echo "Installing az cli"
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
echo "Installing unzip"
sudo apt install -y unzip
echo "Installing jq"
sudo apt install -y jq

echo "Fetching kubelogin"
curl -L https://github.com/Azure/kubelogin/releases/download/v0.0.9/kubelogin-linux-amd64.zip -o $home/downloads/kubelogin.zip
sudo unzip -o $home/downloads/kubelogin.zip -d $home/kubelogin/
echo "Logging into Azure using vm identity"
az login --identity
echo "Getting credentials for AKS"

az aks get-credentials --overwrite-existing --name $aksClusterName --resource-group $resourceGroupName --file $home/.kube/config

echo "Installing Azure CLI"
sudo az aks install-cli 

echo "Running kubelogin"
export KUBECONFIG=$home/.kube/config
$home/kubelogin/bin/linux_amd64/kubelogin convert-kubeconfig -l msi

echo "Installing Helm"
curl https://raw.githubusercontent.com/helm/helm/master/scripts/get-helm-3 | bash

echo "Fetching TLS certificate for NGINX from storage account $storageAccountName"
az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformsecrets" -f $home/aks-ingress-tls.key --name "aks-ingress-tls.key"
az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformsecrets" -f $home/aks-ingress-tls.crt --name "aks-ingress-tls.crt"

ls -la $home/

echo "Setting up nginx ingress in AKS"

# See https://docs.microsoft.com/en-us/azure/aks/ingress-internal-ip for more details

echo "controller:
  service:
    loadBalancerIP: 10.0.1.200
    annotations:
      service.beta.kubernetes.io/azure-load-balancer-internal: ""true""" > $home/internal-ingress.yaml

# Create a namespace for your ingress resources
kubectl create namespace ingress-basic

# Add the ingress-nginx repository
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx

# Use Helm to deploy an NGINX ingress controller
helm install nginx-ingress ingress-nginx/ingress-nginx \
    --namespace ingress-basic \
    -f $home/internal-ingress.yaml \
    --set controller.replicaCount=2 \
    --set controller.nodeSelector."beta\.kubernetes\.io/os"=linux \
    --set defaultBackend.nodeSelector."beta\.kubernetes\.io/os"=linux \
    --set controller.admissionWebhooks.patch.nodeSelector."beta\.kubernetes\.io/os"=linux

echo "Creating Kubernetes secret (in default namespace) to eable TLS on NGINX ingress to ingress objects in default namespace"
kubectl create secret tls aks-ingress-tls \
    --key $home/aks-ingress-tls.key \
    --cert $home/aks-ingress-tls.crt

echo "Deploying Api Management self hosted Gateway to AKS"
apimId=$(az apim show  --name $apimName --resource-group $resourceGroupName -o tsv --query 'id')
azAccessToken=$(az account get-access-token --query 'accessToken' -o tsv)
tomorrow=$(date --date="tomorrow" -Iseconds -u)
apimGatewayRegistrationTokenResponse=$(curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer $azAccessToken" "https://management.azure.com$apimId/gateways/$wellKnownGatewayName/token?api-version=2019-12-01" -d "{\"keyType\":\"primary\",\"expiry\":\"$tomorrow\"}") 
apimGatewayRegistrationToken=$(echo $apimGatewayRegistrationTokenResponse | jq '.value' -r)


echo "Deploying APIm Self Hosted Gateway to AKS"
kubectl create secret generic "$wellKnownGatewayName-token" --from-literal=value="GatewayKey $apimGatewayRegistrationToken"  --type=Opaque
echo "Created secret for gateway. Processing to deploy manifest"

echo "# NOTE: Before deploying to a production environment, please review the documentation -> https://aka.ms/self-hosted-gateway-production
---
apiVersion: v1
kind: ConfigMap
metadata:
  name: $wellKnownGatewayName-env
data:
  config.service.endpoint: "https://$apimName.management.azure-api.net$apimId?api-version=2019-12-01"
---
apiVersion: apps/v1
kind: Deployment
metadata:
  name: $wellKnownGatewayName
spec:
  replicas: 1
  selector:
    matchLabels:
      app: $wellKnownGatewayName
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxUnavailable: 0
      maxSurge: 25%
  template:
    metadata:
      labels:
        app: $wellKnownGatewayName
    spec:
      terminationGracePeriodSeconds: 60
      containers:
      - name: $wellKnownGatewayName
        image: mcr.microsoft.com/azure-api-management/gateway:latest
        ports:
        - name: http
          containerPort: 8080
        - name: https
          containerPort: 8081
        readinessProbe:
          httpGet:
            path: /internal-status-0123456789abcdef
            port: http
            scheme: HTTP
          initialDelaySeconds: 0
          periodSeconds: 5
          failureThreshold: 3
          successThreshold: 1
        env:
        - name: config.service.auth
          valueFrom:
            secretKeyRef:
              name: $wellKnownGatewayName-token
              key: value
        envFrom:
        - configMapRef:
            name: $wellKnownGatewayName-env
---
apiVersion: v1
kind: Service
metadata:
  name: $wellKnownGatewayName
spec:
  ports:
  - name: http
    port: 80
    targetPort: 8080
  - name: https
    port: 443
    targetPort: 8081
  selector:
    app: $wellKnownGatewayName
---
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: apimgateway
  annotations:
    kubernetes.io/ingress.class: nginx
    nginx.ingress.kubernetes.io/backend-protocol: \"HTTPS\"
spec:
  rules:
  - host: apigateway.api.poc.internal
    http:
      paths:
        - path: /
          pathType: Prefix
          backend:
              service:
                  name: $wellKnownGatewayName
                  port: 
                      number: 443" > $home/self-hosted-gateway.yaml

kubectl apply -f $home/self-hosted-gateway.yaml
echo "Deploy self hosted gateway manifest"

# Set ownership on all of the files we've created
cd $home
sudo chown -R $user:$user *
sudo chown -R $user:$user $home/.kube/*


