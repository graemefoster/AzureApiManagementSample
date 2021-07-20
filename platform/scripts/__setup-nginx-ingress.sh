#!/bin/bash
home=$1
storageAccountName=$2
storageSasToken=$3

echo "Fetching TLS certificate for NGINX from storage account $storageAccountName"
az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformsecrets" -f $home/aks-ingress-tls.key --name "aks-ingress-tls.key" --output none
az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformsecrets" -f $home/aks-ingress-tls.crt --name "aks-ingress-tls.crt" --output none

echo "Setting up nginx ingress in AKS"

# See https://docs.microsoft.com/en-us/azure/aks/ingress-internal-ip for more details

cat <<EOF > $home/internal-ingress.yaml
controller:
  service:
    loadBalancerIP: 10.0.1.200
    annotations:
      service.beta.kubernetes.io/azure-load-balancer-internal: "true"
EOF

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

