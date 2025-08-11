// filename: AesCrypto.cs
using System;
using System.Drawing.Printing;
using System.IO;
using System.Security.Cryptography;
using System.Text;

    public class AesCrypto
    {
    static string? _KeyString = Environment.GetEnvironmentVariable("WOONG_SECRET_KEY");
    private static byte[] _Key
    {
        get
        {
            Console.WriteLine(_KeyString);
            return Encoding.UTF8.GetBytes(_KeyString);
        }
    }
        public static string Encrypt(string plainText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            Console.WriteLine(_KeyString.Length);
            aesAlg.Key = _Key;
            aesAlg.GenerateIV(); // Random 16-byte IV
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            using (var encryptor = aesAlg.CreateEncryptor())
            using (var ms = new MemoryStream())
            {
                // First write the IV
                ms.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

        public static string Decrypt(string base64CipherText)
        {
            byte[] fullCipher = Convert.FromBase64String(base64CipherText);
            byte[] iv = new byte[16];
            byte[] cipherText = new byte[fullCipher.Length - 16];

            Array.Copy(fullCipher, iv, 16);
            Array.Copy(fullCipher, 16, cipherText, 0, cipherText.Length);

            Console.WriteLine(_KeyString.Length);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _Key;
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (var decryptor = aesAlg.CreateDecryptor())
                using (var ms = new MemoryStream(cipherText))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }