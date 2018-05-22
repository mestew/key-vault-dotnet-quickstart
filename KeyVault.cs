using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.KeyVault;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure.KeyVault.Models;

namespace dotnetconsole
{
    public class KeyVault
    {
        KeyVaultClient _keyVaultClient;
        public KeyVault() {
            _keyVaultClient = new KeyVaultClient(this.GetAccessToken);
        }
   
        public static ClientAssertionCertificate AssertionCert { get; set; }
        
        public async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var certByThumbprint = FindCertificateByThumbprint(System.Environment.GetEnvironmentVariable("CERT_THUMBPRINT", EnvironmentVariableTarget.User));
            AssertionCert = new ClientAssertionCertificate(System.Environment.GetEnvironmentVariable("APPLICATION_ID", EnvironmentVariableTarget.User), certByThumbprint);
            var result = await context.AcquireTokenAsync(resource, AssertionCert);
            return result.AccessToken;
        }


        public async Task CreateSecretKeyValuePair(string vaultBaseURL)
        {
            System.Console.WriteLine("Authenticating to Key Vault using ADAL Callback");
            System.Console.WriteLine(vaultBaseURL);
            KeyVaultClient kvClient = new KeyVaultClient(this.GetAccessToken);
            await kvClient.SetSecretAsync(vaultBaseURL, "TestKey", "TestVault");
        }

        public void GetResult()
        {
            try
            {
                var keyvaultUri = "https://" + System.Environment.GetEnvironmentVariable("KEYVAULT_URI", EnvironmentVariableTarget.User) + ".vault.azure.net/";
                var APPLICATION_ID = System.Environment.GetEnvironmentVariable("APPLICATION_ID", EnvironmentVariableTarget.User);
                var CERT_THUMBPRINT = System.Environment.GetEnvironmentVariable("CERT_THUMBPRINT", EnvironmentVariableTarget.User);
                var result = this._keyVaultClient.GetSecretAsync(keyvaultUri, "TestKey").Result.Value;
                System.Console.WriteLine(result);    
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
      
        public static X509Certificate2 FindCertificateByThumbprint(string findValue)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindByThumbprint,
                    findValue, false); // Don't validate certs, since the test root isn't installed.
                if (col == null || col.Count == 0 )
                    return null;
                return col[0];
            }
            finally
            {
                store.Close();
            }
        }
    }
}