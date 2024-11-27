//using Portable.Licensing;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
//using Portable.Licensing.Validation;
 using System.Text;
using CommonApplicationFramework.Common;

namespace CommonApplicationFramework.Licensing
{
    public enum LicenseTypes
    {
        Trial = 0,
        Floating = 1,
        Custom = 2
    }

    public enum LicenseStatus
    {
        UNDEFINED = 0,
        VALID = 1,
        INVALID = 2,
        MODIFIED = 3

    }

    public  class LicenseModel
    {
        public string LicenseKey { get; set; }
        public LicenseTypes Type { get; set; }
        public LicenseStatus Status { get; set; }
        public string LicenseNote { get; set; }

        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string LicensedTo { get; set; }
        public string Email { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductVersion { get; set; }

        public string SerialNumber { get; set; }

        public string OrderNumber { get; set; }
       
        public string Createdby { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset ActivatedOn { get; set; }
        public DateTimeOffset ExpiredOn { get; set; }

        public string Signature { get; set; }
    }

    public class TrialLicenseModel : LicenseModel
    {


    }

    public class CustomLicenseModel : LicenseModel
    {
        public int NoOfUsers { get; set; }
        public string MacAddress { get; set; }

        public List<string> ProductFeatures { get; set; }




    }

    public class LicenseManager
    {


        //private string passPhrase = "lease";
        //private string privateKey;
        //private string publicKey;

        #region Portable license
        //public bool GenerateLicense(LicenseModel licenseModel)
        //{
        //    var keyGenerator = Portable.Licensing.Security.Cryptography.KeyGenerator.Create();
        //    var keyPair = keyGenerator.GenerateKeyPair();
        //    privateKey = keyPair.ToEncryptedPrivateKeyString(passPhrase);
        //    publicKey = keyPair.ToPublicKeyString();

        //    var license = License.New()
        //    .WithUniqueIdentifier(Guid.NewGuid())
        //    .As(LicenseType.Trial)
        //    .ExpiresAt(licenseModel.ExpiredOn.DateTime)
        //    .WithMaximumUtilization(5)
        //    .WithProductFeatures(new Dictionary<string, string>  
        //                                  {  
        //                                      {licenseModel.ProductName, "yes"}


        //                          })
        //    .WithAdditionalAttributes(licenseModel.LicenseAttributes)
        //    //.WithAdditionalAttributes(new Dictionary<String, String>{
        //    //                                   {"MaxUsers","10"},
        //    //                                   {"MaxDocs", "100"},
        //    //                                   {"MacAddress", "48-2C-6A-1E-59-3D"}
        //    //                                    })
        //    .LicensedTo(licenseModel.LicensedTo, licenseModel.Email)
        //    .CreateAndSignWithPrivateKey(privateKey, passPhrase);

        //    File.WriteAllText(@"D:\License\License.lic", license.ToString(), Encoding.UTF8);


        //    licenseModel.PublicKey = publicKey;
        //    licenseModel.PassPhrase = passPhrase;
        //    licenseModel.LicenseKey = license.Id.ToString();


        //    return true;
        //}

        //public bool ActivateLicense()
        //{
        //    return true;
        //}

        //public bool ValidateLicense(LicenseModel licenseModel)
        //{
        //    StreamReader reader = new StreamReader(@"E:\License\License2.lic");
        //    License license = License.Load(reader);
        //    var validationFailures = license.Validate()
        //                            .ExpirationDate()
        //                                .When(lic => lic.Type == (LicenseType.Trial.ToString())license.AdditionalAttributes.Get("").ToString())
        //                            .And()
        //                            .Signature(publicKey)
        //                            .AssertValidLicense();
        //    if (validationFailures == null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        string validationMessage = string.Empty;
        //        foreach (var failure in validationFailures)
        //            validationMessage += (failure.GetType().Name + ": " + failure.Message + " - " + failure.HowToResolve);
        //        throw new Exception(validationMessage);
        //    }
        //}
        #endregion
                   

        public static string GenerateLicense(LicenseModel lic)
        {
            //Serialize license object into XML                    
            XmlDocument _licenseObject = new XmlDocument();
            using (StringWriter _writer = new StringWriter())
            {
                XmlSerializer _serializer = new XmlSerializer(typeof(LicenseModel), new Type[] { lic.GetType() });

                _serializer.Serialize(_writer, lic);

                _licenseObject.LoadXml(_writer.ToString());
            }

            //Get RSA key from certificate
            //X509Certificate2 cert = new X509Certificate2(certPrivateKeyData, certFilePwd);

            //RSACryptoServiceProvider rsaKey = (RSACryptoServiceProvider)cert.PrivateKey;
            X509Certificate2 certificate = new X509Certificate2("C:\\Users\\shashikanth\\source\\repos\\ConsoleApp6\\ConsoleApp6\\briconomics.pfx", "123456");


            if (certificate.HasPrivateKey)
            {
                using (var rsaKey = (RSACryptoServiceProvider)certificate.PrivateKey)
                {
                    SignXML(_licenseObject, rsaKey);
                }
            }

            //Convert the signed XML into BASE64 string       
            Console.WriteLine(_licenseObject.OuterXml);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(_licenseObject.OuterXml));

        }

        public static LicenseModel ParseLicenseFromBASE64String(string licenseString, out LicenseStatus licStatus, out string validationMsg)
        {
            validationMsg = string.Empty;
            licStatus = LicenseStatus.UNDEFINED;

            if (string.IsNullOrWhiteSpace(licenseString))
            {
                licStatus = LicenseStatus.MODIFIED;
                return null;
            }

            string _licXML = string.Empty;
            LicenseModel _lic = null;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                // Load an XML file into the XmlDocument object.
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.LoadXml(Encoding.UTF8.GetString(Convert.FromBase64String(licenseString)));

                //Get RSA key from certificate
                //X509Certificate2 cert = new X509Certificate2(certPubKeyData);
                //RSACryptoServiceProvider rsaKey = (RSACryptoServiceProvider)cert.PublicKey.Key;

                X509Certificate2 certificate = new X509Certificate2("C:\\Users\\shashikanth\\source\\repos\\ConsoleApp6\\ConsoleApp6\\briconomics.pfx", "123456");

                if (certificate.HasPrivateKey)
                {
                    using (var rsaKey = (RSACryptoServiceProvider)certificate.PublicKey.Key)
                    {
                        // Verify the signature of the signed XML.            
                        if (VerifyXml(xmlDoc, rsaKey))
                        {
                            licStatus = LicenseStatus.VALID;                             
                        }
                        else
                        {
                            licStatus = LicenseStatus.INVALID;
                        }
                    }
                }

            }
            catch
            {
                licStatus = LicenseStatus.MODIFIED;
            }

            return _lic;
        }

