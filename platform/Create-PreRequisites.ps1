[CmdletBinding()]
param(
    $ResourceGroupName, 
    $ResourcePrefix, 
    $Location
)

#Example commandline ./Deploy.ps1 -ResourceGroupName MyRG -Name example123 -Location australiaEast

Write-Host "Building pre-requisites for platform deployment"

Write-Host "ResourceGroupName: $ResourceGroupName"
Write-Host "ResourcePrefix: $ResourcePrefix"

$rg = Get-AzResourceGroup -Name $ResourceGroupName -Location $Location -ErrorAction SilentlyContinue
if ($rg -eq $null) {
    New-AzResourceGroup -Name $ResourceGroupName -Location $Location
}

Write-Host "Build storage account"
$storageAccountName = "$($ResourcePrefix)stg"
$act = Get-AzStorageAccount -ResourceGroupName $ResourceGroupName -Name $storageAccountName  -ErrorAction SilentlyContinue
if ($act -eq $null) {
    $act = New-AzStorageAccount -ResourceGroupName $ResourceGroupName -Name $storageAccountName -SkuName Standard_LRS -Location $Location -Kind StorageV2 -EnableHttpsTrafficOnly $true -AllowBlobPublicAccess $false
}

$ctx = $act.Context
$container = 'platformtemplates'

New-AzStorageContainer -Context $ctx -Name $container -ErrorAction SilentlyContinue
Get-ChildItem -Path . -File -Filter "*.json" -Recurse | ForEach-Object { 
    $blobName = "platform/$($_.Name)"
    Write-Host "Copying $($_.FullName) to $blobName"
    Set-AzStorageBlobContent -File $_.FullName -Container $container -Context $ctx -Blob $blobName -Force
}

Get-ChildItem -Path . -File -Filter "*.sh" -Recurse | ForEach-Object { 
    $blobName = "script/$($_.Name)"
    Write-Host "Copying $($_.FullName) to $blobName"
    Set-AzStorageBlobContent -File $_.FullName -Container $container -Context $ctx -Blob $blobName -Force
}

Get-ChildItem -Path ..\functions\ApiArm\policies\ -File -Filter "*.xml" | ForEach-Object { 
    $blobName = "apis/policies/$($_.Name)"
    Write-Host "Copying $($_.FullName) to $blobName"
    Set-AzStorageBlobContent -File $_.FullName -Container $container -Context $ctx -Blob $blobName -Force
}

Get-ChildItem -Path ..\functions\ApiArm\ -File -Filter "*.json" | ForEach-Object { 
    $blobName = "apis/api-arm/$($_.Name)"
    Write-Host "Copying $($_.FullName) to $blobName"
    Set-AzStorageBlobContent -File $_.FullName -Container $container -Context $ctx -Blob $blobName -Force
}

Get-ChildItem -Path ..\functions\ApiArm\open-api-definitions\ -File -Filter "*.json" -Recurse | ForEach-Object { 
    $blobName = "apis/open-api-definitions/$($_.Name)"
    Write-Host "Copying $($_.FullName) to $blobName"
    Set-AzStorageBlobContent -File $_.FullName -Container $container -Context $ctx -Blob $blobName -Force
}

Write-Host "Getting SAS token for deployment"
$token = New-AzStorageAccountSASToken -Context $ctx -Service Blob -ResourceType Object -Permission r -ExpiryTime ([System.DateTime]::Now).AddMinutes(180)
Write-Host "##vso[task.setvariable variable=SasToken;isSecret=true;isOutput=true;]$token"
$blob = Get-AzStorageBlob -Context $ctx -Container $container -Blob 'platform/platform-deploy.json'
Write-Host "##vso[task.setvariable variable=BlobUri;isSecret=false;isOutput=true;]$($blob.ICloudBlob.Uri.AbsoluteUri)"
$containerResource = Get-AzStorageContainer -Context $ctx -Name $container
Write-Host "##vso[task.setvariable variable=ContainerUri;isSecret=false;isOutput=true;]$($containerResource.CloudBlobContainer.Uri.AbsoluteUri)"

