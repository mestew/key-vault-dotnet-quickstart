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
        string APPLICATION_ID, CERT_THUMBPRINT;
        public KeyVault(string APPLICATION_ID, string CERT_THUMBPRINT) {
            this.APPLICATION_ID = APPLICATION_ID;
            this.CERT_THUMBPRINT = CERT_THUMBPRINT;
            _keyVaultClient = new KeyVaultClient(this.GetAccessToken);
        }
   
        public static ClientAssertionCertificate AssertionCert { get; set; }
        
        // This method is used to get a token from Azure Active Directory. 
        public async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var certByThumbprint = FindCertificateByThumbprint(this.CERT_THUMBPRINT);
            AssertionCert = new ClientAssertionCertificate(this.APPLICATION_ID, certByThumbprint);
            var result = await context.AcquireTokenAsync(resource, AssertionCert);
            return result.AccessToken;
        }


        public async Task CreateSecretKeyValuePair(string vaultBaseURL)
        {
            System.Console.WriteLine("Authenticating to Key Vault using ADAL Callback to create Secret Key Value Pair");
            System.Console.WriteLine(vaultBaseURL);
            KeyVaultClient kvClient = new KeyVaultClient(this.GetAccessToken);
            await kvClient.SetSecretAsync(vaultBaseURL, "TestKey", "TestSecret");
        }

        // In this method we first get a token from Azure Active Directory by using the self signed cert we created in our powershell commands
        // And then we pass that token to Azure Key Vault to authenticate the service principal to get access to the secrets
        // Finally we retrieve the secret value that was created previously 
        public void GetResult(string keyvaultUri)
        {
            try
            {
                var result = this._keyVaultClient.GetSecretAsync(keyvaultUri, "TestKey").Result.Value;
                System.Console.WriteLine("Secret Key retrieved is {0} and value is {1}, ", "TestKey", result);    
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        // Given a thumbprint this method finds the certificate       
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