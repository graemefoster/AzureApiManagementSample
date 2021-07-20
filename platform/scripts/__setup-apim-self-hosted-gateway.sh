#!/bin/bash
home=$1
resourceGroupName=$2
apimName=$3
wellKnownGatewayName=$4

echo "Deploying Api Management self hosted Gateway to AKS"
apimId=$(az apim show  --name $apimName --resource-group $resourceGroupName -o tsv --query 'id')
azAccessToken=$(az account get-access-token --query 'accessToken' -o tsv)
tomorrow=$(date --date="tomorrow" -Iseconds -u)
apimGatewayRegistrationTokenResponse=$(curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer $azAccessToken" "https://management.azure.com$apimId/gateways/$wellKnownGatewayName/token?api-version=2019-12-01" -d "{\"keyType\":\"primary\",\"expiry\":\"$tomorrow\"}") 
apimGatewayRegistrationToken=$(echo $apimGatewayRegistrationTokenResponse | jq '.value' -r)


echo "Deploying APIm Self Hosted Gateway to AKS"
kubectl create secret generic "$wellKnownGatewayName-token" --from-literal=value="GatewayKey $apimGatewayRegistrationToken"  --type=Opaque
echo "Created secret for gateway. Processing to deploy manifest"

cat <<EOF > $home/self-hosted-gateway.yaml
# NOTE: Before deploying to a production environment, please review the documentation -> https://aka.ms/self-hosted-gateway-production
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
    nginx.ingress.kubernetes.io/backend-protocol: "HTTPS"
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
                      number: 443
EOF

kubectl apply -f $home/self-hosted-gateway.yaml
echo "Deploy self hosted gateway manifest"

