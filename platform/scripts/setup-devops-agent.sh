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
echo "ACR Name : $7"
echo "AKS App Insights Key : $8"
echo "DevOps Agent Name: $9"
echo "DevOps Server: ${10}"
echo "DevOps PAT: ${11}"

aksClusterName=$2
resourceGroupName=$3
storageAccountName=$4
storageSasToken=$5
apimName=$6
acrName=$7
appInsightsKey=$8
devopsServer=$9
devopsPAT=${10}
devopsAgentName=${11}

mkdir -p $home/downloads
echo "Installing az cli"
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
echo "Logging into Azure using vm identity"
az login --identity

mkdir -p $home/agent-scripts
mkdir -p $home/api-definitions

az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformtemplates" -f "$home/agent-scripts/__install-dependencies.sh" --name "script/__install-dependencies.sh" --output none
az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformtemplates" -f "$home/agent-scripts/__setup-nginx-ingress.sh" --name "script/__setup-nginx-ingress.sh" --output none
az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformtemplates" -f "$home/agent-scripts/__setup-apim-self-hosted-gateway.sh" --name "script/__setup-apim-self-hosted-gateway.sh" --output none
az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformtemplates" -f "$home/agent-scripts/__setup-sample-api.sh" --name "script/__setup-sample-api.sh" --output none
az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformtemplates" -f "$home/agent-scripts/__setup-devops-agent.sh" --name "script/__setup-devops-agent.sh" --output none
az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformtemplates" -f "$home/api-definitions/employee-api.json" --name "apis/open-api-definitions/employee-api.json" --output none
chmod -R 744  $home/agent-scripts/


echo "--------------------------------------------------------"
echo "SETTING UP DEPENDENCIES"
echo "--------------------------------------------------------"
$home/agent-scripts/__install-dependencies.sh "$home" "$aksClusterName" "$resourceGroupName"
echo "--------------------------------------------------------"

echo "--------------------------------------------------------"
echo "RUNNING KUBELOGIN"
export KUBECONFIG=$home/.kube/config
$home/kubelogin/bin/linux_amd64/kubelogin convert-kubeconfig -l msi
kubectl get nodes
echo "--------------------------------------------------------"

echo "--------------------------------------------------------"
echo "SETTING UP NGINX INGRES IN AKS"
echo "--------------------------------------------------------"
$home/agent-scripts/__setup-nginx-ingress.sh "$home" "$storageAccountName" "$storageSasToken"
echo "--------------------------------------------------------"

echo "--------------------------------------------------------"
echo "SETTING UP APIm SELF HOSTED GATEWAY IN AKS"
echo "--------------------------------------------------------"
$home/agent-scripts/__setup-apim-self-hosted-gateway.sh "$home" "$resourceGroupName" "$apimName" "$apimName-onprem-apim-gway"
echo "--------------------------------------------------------"

echo "DEPLOYING SAMPLE API TO AKS"
echo "--------------------------------------------------------"
$home/agent-scripts/__setup-sample-api.sh "$home" "$acrName" "$appInsightsKey" "$resourceGroupName" "$apimName"
echo "--------------------------------------------------------"


if [ -z "$devopsServer" ]
then
    echo "Skipping devops agent configuration $devopsServer"
else

  echo "DEPLOYING DEVOPS AGENT"
  echo "--------------------------------------------------------"
  $home/agent-scripts/__setup-devops-agent.sh "$home" "$devopsServer" "$devopsPAT" "$user" "$devopsAgentName"
  echo "--------------------------------------------------------"

fi

# Set ownership on all of the files we've created
echo "--------------------------------------------------------"
echo "SETTING OWNERSHIP OF FILES TO $user"
echo "--------------------------------------------------------"
cd $home
sudo chown -R $user:$user *
sudo chown -R $user:$user $home/.kube/*
echo "--------------------------------------------------------"


