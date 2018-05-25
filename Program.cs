using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.ObjectModel;

namespace dotnetconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This Application must be run after running the powershell script Setup.ps1!");
            Console.WriteLine("This DotNet Console Application authenticates to Key Vault!");
            Console.WriteLine("It also creates a Secret Key Value Pair!");
            Console.WriteLine("And then it gets the Secret Key Value Pair!");
            var keyvaultUri = System.Environment.GetEnvironmentVariable("KEYVAULT_URI", EnvironmentVariableTarget.User);
            var APPLICATION_ID = System.Environment.GetEnvironmentVariable("APPLICATION_ID", EnvironmentVariableTarget.User);
            var CERT_THUMBPRINT = System.Environment.GetEnvironmentVariable("CERT_THUMBPRINT", EnvironmentVariableTarget.User);
            
            KeyVault keyVaultObj = new KeyVault();
            var VaultName = "https://" + keyvaultUri + ".vault.azure.net/";
            Console.WriteLine("Vault URI is! {0}", VaultName);
            var waitHandle = keyVaultObj.CreateSecretKeyValuePair(VaultName);    

            Console.WriteLine("Wait method is invoked to wait for Secret Key Value pair to be created");
            waitHandle.Wait();
            Console.WriteLine("Secret Key Value pair is now created");
            
            keyVaultObj.GetResult(VaultName);
        }
    }
}
