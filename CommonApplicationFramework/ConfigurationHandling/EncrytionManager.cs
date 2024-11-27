using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CommonApplicationFramework.ConfigurationHandling
{
    public static class EncrytionManager
    {
        public static string Encrypt(string textToEncrypt,string encrytionKey)
        {
            try
            {
                byte[] bytes = ASCIIEncoding.ASCII.GetBytes(encrytionKey);
                if (String.IsNullOrEmpty(textToEncrypt))
                {
                    throw new ArgumentNullException("The string which needs to be encrypted can not be null.");
                }
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
                StreamWriter writer = new StreamWriter(cryptoStream);
                writer.Write(textToEncrypt);
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();
                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
        
        public static string Decrypt(string encryptedText,string encryptionKey)
        {
            try
            {
                byte[] bytes = ASCIIEncoding.ASCII.GetBytes(encryptionKey);
                if (String.IsNullOrEmpty(encryptedText))
                {
                    throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
                }
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(encryptedText));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
