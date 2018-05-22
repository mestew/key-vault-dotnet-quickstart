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
            Console.WriteLine("Hello World!");
            var keyvaultUri = System.Environment.GetEnvironmentVariable("KEYVAULT_URI", EnvironmentVariableTarget.User);
            var APPLICATION_ID = System.Environment.GetEnvironmentVariable("APPLICATION_ID", EnvironmentVariableTarget.User);
            var CERT_THUMBPRINT = System.Environment.GetEnvironmentVariable("CERT_THUMBPRINT", EnvironmentVariableTarget.User);
            
            // Util.GetCert();
            // var result = CertificateHelper.FindCertificateByThumbprint("c9c1ffb41706ed59f5ea1a6dff942142c2996875");
            // var AssertionCert = new ClientAssertionCertificate("d719328f-8425-435d-a342-fb68147b4c9d", result);
            // System.Console.WriteLine(result);
            // var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(Util.GetAccessToken));           
            KeyVault sample = new KeyVault();
            var waitHandle = sample.CreateSecretKeyValuePair("https://" + System.Environment.GetEnvironmentVariable("KEYVAULT_URI", EnvironmentVariableTarget.User) + ".vault.azure.net/");    

            waitHandle.Wait();
            
            // KeyVault.GetCert();
            sample.GetResult();
        }
    }
}
