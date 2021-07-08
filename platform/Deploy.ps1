param(
    [Parameter(Mandatory, ParameterSetName='Gateway')]
    [Parameter(Mandatory, ParameterSetName='NoGateway')]
        $ResourceGroupName, 
    [Parameter(Mandatory, ParameterSetName='Gateway')]
    [Parameter(Mandatory, ParameterSetName='NoGateway')]
        $ResourcePrefix, 
        [Parameter(Mandatory, ParameterSetName='Gateway')]
        [Parameter(Mandatory, ParameterSetName='NoGateway')]
            $Location,
    [Parameter(Mandatory, ParameterSetName='Gateway')]
    [Parameter(Mandatory, ParameterSetName='NoGateway')]
        $ApimPubisherEmail,
    [ValidateSet('Developer','Premium')]
     $ApimSku = 'Developer',
    [Parameter(Mandatory, ParameterSetName='Gateway')]$WafPfxPath,
    [Parameter(Mandatory, ParameterSetName='Gateway')][SecureString]$WafPfxPassword,
    [Parameter(Mandatory, ParameterSetName='Gateway')]$WafHostname
)

function Deploy
{
    $preRequisites = . "./Create-PreRequisites.ps1" -ResourceGroupName $ResourceGroupName -ResourcePrefix $ResourcePrefix -Location $Location
    
    Write-Host "Setting up pre-requisites"
    Write-Host "----------------------------------"
    Write-Host "Deploying ARM template"
    Write-Host "$($preRequisites.ContainerUri)/platform"

    $DeployWaf = $false
    If ($WafPfxPath) {
        $DeployWaf = $true
        $WafPfxContents = [Convert]::ToBase64String([System.IO.File]::ReadAllBytes($WafPfxPath))
    } else {
        $WafPfxContents = ""
        $wafPfxPassword = ""
        $wafHostname = ""
    }

    New-AzResourceGroupDeployment `
        -ResourceGroupName $ResourceGroupName `
        -Mode Incremental `
        -Verbose `
        -TemplateUri "$($preRequisites.BlobUri)$($preRequisites.SasToken)" `
        -TemplateParameterObject @{ 
            storageBaseUri = "$($preRequisites.ContainerUri)/" 
            templateSasKey = $preRequisites.SasToken
            jumpboxPublicKey = $preRequisites.JumpboxPublicKey
            devopsAgentPublicKey = $preRequisites.DevOpsAgentPublicKey 
            publisherEmail = "$ApimPubisherEmail"
            storageAccountName = "$($ResourcePrefix)stg"
            resourcePrefix = $ResourcePrefix
            deployWaf = $DeployWaf
            wafPfxContents = $WafPfxContents
            wafPfxPassword = $WafPfxPassword
            wafHostname = $WafHostname
            apimSku = $ApimSku
        }
}

Deploy


