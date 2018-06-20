
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

public class Util
{
    public static X509Certificate2 ConvertFromPfxToPem(string filename)
    {
        AsymmetricCipherKeyPair sample;
        using (System.IO.FileStream fs = System.IO.File.OpenRead(filename))
        {
            byte[] data = new byte[fs.Length];
            byte[] res = null;
            fs.Read(data, 0, data.Length);
            if (data[0] != 0x30)
            {
                res = GetPem("CERTIFICATE", data);
                // rsaKeyBits = GetPem("PRIVATE KEY", data);
            }
            X509Certificate2 x509 = new X509Certificate2(res); //Exception hit here
            return x509;
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
        base64 = base64.Replace(System.Environment.NewLine, "");
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

    public static RSACryptoServiceProvider PemFileReader(){
        RsaPrivateCrtKeyParameters keyParams;
        using (var reader = File.OpenText("cert.pem")) // file containing RSA PKCS1 private key
        {
            keyParams = ((RsaPrivateCrtKeyParameters)new PemReader(reader).ReadObject());
        }

        
        RSAParameters rsaParameters = new RSAParameters();

        rsaParameters.Modulus = keyParams.Modulus.ToByteArrayUnsigned();
        rsaParameters.P = keyParams.P.ToByteArrayUnsigned();
        rsaParameters.Q = keyParams.Q.ToByteArrayUnsigned();
        rsaParameters.DP = keyParams.DP.ToByteArrayUnsigned();
        rsaParameters.DQ = keyParams.DQ.ToByteArrayUnsigned();
        rsaParameters.InverseQ = keyParams.QInv.ToByteArrayUnsigned();
        rsaParameters.D = keyParams.Exponent.ToByteArrayUnsigned();
        rsaParameters.Exponent = keyParams.PublicExponent.ToByteArrayUnsigned();

        RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(2048);
        rsaKey.ImportParameters(rsaParameters);
        return rsaKey;
    }

    private static RSACryptoServiceProvider CreateRsaProviderFromPrivateKey(string privateKey)
    {
        var RSA = new RSACryptoServiceProvider();
        try {
            privateKey = privateKey.Replace('-', '+');
            privateKey = privateKey.Replace('_', '/');
            var privateKeyBits = System.Convert.FromBase64String(privateKey);
            
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
        } catch(Exception ex){
            Console.WriteLine(ex.Message);
        }
        return RSA;
    }

    private static int GetIntegerSize(BinaryReader binr)
    {
        byte bt = 0;
        byte lowbyte = 0x00;
        byte highbyte = 0x00;
        int count = 0;
        bt = binr.ReadByte();
        // if (bt != 0x02)
        //     return 0;
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