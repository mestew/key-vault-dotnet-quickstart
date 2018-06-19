
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public class Util
{
    public static X509Certificate2 ConvertFromPfxToPem(string filename)
    {
        using (System.IO.FileStream fs = System.IO.File.OpenRead(filename))
        {
            byte[] data = new byte[fs.Length];
            byte[] res = null;
            byte[] rsaKeyBits = null;
            fs.Read(data, 0, data.Length);
            if (data[0] != 0x30)
            {
                res = GetPem("CERTIFICATE", data);
                rsaKeyBits = GetPem("PRIVATE KEY", data);
            }
            RSACryptoServiceProvider rsaPrivateKey = CreateRsaProviderFromPrivateKey(GetSubstringFromCert("PRIVATE KEY", data));
            X509Certificate2 x509 = new X509Certificate2(res); //Exception hit here
            var finalCert = x509.CopyWithPrivateKey(rsaPrivateKey);
            return finalCert;
        }      
    }   

    private static byte[] GetPem(string type, byte[] data)
    {
        string base64 = GetSubstringFromCert(type, data);
        base64 = base64.Replace('-', '+');
        base64 = base64.Replace('_', '/');
        return Convert.FromBase64String(base64);
    }


    private static string GetSubstringFromCert(string type, byte[] data)
    {
        string pem = Encoding.UTF8.GetString(data);
        string header = String.Format("-----BEGIN {0}-----", type);
        string footer = String.Format("-----END {0}-----", type);
        int start = pem.IndexOf(header) + header.Length;
        int end = pem.IndexOf(footer, start);
        string base64 = pem.Substring(start, (end - start));
        return base64;
    }

    private static string ConvertToBase64String(string data)
    {
        X509Certificate2 certificate = ConvertFromPfxToPem("cert.pem");
        RSACryptoServiceProvider rsaCsp = (RSACryptoServiceProvider)certificate.PrivateKey;
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] signatureBytes = rsaCsp.SignData(dataBytes, "SHA1");
        return Convert.ToBase64String(signatureBytes);
    }

    private static RSACryptoServiceProvider CreateRsaProviderFromPrivateKey(string privateKey)
    {
        var privateKeyBits = System.Convert.FromBase64String(privateKey);

        var RSA = new RSACryptoServiceProvider();
        var RSAparams = new RSAParameters();

        using (BinaryReader binr = new BinaryReader(new MemoryStream(privateKeyBits)))
        {
            byte bt = 0;
            ushort twobytes = 0;
            twobytes = binr.ReadUInt16();
            if (twobytes == 0x8130)
                binr.ReadByte();
            else if (twobytes == 0x8230)
                binr.ReadInt16();
            else
                throw new Exception("Unexpected value read binr.ReadUInt16()");

            twobytes = binr.ReadUInt16();
            if (twobytes != 0x0102)
                throw new Exception("Unexpected version");

            bt = binr.ReadByte();
            if (bt != 0x00)
                throw new Exception("Unexpected value read binr.ReadByte()");

            RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
            RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
            RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
            RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
            RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
            RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
            RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
            RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
        }

        RSA.ImportParameters(RSAparams);
        return RSA;
    }

    private static int GetIntegerSize(BinaryReader binr)
    {
        byte bt = 0;
        byte lowbyte = 0x00;
        byte highbyte = 0x00;
        int count = 0;
        bt = binr.ReadByte();
        if (bt != 0x02)
            return 0;
        bt = binr.ReadByte();

        if (bt == 0x81)
            count = binr.ReadByte();
        else
            if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

        while (binr.ReadByte() == 0x00)
        {
            count -= 1;
        }
        binr.BaseStream.Seek(-1, SeekOrigin.Current);
        return count;
    }
}