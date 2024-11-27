using CommonApplicationFramework.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CommonApplicationFramework.Common
{
    public class SecureModel
    {
        private readonly PasswordHasher _PasswordHasher = new PasswordHasher();
        public string Salt { get; set; }
        public string Password { get; set; }

        public SecureModel(string Password)
        {
            SetSaltAndPassword(Password);
        }

        private void SetSaltAndPassword(string password)
        {
            string salt; string hashedPassword;
            this._PasswordHasher.HashPassword(password, out hashedPassword, out salt);
            this.Salt = salt;
            this.Password = hashedPassword;
        }
        public static string Decrypt(string cryptedString)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");
            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }
    }
}
