using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Security.Principal.Windows;
using System.Security.Cryptography.X509Certificates;

namespace dotnetconsole
{
    public static class Test
    {
        public static int Main()
        {
            string pemCertWithPrivateKeyText = @"-----BEGIN PRIVATE KEY-----
        MIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQDlorhP38IfbA1F
        s3HXBBWDp+2LwA1A03NFAkPG96lAWqfIaKn87rdgwugg0lpiDeqrSuizQLSKeFvz
        Ev1Xb2ET09isy4pTxqVtZcI6y3EZgdPxAWyXKb1THcsDxOg3UV/uyA7ovBwnlpil
        KX8pE211oZdmmdKhk6zHIFBu+8iiW/PbQLL3X/VgpshGvsTV6i1ZWisnOz8KaLmK
        /98k7soPhOR8d5V01fqiT42gh5bF6IeuRqnXusWiJbD8Z6bz4i0/rSHRbtqCanN+
        sQkW5oAEeUsyoVCFkRm0MdPFDJXL40pS/tdvQom1wgdnkYrfoC2NBE0BYSHpHWEk
        E9jvA+wfAgMBAAECggEAC2NAZKWiCNDg753wfUs0jezb+NwzTA2wX7G3DrzlZc2Z
        GHUoSOTFr0S7IjWMSeSKyKfUVl9VFLDXTnyYl6dsVwFgc2E8oN7vATfUo/nGyA8K
        JZ9+dRiazO5jTPKM2YFtknfVVXFOrB/pvfcK2UyfVwAGdA4Yxe5+2PkfRAG0d2ij
        4SD/c0JIv98Mb91HirsoWCNYU63VAX3ZT9rAlUrX4YNowfbXl7DgBtLQVlJhb7qQ
        DgerRBH6nELpbCUthpckqykuADwOOEP6+WWCwTSBfTRUTHH5B/p0RXo5gO5L2afU
        n1R7ERaiTciQMsNNklLwjuKRx4kVW5F6hTKYp9bsQQKBgQD/BqPqJ+6y4VsoP5t0
        maxUukltGcNjtKzqzPkW3FaQTGdEtfaJNP0Kik2e7vg27X1FtWyrrvYEtEA6xQLl
        vMPWKAuYWxO8Ep0MPDCIn9LUqoFa+1ztcO2XsmlCqOJLv0O1nWF99o+w3KaUtH39
        obD2MNuIi2M5baZYxp02dm5DDwKBgQDmg0Dk1Poo9rm4X9q8Jfk7dBT1lUD6vQLz
        oTEgL7+s5QrbDyktIgkquMnB3Pvv/AXR6mE3XcLPPxD3oTZOi6n6Rzg1srBPuJHB
        lBGVNC5lbVwsBP2xieNKjw5vx0SJYjEf4cvKxovTTSUPMDOBdLexd1MXyl1OW7ve
        uL9t0aCF8QKBgQC1/lYhdyfYDgyQl3vn5PbfbCWULyuJztkLowUrdEhuJ6gWl8h9
        OAxQZkxoXF9US3z3rzYC+xPkXYHsrsWXj3MuNFu5+V0G/T2ICrgT+AJr89XmSLWT
        WOClPhPyFzWPGspGJC77xmXasExMmNXEl4wC1PEF63r+86ofdnWg9TlQrwKBgQCA
        f0hMh6DP9wF+kwiG+5AcoVARulrXq0ea8g9FhviNc7yCcsgcXa3If+wQQpS6qb3A
        z6vTxlOTXe+iat6wGFDvsIXYAT0ho5y00Uqf5s+6QKUx8LJPJrNqW6bLjsRY5UDH
        KgKFjfpiFs4C0nbTwnGn3wGV8Hvk82Qd+tTTbhjSsQKBgQCC86T3OyZ8hXCec9qr
        5nFHuEmAWqoDwmv/c5DfFT/ukjFunBLNVY8XaGPfb0ZSTeVpio5+9Ohc7X4XoMJ8
        YalW1DJmufUDbi951LROfXGSSzhiP4M09GA/ThsCLS62Kpc8ivt2yOPGL7CZFm/l
        tj5e8lzHa70CYXu/RdccbXmL3w==
        -----END PRIVATE KEY-----
        -----BEGIN CERTIFICATE-----
        MIICoTCCAYkCAgPoMA0GCSqGSIb3DQEBBQUAMBQxEjAQBgNVBAMMCUNMSS1Mb2dp
        bjAiGA8yMDE4MDYwODAzNTAzMFoYDzIwMTkwNjA4MDM1MDMyWjAUMRIwEAYDVQQD
        DAlDTEktTG9naW4wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDlorhP
        38IfbA1Fs3HXBBWDp+2LwA1A03NFAkPG96lAWqfIaKn87rdgwugg0lpiDeqrSuiz
        QLSKeFvzEv1Xb2ET09isy4pTxqVtZcI6y3EZgdPxAWyXKb1THcsDxOg3UV/uyA7o
        vBwnlpilKX8pE211oZdmmdKhk6zHIFBu+8iiW/PbQLL3X/VgpshGvsTV6i1ZWisn
        Oz8KaLmK/98k7soPhOR8d5V01fqiT42gh5bF6IeuRqnXusWiJbD8Z6bz4i0/rSHR
        btqCanN+sQkW5oAEeUsyoVCFkRm0MdPFDJXL40pS/tdvQom1wgdnkYrfoC2NBE0B
        YSHpHWEkE9jvA+wfAgMBAAEwDQYJKoZIhvcNAQEFBQADggEBAHm+A2fGWxi0lRty
        qZa00zvslO5VgMD8u5x93YDZZbHEez2pyUGJSbF+REH/zt5ondpOCSlNoesQjoQT
        H2jvFUR1MEwHv8yuskC65Pc9F3UJ18/OPNoZ3KFvXhAz0JDCnKhonigoj5GHPFCh
        QCriCI/dcmQWreA1HjoOO+Whjx5HmxI4OqezsihDBTsZwZt7mxXWfNfd4rQPU2D8
        2d5Aqe7II6Gh90iKLzhFJKXXXc/TlZfrd0eO2C86UMnj1loZADPNnymBBJMdHXCw
        SWBMh0mh7i0PBy+Rn/1KZvQ7aeqq0PG6s+OpEccXijmz4uhkz0s7Yb9n+MQSPekT
        jk2/wUg=
        -----END CERTIFICATE-----
        "; // just an example

            X509Certificate2 cert = PEMToX509.Convert(pemCertWithPrivateKeyText);
            return (cert.HasPrivateKey ? 1 : -1);
        }
    }