        // Sign an XML file. 
        // This document cannot be verified unless the verifying 
        // code has the key with which it was signed.
        private static void SignXML(XmlDocument xmlDoc, RSA Key)
        {
            // Check arguments.
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");
            if (Key == null)
                throw new ArgumentException("Key");

            // Create a SignedXml object.
            //////SignedXml signedXml = new SignedXml(xmlDoc);

            // Add the key to the SignedXml document.
            //////signedXml.SigningKey = Key;

            // Create a reference to be signed.
            //////Reference reference = new Reference();
            //////reference.Uri = "";

            //////// Add an enveloped transformation to the reference.
            //////XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            //////reference.AddTransform(env);

            //////// Add the reference to the SignedXml object.
            //////signedXml.AddReference(reference);

            //////// Compute the signature.
            //////signedXml.ComputeSignature();

            //////// Get the XML representation of the signature and save
            //////// it to an XmlElement object.
            //////XmlElement xmlDigitalSignature = signedXml.GetXml();

            //////// Append the element to the XML document.
            //////xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));

        }

        // Verify the signature of an XML file against an asymmetric 
        // algorithm and return the result.
        private static Boolean VerifyXml(XmlDocument Doc, RSA Key)
        {
            // Check arguments.
            if (Doc == null)
                throw new ArgumentException("Doc");
            if (Key == null)
                throw new ArgumentException("Key");

            // Create a new SignedXml object and pass it
            // the XML document class.
            //////SignedXml signedXml = new SignedXml(Doc);

            // Find the "Signature" node and create a new
            // XmlNodeList object.
            XmlNodeList nodeList = Doc.GetElementsByTagName("Signature");

            // Throw an exception if no signature was found.
            if (nodeList.Count <= 0)
            {
                throw new CryptographicException("Verification failed: No Signature was found in the document.");
            }

            // Load the first <signature> node.  
            //////signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result.
            return false; // signedXml.CheckSignature(Key);
        }

    }



     
    
}