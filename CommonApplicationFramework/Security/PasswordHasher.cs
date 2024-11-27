using System;
using System.Security.Cryptography;

namespace CommonApplicationFramework.Security
{
    public class PasswordHasher
    {
        // The number of bytes used for the password salt.
        private readonly int _SaltLength = 20;

        // The number of bytes used for hashed password.
        private readonly int _HashLength = 20;

        // The number of times to hash the password.
        private readonly int _HashIterations = 1000;

        public bool ValidatePassword(string unhashedPassword, string salt, string hashedPassword)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(unhashedPassword, saltBytes, this._HashIterations);
            byte[] hashBytes = pbkdf2.GetBytes(this._HashLength);
            string hash = Convert.ToBase64String(hashBytes);
            //// Security Decisions For String Comparisons
            ////
            //// If you are making a security decision (such as whether to allow access to a system resource) based on the 
            //// result of a string comparison or a case change, you should not use the invariant culture. Instead, you 
            //// should perform a case-sensitive or case-insensitive ordinal comparison by calling a method that includes 
            //// a StringComparison parameter and supplying either StringComparison.Ordinal or 
            //// StringComparison.OrdinalIgnoreCase as an argument. Code that performs culture-sensitive string operations 
            //// can cause security vulnerabilities if the current culture is changed or if the culture on the computer 
            //// that is running the code differs from the culture that is used to test the code. In contrast, an ordinal 
            //// comparison depends solely on the binary value of the compared characters.
            ////
            //// Source: http://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.invariantculture.aspx
            //// TODO: Probably need to replace this with a time-constant string comparison to
            //// avoid timing attacks (see TimeConstantByteCompare below);
            return hash.Equals(hashedPassword, StringComparison.Ordinal);
        }

        public void HashPassword(string unhashedPassword, out string hashedPassword, out string salt)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(unhashedPassword, this._SaltLength, this._HashIterations);
            salt = Convert.ToBase64String(pbkdf2.Salt);
            hashedPassword = Convert.ToBase64String(pbkdf2.GetBytes(this._HashLength));
        }
    }
}
