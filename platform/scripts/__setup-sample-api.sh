#!/bin/bash
home=$1
acrName=$2
appInsightsKey=$3
resourceGroupName=$4
apimName=$5

echo "Importing container to private repo"
az acr login -n $acrName
az acr import -n $acrName --source ghcr.io/graemefoster/api-management-sample-java-soap-api:latest --image api-management-sample-java-soap-api:latest
echo "Imported container to private repo. Proceeding to deploy to AKS"

cat <<EOF > $home/sample-java-api.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: sample-java-soap-api-deployment
  labels:
    app: sample-java-soap-api
spec:
  replicas: 2
  selector:
    matchLabels:
      app: sample-java-soap-api
  template:
    metadata:
      labels:
        app: sample-java-soap-api
    spec:
      containers:
      - name: sample-java-soap-api
        image: "$acrName.azurecr.io/api-management-sample-java-soap-api:latest"
        ports:
          - containerPort: 8080
            protocol: TCP
        env:
          - name: APPLICATIONINSIGHTS_CONNECTION_STRING
            value: "$appInsightsKey"
---
apiVersion: v1
kind: Service
metadata:
  name: sample-java-soap-api-service
spec:
  selector:
    app: sample-java-soap-api
  ports:
    - protocol: TCP
      port: 5678
      targetPort: 8080
---
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: sample-java-soap-api-ingress
  annotations:
    kubernetes.io/ingress.class: nginx
    nginx.ingress.kubernetes.io/ssl-redirect: "false"
    nginx.ingress.kubernetes.io/use-regex: "true"
    nginx.ingress.kubernetes.io/rewrite-target: /\$2
spec:
  tls:
  - hosts:
    - api.poc.internal
    secretName: aks-ingress-tls
  rules:
  - host: api.poc.internal
    http:
      paths:
        - path: /country(/|$)(.*)
          pathType: Prefix
          backend:
            service:
              name: sample-java-soap-api-service
              port:
                number: 5678"
EOF

kubectl apply -f $home/sample-java-api.yaml

echo "Deployed SOAP API to Kubernetes"
echo "--------------------------------------------------------"

echo "--------------------------------------------------------"
echo "Importing API definition to API Management"
az apim api import --path employee --resource-group $resourceGroupName --service-name $apimName --api-id employeeApi --api-type http --specification-format OpenApi --specification-path "$home/api-definitions/employee-api.json"
echo "--------------------------------------------------------"