#Write a key pair to storage so we can access the jumpbox vm. Better way would be to put this in keyvault
$container = 'platformsecrets'
New-AzStorageContainer -Context $ctx -Name $container -ErrorAction SilentlyContinue
$keyName = 'jumpboxSshKey'
$publicKeyName = 'jumpboxSshKey.pub'
$publicKey = Get-AzStorageBlob -Context $ctx -Container $container -Blob $publicKeyName -ErrorAction SilentlyContinue
If ($null -eq $publicKey) {
    ssh-keygen -b 2048 -t rsa -f ./$keyName -q -N """"
    Set-AzStorageBlobContent -File ./$keyName -Container $container -Context $ctx -Blob $keyName -Force
    Set-AzStorageBlobContent -File ./$publicKeyName -Container $container -Context $ctx -Blob $publicKeyName -Force
    Write-Host "Generated Keypair for jumpbox virtual machine"
    $sshPublicKeyContent = (Get-Content .\$publicKeyName)
    Remove-Item ./$keyName
    Remove-Item ./$publicKeyName
}
else {
    $sshPublicKeyContent = ([System.IO.StreamReader]::new($publicKey.ICloudBlob.OpenRead()).ReadToEnd()).Trim()
}

Write-Host "##vso[task.setvariable variable=JumpboxPublicKey;isSecret=false;isOutput=true;]""$sshPublicKeyContent"""
Write-Host "Public Key: $sshPublicKeyContent"

$keyName = 'devopsAgentSshKey'
$publicKeyName = 'devopsAgentSshKey.pub'
$publicKey = Get-AzStorageBlob -Context $ctx -Container $container -Blob $publicKeyName -ErrorAction SilentlyContinue
If ($null -eq $publicKey) {
    ssh-keygen -b 2048 -t rsa -f ./$keyName -q -N """"
    Set-AzStorageBlobContent -File ./$keyName -Container $container -Context $ctx -Blob $keyName -Force
    Set-AzStorageBlobContent -File ./$publicKeyName -Container $container -Context $ctx -Blob $publicKeyName -Force
    Write-Host "Generated Keypair for devops agent virtual machine"
    $sshAdoAgentPublicKeyContent = (Get-Content .\$publicKeyName)
    Remove-Item ./$keyName
    Remove-Item ./$publicKeyName
}
else {
    $sshAdoAgentPublicKeyContent = ([System.IO.StreamReader]::new($publicKey.ICloudBlob.OpenRead()).ReadToEnd()).Trim()
}

Write-Host "##vso[task.setvariable variable=DevOpsAgentPublicKey;isSecret=false;isOutput=true;]""$sshAdoAgentPublicKeyContent"""
Write-Host "Public Key: $sshAdoAgentPublicKeyContent"

$keyName = 'aks-ingress-tls.key'
$publicKeyName = 'aks-ingress-tls.crt'
$tlsCertificate = Get-AzStorageBlob -Context $ctx -Container $container -Blob $publicKeyName -ErrorAction SilentlyContinue
if ($null -eq $tlsCertificate) {

    &openssl req -x509 -nodes -days 365 -newkey rsa:2048 -out ./aks-ingress-tls.crt -keyout ./aks-ingress-tls.key -subj "/CN=api.poc.internal/O=aks-ingress-tls" -addext "subjectAltName = DNS.1:apigateway.api.poc.internal,DNS.2:api.poc.internal"

    Set-AzStorageBlobContent -File ./$keyName -Container $container -Context $ctx -Blob $keyName -Force
    Set-AzStorageBlobContent -File ./$publicKeyName -Container $container -Context $ctx -Blob $publicKeyName -Force

    Write-Host "Generated keys for self-signed nginx certificate"

    Remove-Item ./$keyName
    Remove-Item ./$publicKeyName
}

Write-Host "Testing openssl"
&openssl req -x509 -nodes -days 365 -newkey rsa:2048 -out ./test-tls.crt -keyout ./test-tls.key -subj "/CN=api.poc.internal/O=aks-ingress-tls" -addext "subjectAltName = DNS.1:apigateway.api.poc.internal,DNS.2:api.poc.internal"


$returnParameters = @{
    SasToken             = $token
    BlobUri              = $blob.ICloudBlob.Uri.AbsoluteUri
    ContainerUri         = $containerResource.CloudBlobContainer.Uri.AbsoluteUri
    JumpboxPublicKey     = $sshPublicKeyContent
    DevOpsAgentPublicKey = $sshAdoAgentPublicKeyContent
}

return $returnParameters
