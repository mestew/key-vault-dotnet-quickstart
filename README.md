# Azure Key Vault - Getting Started in .NET
This sample demonstrates how to get started with Key Vault in .NET
This project allows you to do 
- Create an application in Azure Active Directory
- Create a Service Principal associated with the application 
- Create a Key Vault
- Give permissions to the Service Principal created above to access Key Vault 
- Create a secret in Key Vault
- Read the secret from Key Vault

### Prerequisites
If you don't have an Azure subscription, please create a **[free account](https://azure.microsoft.com/free/?ref=microsoft.com&amp;utm_source=microsoft.com&amp;utm_medium=docs)** before you begin.
In addition you would need

* [.NET Core](https://www.microsoft.com/net/learn/get-started/windows)
    * Please install .NET Core. This can be run on Windows, Mac and Linux.
* [Git](https://www.git-scm.com/)
    * Please download git from [here](https://git-scm.com/downloads).
* [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)
    * For the purpose of this tutorial we would work with Azure CLI which is available on Windows, Mac and Linux

### Installation
#### On Windows
- Download the powershell command in this repo (Named Setup.ps1) in administrator mode
- Go to C:\Temp folder. Find the cert named .pfx and install it on your machine (by right clicking on the .pfx file and selecting Install)
- Clone this repository. Once cloned open the repo in any text editor and run the following command w.r.t that folder
    - dotnet restore
    - dotnet run

### Quickstart
#### On Windows
- Download the powershell command in this repo (Named Setup.ps1) in administrator mode
- Go to C:\Temp folder by using cd C:\temp command. 
- Find the cert named .pfx in that folder and install it on your machine (by right clicking on the .pfx file and selecting Install)
- Clone this repository. Once cloned open the repo in any text editor and run the following command w.r.t that folder
    - dotnet restore
    - dotnet run

### What does this code do?
- This sample will show you how to create a test key and secret in Key Vault
- It will also show you how to retrieve the secret from Key Vault

- Next if we need to point the users to different section of code (in a different file) we still have the code copy here and explain what it does
- This section should feel like a story where we explain 
  - What concepts we want users to understand?
  - What sections of code we want users to focus on?
  - As a result Best practices that we want users to follow

## Resources
(Any additional resources that we want users to read)
- Link to Azure Key Vault 
- Link to Azure Key Vault Roadmap
