#!/bin/bash

echo "Building pre-requisites for platform deployment"

echo "ResourceGroupName: $RESOURCEGROUP"
echo "ResourcePrefix: $RESOURCEPREFIX"

echo "Location: $LOCATION"

echo "Build storage account"
storageAccountName=$RESOURCEPREFIX"stg"

echo "storageAccountName: $storageAccountName"

az login --identity

#figure out way to check if storage account already exists
az storage account create --name $storageAccountName --location "$LOCATION" --resource-group $RESOURCEGROUP --sku Standard_LRS --https-only true --kind StorageV2 --allow-blob-public-access false || echo "Storage account already exists"

container="platformtemplates"

az storage container create -n $container --account-name $storageAccountName

files="./functions/ApiArm/policies/*.xml"
destination_folder='apis/policies/'

for f in $files
do
    echo "Uploading $f file..."
    az storage blob upload --account-name $storageAccountName --container-name $container --file $f --name $destination_folder$(basename $f)
done

echo "List all blobs in container..."
az storage blob list -c $container --account-name $storageAccountName

tokenexpiry=$(date -d '+20 minutes' +"%Y-%m-%dT%TZ")
echo "Getting SAS token for api deployment"
token=$(az storage container generate-sas --name $container --permissions r --expiry $tokenexpiry --account-name $storageAccountName)
token="?${token:1:-1}"
echo "Token: $token"
echo "##vso[task.setvariable variable=SasToken;isSecret=false;isOutput=true;]$token"

containerUri="https://"$storageAccountName".blob.core.windows.net/"$container
echo "ContainerUri: $containerUri"
echo "##vso[task.setvariable variable=ContainerUri;isSecret=false;isOutput=true;]$containerUri"
