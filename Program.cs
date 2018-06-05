using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace dotnetconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"This Application must be run after running the powershell script Setup.ps1!
                                This DotNet Console Application authenticates to Key Vault!
                                It also creates a Secret Key Value Pair!
                                And then it gets the Secret Key Value Pair!");
            
            bool isWindows = System.Runtime.InteropServices.RuntimeInformation
                                               .IsOSPlatform(OSPlatform.Windows);
            string KEYVAULT_URI = String.Empty;
            string APPLICATION_ID = String.Empty;
            string CERT_THUMBPRINT = String.Empty;
            if(!isWindows)
            {
                KEYVAULT_URI = System.Environment.GetEnvironmentVariable("VAULT_NAME", EnvironmentVariableTarget.User);
                APPLICATION_ID = System.Environment.GetEnvironmentVariable("APPLICATION_ID", EnvironmentVariableTarget.User);
                CERT_THUMBPRINT = System.Environment.GetEnvironmentVariable("CERT_THUMBPRINT", EnvironmentVariableTarget.User);    
            }
            else 
            {
                GetVariablesFromJSON();
            }
            
            KeyVault keyVaultObj = new KeyVault();
            var VaultName = "https://" + KEYVAULT_URI + ".vault.azure.net/";
            
            var waitHandle = keyVaultObj.CreateSecretKeyValuePair(VaultName);    

            Console.WriteLine("Vault URI is! {0}", VaultName);
            Console.WriteLine("Wait method is invoked to wait for Secret Key Value pair to be created");

            waitHandle.Wait();

            Console.WriteLine("Secret Key Value pair is now created");
            
            keyVaultObj.GetResult(VaultName);
        }

        private static Tuple<string, string, string> GetVariablesFromJSON()
        {
            var ServicePrincipalJSON = Directory.GetCurrentDirectory() + "\\ServicePrincipal.json";
            var CertThumbprintJSON = Directory.GetCurrentDirectory() + "CertThumbprint.json";
            var VaultJSON = Directory.GetCurrentDirectory() + "ServicePrincipal.json";
            if(File.Exists(ServicePrincipalJSON))
            {
                ProcessFile(ServicePrincipalJSON, "APP_ID");
            }

            return new Tuple<string, string, string>(ProcessFile(ServicePrincipalJSON, "APP_ID"), CertThumbprintJSON, VaultJSON);
        }

        private static string ProcessFile(string fileName, string valueToLookFor)
        {
            var result = "";

            
            using (StreamReader SPJson = File.OpenText(fileName))
            {
                string json = SPJson.ReadToEnd();
                dynamic stuff = JsonConvert.DeserializeObject(json);
            }
            
            // using (JsonTextReader reader = new JsonTextReader(SPJson))
            // {
            //     JObject o2 = (JObject) JToken.ReadFrom(reader);
            //     result = o2.SelectToken(valueToLookFor).Value<string>();
            // }
            return result;
        }
    }
}