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
 - An Azure DevOps agent

It is not production quality. But it can be used to play and poke at a depoyment and to understand how to deploy all of this with Azure ARM templates.

There are sample Azure DevOps pipelines in the repo. The sample doesn't need these to function, but if you want to clone the repo, and make changes to the APIs, then they could be used as a starting point to wrap CI/CD pipelines around the sample.

## Platform Deployment from local machine

### Simple Deployment

>>This is the simplest way to run the sanmple.

``` \platform\Deploy.ps1 -ResourceGroupName {{resourceGroupName}} -ResourcePrefix {{resourcePrefix}} -Location "{{azure location}}" -ApimPublisherEmail {{azure apim publisher email}} ``` 

### Deployment with devops agent and WAF

>> Requires a TLS certificate with a CN / SAN matching the hostname parameter

``` \platform\Deploy.ps1 -ResourceGroupName {{resourceGroupName}} -ResourcePrefix {{resourcePrefix}} -Location "{{azure location}}" -ApimPublisherEmail {{azure apim publisher email}} -DevOpsServerUrl {{devops server uri}} -DevOpsServerPAT {{devops personal access token for a build agent}} -WafPfxPath {{waf pfx file}} -WafHostname {{waf hostname}} ```

### Deployment with devops agent

>> Requires a PAT token from Azure Devops

``` \platform\Deploy.ps1 -ResourceGroupName {{resourceGroupName}} -ResourcePrefix {{resourcePrefix}} -Location "{{azure location}}" -ApimPublisherEmail {{azure apim publisher email}} -DevOpsServerUrl {{devops server uri}} -DevOpsServerPAT {{devops personal access token for a build agent}}  ```

### Deployment with WAF
``` \platform\Deploy.ps1 -ResourceGroupName {{resourceGroupName}} -ResourcePrefix {{resourcePrefix}} -Location "{{azure location}}" -ApimPublisherEmail {{azure apim publisher email}} -WafPfxPath {{waf pfx file}} -WafHostname {{waf hostname}}  ```

## Required Az features 

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

## Running the sample

The sample deploys into a Virtual Network and without the WAF is inaccessible publically. The simplest way to access it is by using a SSH tunnel to create a SOCKS proxy to the jumpbox.

The SSH key is inside a storage account deployed into the resource group. The continer is ```platformsecrets```. The path is ```jumpboxSshKey```. Grab this and put it somewhere on your local disk.

Next fetch the public IP address associated with the jumpbox. It's a resource inside the resource group, of type 'Public IP address', and there's only one.

Now you're ready to setup the tunnel. Execute this command from a shell:

```
ssh -D 1337 -C -N -i <path>/jumpboxSshKey jumpboxadmin@<ip-address>
```

Now you can open Google Chrome up using this proxy. On Windows I had to kill all running Chrome instances. When you've done that, fire this command at your shell:

```
& 'C:\Program Files\Google\Chrome\Application\chrome.exe' --proxy-server="socks5://localhost:1337"
```

## Accessing the sample apps
The entry app will have a name something like ```sample-consumer-app-<random>```. Go and find this inside the resource group you deployed to in the Azure Portal. 

Navigate to ```https://sample-consumer-app-<random>.azurewebsites.net```. You will find yourself bounced to a sign-in page. This is a private instance of Identity Server.

There are two users you can sign in as:
| Username | Password |
| --- | --- |
| fred | p@ssw0rd |
| graeme | p@ssw0rd |

And that's it. You should be able to search on Ids from 12345678, and a thousand or so above that! The data is all deterministic.

## Azure DevOps

The sample deploys and runs without any requirements on CI/CD pipelines. If you choose to deploy a DevOps agent then you'll need to do some configuration inside Azure DevOps to get the pipelines to run.

### Setup 'Platform-POC' Library Group

Create a new library group called 'Platform-POC' with the following variables:

| Name | Definition |
| --- | --- |
| Location | Data centre to host in, e.g. Australia East |
| Subscription | ID of the subscription |
| ResourceGroup | Name of the resource group to deploy to |
| ApimPublisherEmail | Email address to associate with Azure Api Management service |
| ResourcePrefix | Simple prefix to help uniquely identify the resource (e.g. grf01) |
| DevopsServerUrl | Url of your devops organisation so we can setup a build agent (e.g. https://dev.azure.com/myorg/) |
| DevopsServerPAT | A DevOps Personal Access Token for setting up the build agent (https://docs.microsoft.com/en-us/azure/devops/pipelines/agents/v2-linux?view=azure-devops) |

### Setup DevOps Service Connections

Create the following Service Connections for ARM deployment that can access the resource-group named above.

| Service Connection Name | Type | Reason |
| --- | --- | --- |
| PoCServiceConnection | Azure Resource Manager | Deploys the resources |

### Import the DeOps Pipeline

Create an azure pipeline using the ```./platform/platform-deploy-pipeline.yaml``` file, and run the pipeline.

### Get ssh keys for jumpbox, and devops agent

When the platform pipeline has run it will create two ssh keys inside the blob storage container. ```jumpboxSshKey``` gets you into the jumpbox. From there you can jump using ``` devopsAgentSshKey ``` to the DevOps agent.

> TODO - better to put these in KeyVault. Haven't provisioned that service yet though.

### Setup the DevOps agent on the DevOps vm

Install the devops agent onto the Devops Agent machine as described here https://docs.microsoft.com/en-us/azure/devops/pipelines/agents/v2-linux?view=azure-devops


## Sample API Deployment in Azure DevOps

### Step 1

Create a new library group called 'Runtime-POC' with the following variables. These are needed to deploy the APIs and apps that run on the platform.

| Name | Definition |
| --- | --- |
| ImageRepository | Fully qualified name of the private image repository created in Platform deployment (e.g. myacr.azurecr.io) |
| AksApplicationInsightsConnectionString | Connection string to the AKS application insights |
| ApiManagementName | Api Management Resource name |
| FunctionApp | Function App site name |
| IdentityServerApp | Identity Server App Resource name |
| SampleWebApp | Sample Web App site name |
| KeyvaultName | Key Vault name |

### Step 2

Create the following Service Connections to allow Azure Devops pipelines to push to your container registry.

| Service Connection Name | Type | Reason |
| --- | --- | --- |
| PoCAcrServiceConnection | Docker Registry | Pushes the sample API to the private Azure Container Registry |

### Step 3

Create azure pipelines using the following yaml files:
```
./apis/deploy-api.yaml #deploys APIS to AKS
./functions/functions-pipeline.yaml #deploys Identity Server
./identity/identityserver-pipeline.yaml #deploys functions, apis, and the sample web app
``` 
Run the pipelines

> We use Kubernetes RBAC Authorization and a managed identity on the Devops VM to authenticate / authorize kubectl requests. This is currently wired up in a script extension om the devops agent.

## Azure Architecture
![apim-poc.drawio.png](./apim-poc.drawio.png)

## Flows
![logical-view.drawio.png](./logical-view.drawio.png)



## References

### API Management
For limitations on SOAP endpoints see [https://docs.microsoft.com/en-us/azure/api-management/api-management-api-import-restrictions](https://docs.microsoft.com/en-us/azure/api-management/api-management-api-import-restrictions)

### SOAP WSDL
We used the dotnet wrapper around svcutil to import wsdl into our .net 5 project ```dotnet tool install --global dotnet-svcutil```
