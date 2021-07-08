# APIm -> Function -> K8S (hosting legacy API)

This repo provides a sample Azure deployment demonstrating:

 - Azure API Management integrated with KeyVault
 - API Management JWT Bearer validation 
 - API Management Open API Import
 - API Management Self Hosted Gateway
 - Consuming a WS-* SOAP API
 - Private AKS Cluster Deployment
 - NGINX Ingres
 - App Service Plan (Premium tier for Private Link)
 - Logic App (Preview hosted in App Service Plan)
 - Monitoring via Log Analytics 

It optionally deploys
 - An Azure Web Application Firewall with public IP

It is not production quality. But it can be used to play and poke at a depoyment and to understand how to deploy all of this with Azure ARM templates.

## Platform Deployment from local machine

### Deployment with WAF
``` \platform\Deploy.ps1 -ResourceGroupName {{resourceGroupName}} -ResourcePrefix {{resourcePrefix}} -Location "{{azure location}}" -ApimPublisherEmail {{azure apim publisher email}} -WafPfxPath {{waf pfx file}} -WafHostname {{waf hostname}}  ```

### Deployment with no WAF
``` \platform\Deploy.ps1 -ResourceGroupName {{resourceGroupName}} -ResourcePrefix {{resourcePrefix}} -Location "{{azure location}}" -ApimPublisherEmail {{azure apim publisher email}} ``` 


We love trying new features and have used the AKS AAD Authorization feature that, as of the time of writing, is in preview. To register it:

```
#Register the feature against your subscription
az feature register --name EnableAzureRBACPreview  --namespace Microsoft.ContainerService

# Check the status using:
az feature list -o table --query "[?contains(name, 'Microsoft.ContainerService/EnableAzureRBACPreview')].{Name:name,State:properties.state}"

# Wait some time for this to register...
az provider register -n Microsoft.ContainerService
echo This takes a while use this command to check status 
echo az provider show -n Microsoft.ContainerService -otable
```

> If you are updating an existing cluster you will need to register the UpdateAzureRBACPreview feature

### Get ssh keys for jumpbox, and devops agent

When the platform pipeline has run it will create two ssh keys inside the blob storage container. ```jumpboxSshKey``` gets you into the jumpbox. From there you can jump using ``` devopsAgentSshKey ``` to the DevOps agent.

> TODO - better to put these in KeyVault. Haven't provisioned that service yet though.


## Azure Architecture
![apim-poc.drawio.png](./apim-poc.drawio.png)

## Flows
![logical-view.drawio.png](./logical-view.drawio.png)



## References

### API Management
For limitations on SOAP endpoints see [https://docs.microsoft.com/en-us/azure/api-management/api-management-api-import-restrictions](https://docs.microsoft.com/en-us/azure/api-management/api-management-api-import-restrictions)

### SOAP WSDL
We used the dotnet wrapper around svcutil to import wsdl into our .net 5 project ```dotnet tool install --global dotnet-svcutil```
