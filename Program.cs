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
}
