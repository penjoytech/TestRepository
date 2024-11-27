using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CommonApplicationFramework.Security
{
	public class AESEncryptionHelper
	{
	 
		private static readonly string sKey = "y-R?#5YjHF}nep->BAS;J{v/4YB}^c?E)PMp5M=;<P$j<C*9m";
		private static readonly string encryptionKey = sKey.Substring(0, 8) + sKey.Substring(sKey.Length - 8, 8);
		private static readonly byte[] Key = Encoding.UTF8.GetBytes(encryptionKey);
		private static readonly byte[] Vector = Encoding.UTF8.GetBytes(encryptionKey);
		private static AESEncryptionHelper instance = null;
		private static ICryptoTransform _encryptor;
		private static ICryptoTransform _decryptor;
		private static UTF8Encoding encoder;

		public static  AESEncryptionHelper Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new AESEncryptionHelper();
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
			if (unencrypted == "0") return "0";
			if (string.IsNullOrEmpty(unencrypted.Trim())) return "";
			return string.IsNullOrEmpty(unencrypted) ?"" : 
				Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
		}

		public string Decrypt(string encrypted)
		{
			if (encrypted == "0") return "0";
			if (string.IsNullOrEmpty(encrypted.Trim())) return "";
			return string.IsNullOrEmpty(encrypted) ? "" : 
				encoder.GetString(Decrypt(Convert.FromBase64String(encrypted.Replace(" ", "+"))));
			 
		}

		public string EncryptToUrl(string unencrypted)
		{
			if (unencrypted == "0") return "0";
			if (string.IsNullOrEmpty(unencrypted.Trim())) return "";
			return HttpUtility.UrlEncode(Encrypt(unencrypted));
		}

		public string DecryptFromUrl(string encrypted)
		{
			if (encrypted == "0") return "0";
			if (string.IsNullOrEmpty(encrypted.Trim())) return "";
			return Decrypt(HttpUtility.UrlDecode(encrypted.Replace(" ", "+")));
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
