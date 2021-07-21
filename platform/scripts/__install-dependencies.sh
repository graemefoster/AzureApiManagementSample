#!/bin/bash

home=$1
aksClusterName=$2
resourceGroupName=$3

echo "Installing unzip"
sudo apt install -y unzip

echo "Installing jq"
sudo apt install -y jq

echo "Fetching kubelogin"
curl -L https://github.com/Azure/kubelogin/releases/download/v0.0.9/kubelogin-linux-amd64.zip -o $home/downloads/kubelogin.zip
sudo unzip -o $home/downloads/kubelogin.zip -d $home/kubelogin/

echo "Getting credentials for AKS"
az aks get-credentials --overwrite-existing --name $aksClusterName --resource-group $resourceGroupName --file $home/.kube/config

echo "Installing Azure CLI"
sudo az aks install-cli 

echo "Installing Docker (https://docs.docker.com/engine/install/ubuntu/)"
sudo apt-get install \
    apt-transport-https \
    ca-certificates \
    curl \
    gnupg \
    lsb-release

curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg

cho \
  "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu \
  $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

sudo apt-get update
 sudo apt-get install docker-ce docker-ce-cli containerd.io

echo "Installed Docker"

echo "Running kubelogin"
export KUBECONFIG=$home/.kube/config
$home/kubelogin/bin/linux_amd64/kubelogin convert-kubeconfig -l msi

echo "Installing Helm"
curl https://raw.githubusercontent.com/helm/helm/master/scripts/get-helm-3 | bash

