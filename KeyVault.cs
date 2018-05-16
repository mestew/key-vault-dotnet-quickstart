using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.KeyVault;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace dotnetconsole
{
    public class KeyVault
    {
        KeyVaultClient _keyVaultClient;
        public KeyVault() {
            _keyVaultClient = new KeyVaultClient(this.GetAccessToken);
        }
   
        public static ClientAssertionCertificate AssertionCert { get; set; }

        public static void GetCert()
        {
            // var result = CertificateHelper.FindCertificateByThumbprint("c9c1ffb41706ed59f5ea1a6dff942142c2996875");
            var result = CertificateHelper.FindCertificateByThumbprint("c9c1ffb41706ed59f5ea1a6dff942142c2996875");
            AssertionCert = new ClientAssertionCertificate("d719328f-8425-435d-a342-fb68147b4c9d", result);
        }
        
        public async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, AssertionCert);
            return result.AccessToken;
        }

        public void GetResult()
        {
            try
            {
                var result = this._keyVaultClient.GetSecretAsync("https://netsamplekeyvault.vault.azure.net/", "NETSampleSecret").Result.Value;
                System.Console.WriteLine(result);    
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static class CertificateHelper
        {  
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
}