    internal static class PEMToX509
    {
        const string KEY_HEADER = "-----BEGIN RSA PRIVATE KEY-----";
        const string KEY_FOOTER = "-----END RSA PRIVATE KEY-----";

        internal static X509Certificate2 Convert(string pem)
        {
            try
            {
                byte[] pemCertWithPrivateKey = System.Text.Encoding.ASCII.GetBytes(pem);

                RSACryptoServiceProvider rsaPK = GetRSA(pem);

                X509Certificate2 cert = new X509Certificate2();
                cert.Import(pemCertWithPrivateKey, "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

                if (rsaPK != null)
                {
                    cert.PrivateKey = rsaPK;
                }

                return cert;
            }
            catch
            {
                return null;
            }
        }

        private static RSACryptoServiceProvider GetRSA(string pem)
        {
            RSACryptoServiceProvider rsa = null;

            if (IsPrivateKeyAvailable(pem))
            {
                RSAParameters privateKey = DecodeRSAPrivateKey(pem);


                System.Security.Principal.SecurityIdentifier everyoneSI = new System.Security.Principal.SecurityIdentifier(WellKnownSidType.WorldSid, null);
                CryptoKeyAccessRule rule = new CryptoKeyAccessRule(everyoneSI, CryptoKeyRights.FullControl, AccessControlType.Allow);

                CspParameters cspParameters = new CspParameters();
                cspParameters.KeyContainerName = "MY_C_NAME";
                cspParameters.ProviderName = "Microsoft Strong Cryptographic Provider";
                cspParameters.ProviderType = 1;
                cspParameters.Flags = CspProviderFlags.UseNonExportableKey | CspProviderFlags.UseMachineKeyStore;

                cspParameters.CryptoKeySecurity = new CryptoKeySecurity();
                cspParameters.CryptoKeySecurity.SetAccessRule(rule);

                rsa = new RSACryptoServiceProvider(cspParameters);
                rsa.PersistKeyInCsp = true;
                rsa.ImportParameters(privateKey);
            }

            return rsa;
        }

        private static bool IsPrivateKeyAvailable(string privateKeyInPEM)
        {
            return (privateKeyInPEM != null && privateKeyInPEM.Contains(KEY_HEADER)
                && privateKeyInPEM.Contains(KEY_FOOTER));
        }

        private static RSAParameters DecodeRSAPrivateKey(string privateKeyInPEM)
        {
            if (IsPrivateKeyAvailable(privateKeyInPEM) == false)
                throw new ArgumentException("bad format");

            string keyFormatted = privateKeyInPEM;

            int cutIndex = keyFormatted.IndexOf(KEY_HEADER);
            keyFormatted = keyFormatted.Substring(cutIndex, keyFormatted.Length - cutIndex);
            cutIndex = keyFormatted.IndexOf(KEY_FOOTER);
            keyFormatted = keyFormatted.Substring(0, cutIndex + KEY_FOOTER.Length);
            keyFormatted = keyFormatted.Replace(KEY_HEADER, "");
            keyFormatted = keyFormatted.Replace(KEY_FOOTER, "");
            keyFormatted = keyFormatted.Replace("\r", "");
            keyFormatted = keyFormatted.Replace("\n", "");
            keyFormatted = keyFormatted.Trim();

            byte[] privateKeyInDER = System.Convert.FromBase64String(keyFormatted);

            byte[] paramModulus;
            byte[] paramDP;
            byte[] paramDQ;
            byte[] paramIQ;
            byte[] paramE;
            byte[] paramD;
            byte[] paramP;
            byte[] paramQ;

            MemoryStream memoryStream = new MemoryStream(privateKeyInDER);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            ushort twobytes = 0;
            int elements = 0;
            byte bt = 0;

            try
            {
                twobytes = binaryReader.ReadUInt16();
                if (twobytes == 0x8130)
                    binaryReader.ReadByte();
                else if (twobytes == 0x8230)
                    binaryReader.ReadInt16();
                else
                    throw new CryptographicException("Wrong data");

                twobytes = binaryReader.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new CryptographicException("Wrong data");

                bt = binaryReader.ReadByte();
                if (bt != 0x00)
                    throw new CryptographicException("Wrong data");

                elements = GetIntegerSize(binaryReader);
                paramModulus = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                paramE = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                paramD = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                paramP = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                paramQ = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                paramDP = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                paramDQ = binaryReader.ReadBytes(elements);

                elements = GetIntegerSize(binaryReader);
                paramIQ = binaryReader.ReadBytes(elements);

                EnsureLength(ref paramD, 256);
                EnsureLength(ref paramDP, 128);
                EnsureLength(ref paramDQ, 128);
                EnsureLength(ref paramE, 3);
                EnsureLength(ref paramIQ, 128);
                EnsureLength(ref paramModulus, 256);
                EnsureLength(ref paramP, 128);
                EnsureLength(ref paramQ, 128);

                RSAParameters rsaParameters = new RSAParameters();
                rsaParameters.Modulus = paramModulus;
                rsaParameters.Exponent = paramE;
                rsaParameters.D = paramD;
                rsaParameters.P = paramP;
                rsaParameters.Q = paramQ;
                rsaParameters.DP = paramDP;
                rsaParameters.DQ = paramDQ;
                rsaParameters.InverseQ = paramIQ;

                return rsaParameters;
            }
            finally
            {
                binaryReader.Close();
            }
        }
        private static int GetIntegerSize(BinaryReader binary)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;

            bt = binary.ReadByte();

            if (bt != 0x02)
                return 0;

            bt = binary.ReadByte();

            if (bt == 0x81)
                count = binary.ReadByte();
            else if (bt == 0x82)
            {
                highbyte = binary.ReadByte();
                lowbyte = binary.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
                count = bt;

            while (binary.ReadByte() == 0x00)
                count -= 1;

            binary.BaseStream.Seek(-1, SeekOrigin.Current);

            return count;
        }
        private static void EnsureLength(ref byte[] data, int desiredLength)
        {
            if (data == null || data.Length >= desiredLength)
                return;

            int zeros = desiredLength - data.Length;

            byte[] newData = new byte[desiredLength];
            Array.Copy(data, 0, newData, zeros, data.Length);

            data = newData;
        }
    }
}