
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public class Util
{
    public static X509Certificate2 LoadCertificateFile(string filename)
    {
        using (System.IO.FileStream fs = System.IO.File.OpenRead(filename))
        {
            byte[] data = new byte[fs.Length];
            byte[] res = null;
            fs.Read(data, 0, data.Length);
            if (data[0] != 0x30)
            {
            res = GetPem("CERTIFICATE", data);
            }
            X509Certificate2 x509 = new X509Certificate2(res); //Exception hit here
            return x509;
        }      
    }   

    public static byte[] GetPem(string type, byte[] data)
    {
        // PemReader pem = new PemReader();
        string pem = Encoding.UTF8.GetString(data);
        string header = String.Format("-----BEGIN {0}-----\\n", type);
        string footer = String.Format("-----END {0}-----", type);
        int start = pem.IndexOf(header) + header.Length;
        int end = pem.IndexOf(footer, start);
        string base64 = pem.Substring(start, (end - start));
        return Convert.FromBase64String(base64);
    }

    public static string ConvertToBase64String(string data)
    {
        X509Certificate2 certificate = LoadCertificateFile("cert.pem");
        RSACryptoServiceProvider rsaCsp = (RSACryptoServiceProvider)certificate.PrivateKey;
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] signatureBytes = rsaCsp.SignData(dataBytes, "SHA1");
        return Convert.ToBase64String(signatureBytes);
    }
}