using CommonApplicationFramework.ConfigurationHandling;
using CommonApplicationFramework.Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CommonApplicationFramework.Security
{
	public class Helper
	{

		private readonly bool hashEnabled = true; // Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("HashEnabled")).Value.ToString());

		private static readonly string sKey = "S[SB{/b#%]^,85ACutP7Muct";
		private static readonly string encryptionKey = sKey.Substring(0, 8) + sKey.Substring(sKey.Length - 8, 8);
		private static readonly byte[] Key = Encoding.UTF8.GetBytes(encryptionKey);
		private static readonly byte[] Vector = Encoding.UTF8.GetBytes(encryptionKey);
		private static Helper instance = null;
		private static ICryptoTransform _encryptor;
		private static ICryptoTransform _decryptor;
		private static UTF8Encoding encoder;

		public static Helper Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Helper();
					var rm = new RijndaelManaged();
					rm.Mode = CipherMode.CBC;
					rm.Padding = PaddingMode.PKCS7;
					_encryptor = rm.CreateEncryptor(Key, Vector);
					_decryptor = rm.CreateDecryptor(Key, Vector);					 

					encoder = new UTF8Encoding();
				}
				return instance;
			}
		}

		public string Encrypt(string unencrypted)
		{
			return string.IsNullOrEmpty(unencrypted) ? "" :
				Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
		}

		public string Decrypt(string encrypted)
		{
			return string.IsNullOrEmpty(encrypted) ? "" :
				encoder.GetString(Decrypt(Convert.FromBase64String(encrypted.Replace(" ", "+"))));

		}

		public string Hash(string plainText)
		{
			//LogManager.Log(plainText + "hash");
			if (plainText == "0") return "0";
			if (string.IsNullOrEmpty(plainText.Trim())) return "";
				
			return hashEnabled ? HttpUtility.UrlEncode(EncryptToUrl(plainText)): plainText;
		}

		public string Unhash(string hashedText)
		{
			//LogManager.Log(hashedText+ "unhash");
			if (hashedText == "0") return "0";
			if (string.IsNullOrEmpty(hashedText.Trim())) return "";
			return hashEnabled ? DecryptFromUrl(HttpUtility.UrlDecode(hashedText).Replace(" ", "+")) : hashedText;
		}

		public string EncryptToUrl(string unencrypted)
		{
			//return HttpUtility.UrlEncode(Encrypt(unencrypted));

			//Getting the bytes of Input String.
			byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(unencrypted);

			MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

			//Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
			byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(encryptionKey));

			//De-allocatinng the memory after doing the Job.
			objMD5CryptoService.Clear();

			var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

			//Assigning the Security key to the TripleDES Service Provider.
			objTripleDESCryptoService.Key = securityKeyArray;

			//Mode of the Crypto service is Electronic Code Book.
			objTripleDESCryptoService.Mode = CipherMode.ECB;

			//Padding Mode is PKCS7 if there is any extra byte is added.
			objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

			var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();

			//Transform the bytes array to resultArray
			byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);

			//Releasing the Memory Occupied by TripleDES Service Provider for Encryption.
			objTripleDESCryptoService.Clear();

			//Convert and return the encrypted data/byte into string format.
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}

		public string DecryptFromUrl(string encrypted)
		{
			//return Decrypt(HttpUtility.UrlDecode(encrypted.Replace(" ", "+")));
			byte[] toEncryptArray = Convert.FromBase64String(encrypted);

			MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

			//Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
			byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(encryptionKey));

			//De-allocatinng the memory after doing the Job.
			objMD5CryptoService.Clear();

			var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

			//Assigning the Security key to the TripleDES Service Provider.
			objTripleDESCryptoService.Key = securityKeyArray;

			//Mode of the Crypto service is Electronic Code Book.
			objTripleDESCryptoService.Mode = CipherMode.ECB;

			//Padding Mode is PKCS7 if there is any extra byte is added.
			objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

			var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();

			//Transform the bytes array to resultArray
			byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

			//Releasing the Memory Occupied by TripleDES Service Provider for Decryption.          
			objTripleDESCryptoService.Clear();


			//Convert and return the decrypted data/byte into string format.
			return UTF8Encoding.UTF8.GetString(resultArray);
		}

		public byte[] Encrypt(byte[] buffer)
		{
			using (MemoryStream encryptStream = new MemoryStream())
			{
				using (CryptoStream cs = new CryptoStream(encryptStream, _encryptor, CryptoStreamMode.Write))
				{
					cs.Write(buffer, 0, buffer.Length);
					cs.Clear();
				}
				return encryptStream.ToArray();
			}
		}

		public byte[] Decrypt(byte[] buffer)
		{
			using (MemoryStream decryptStream = new MemoryStream())
			{
				using (CryptoStream cs = new CryptoStream(decryptStream, _decryptor, CryptoStreamMode.Write))
				{
					cs.Write(buffer, 0, buffer.Length);
					cs.Clear();
				}
				return decryptStream.ToArray();
			}
		}

	}
}
