using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace dotnetconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // Util.GetCert();
            // var result = CertificateHelper.FindCertificateByThumbprint("c9c1ffb41706ed59f5ea1a6dff942142c2996875");
            // var AssertionCert = new ClientAssertionCertificate("d719328f-8425-435d-a342-fb68147b4c9d", result);
            // System.Console.WriteLine(result);
            // var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(Util.GetAccessToken));           
            KeyVault sample = new KeyVault();
            KeyVault.GetCert();
            sample.GetResult();
        }
    }

    public class Util {

        public static ClientAssertionCertificate AssertionCert { get; set; }

        public static void GetCert()
        {
            var result = CertificateHelper.FindCertificateByThumbprint("c9c1ffb41706ed59f5ea1a6dff942142c2996875");
            AssertionCert = new ClientAssertionCertificate("d719328f-8425-435d-a342-fb68147b4c9d", result);
        }
        
        public static async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, AssertionCert);
            return result.AccessToken;
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
                var test = store.Certificates.Find(X509FindType.FindBySerialNumber, "111cabe1d82a91a5410209ea82bd498f", true);
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
