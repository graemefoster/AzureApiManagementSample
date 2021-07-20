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
echo "DevOps Server: $8"
echo "DevOps PAT: $9"
echo "DevOps Agent Name: ${10}"

aksClusterName=$2
resourceGroupName=$3
storageAccountName=$4
storageSasToken=$5
apimName=$6
acrName=$7
devopsServer=$8
devopsPAT=$9
devopsAgentName=${10}


mkdir -p $home/agent-scripts
az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformtemplates" -f "$home/agent-scripts/__install-dependencies.sh" --name "script/__install-dependencies.sh"
az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformtemplates" -f "$home/agent-scripts/__setup-nginx-ingress.sh" --name "script/__setup-nginx-ingress.sh"
az storage blob download --sas-token "$storageSasToken" --account-name "$storageAccountName" --container-name "platformtemplates" -f "$home/agent-scripts/__setup-sample-api.sh" --name "script/__setup-sample-api.sh"
chmod -R 744  $home/agent-scripts/


echo "--------------------------------------------------------"
echo "SETTING UP DEPENDENCIES"
echo "--------------------------------------------------------"
$home/agent-scripts/__install-dependencies.sh "$home" "$aksClusterName" "$resourceGroupName"
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
$home/agent-scripts/__setup-sample-api.sh "$home" "$acrName"
echo "DONE"
echo "--------------------------------------------------------"


if [ -z "$devopsPAT" ]
then
    echo "Skipping devops agent configuration $devopsServer"
else

  echo "DEPLOYING DEVOPS AGENT"
  echo "--------------------------------------------------------"
  $home/agent-scripts/__setup-devops-agent.sh "$home" "$devopsServer" "$devopsPAT" "$user"
  echo "DONE"
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


