//-----------------------------------------------------------------------
// <copyright file="EmailSender.cs" company="ASI">
//     Copyright (c) ASI . All rights reserved.
// </copyright>
// <author>Nirav</author>
// <createdon>07-01-2015</createdon>
// <comment></comment>
//-----------------------------------------------------------------------
namespace CommonApplicationFramework.Notification
{
    #region Namespaces
    using CommonApplicationFramework.ConfigurationHandling;
    using CommonApplicationFramework.ExceptionHandling;
    using CommonApplicationFramework.Logging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Xml;
    #endregion

    /// -----------------------------------------------------------------
    /// Namespace:      <ServtrackerModels>
    /// Class:          <EmailSender>
    /// Description:    <Description>
    /// Author:         <Nirav>                    
    /// -----------------------------------------------------------------
    public class EmailSender
    {
        //public static void SendInitialEmailVerificationEmail(ClientModel accountLoginEmail, string emailtemplate, string pwd)
        //{
        //    var templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("InitialEmailVerificationEmail")).Value.ToString();
        //    var txtMsg = ReadTXTFile(HttpContext.Current.Server.MapPath(templateFile));
        //    var mail = new MailMessage();
        //    mail.To.Add(new MailAddress(accountLoginEmail.EmailId));
        //    txtMsg = FormatMail(mail, txtMsg, emailtemplate);
        //    txtMsg = txtMsg.Replace("[name]", accountLoginEmail.FirstName);
        //    txtMsg = txtMsg.Replace("[email]", accountLoginEmail.EmailId);
        //    txtMsg = txtMsg.Replace("[UserName]", accountLoginEmail.EmailId);
        //    if (pwd == string.Empty)
        //    {
        //        txtMsg = txtMsg.Replace("[Password]", accountLoginEmail.UnhashedPassword);
        //    }
        //    else
        //    {
        //        txtMsg = txtMsg.Replace("[Password]", pwd);
        //    }
        //    txtMsg = txtMsg.Replace("[websiteurl]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("PrimaryURL")).Value.ToString());
        //    txtMsg = txtMsg.Replace("[emailverificationcode]", accountLoginEmail.Salt);
        //    txtMsg = txtMsg.Replace("[newline]", "<br />");
        //    mail.Body = txtMsg;
        //    if (templateFile.ToLower().EndsWith(".html") || templateFile.ToLower().EndsWith(".htm"))
        //    {
        //        mail.IsBodyHtml = true;
        //    }
        //    mail.From = new MailAddress("admin@pentechs.com", "SERVtracker");
        //    var smtp = new SmtpClient
        //    {
        //        Host = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPAddress")).Value.ToString()
        //    };
        //    try
        //    {
        //        string userCredential = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderEmail")).Value.ToString();
        //        string userCredentialPass = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderPassword")).Value.ToString();
        //        smtp.Credentials = new System.Net.NetworkCredential(userCredential, userCredentialPass);
        //        smtp.Send(mail);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        public static string FormatMail(MailMessage mail, string txtMsg, string templatetype)
        {
            //XmlTextReader reader = new XmlTextReader(ConfigurationManager.AppSettings["EmailContent"]);
            XmlReader reader = null; // XmlReader.Create(HttpContext.Current.Server.MapPath(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailContent")).Value.ToString()));
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.EndElement && reader.Name == templatetype)
                {
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        reader.Read();
                        if (reader.Name == "subject")
                        {
                            while (reader.NodeType != XmlNodeType.EndElement)
                            {
                                reader.Read();
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    mail.Subject = reader.Value;
                                }
                            }
                            reader.Read();
                        }
                        if (reader.Name == "emailcontent")
                        {
                            while (reader.NodeType != XmlNodeType.EndElement)
                            {
                                reader.Read();
                                if (reader.Name == "header")
                                {
                                    while (reader.NodeType != XmlNodeType.EndElement)
                                    {
                                        reader.Read();
                                        if (reader.NodeType == XmlNodeType.Text)
                                        {
                                            txtMsg = txtMsg.Replace("[header]", reader.Value);
                                        }
                                    }
                                    reader.Read();
                                }
                                if (reader.Name == "emailbody")
                                {
                                    while (reader.NodeType != XmlNodeType.EndElement)
                                    {
                                        reader.Read();
                                        if (reader.NodeType == XmlNodeType.CDATA)
                                        {
                                            txtMsg = txtMsg.Replace("[emailbody]", reader.Value);
                                        }
                                    }
                                    reader.Read();
                                }
                            }
                        }
                    }
                }
            }
            return txtMsg;
        }

        public static void SendPasswordReminderEmail(EmailSenderModel user, string emailTemplate, string CompanyName,string CompanyType=null)
        {
            System.Net.Mail.SmtpClient client;
            System.Net.Mail.MailMessage mail;
            string userCredential = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderEmail")).Value.ToString();
            string userCredentialPass = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderPassword")).Value.ToString();
            client = new System.Net.Mail.SmtpClient(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPAddress")).Value.ToString());
            client.Port = Convert.ToInt32(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPPort")).Value.ToString());
            client.Credentials = new System.Net.NetworkCredential(userCredential, userCredentialPass);
            client.EnableSsl = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableSSL")).Value.ToString());
            string userDisplayName = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderDisplayName")).Value.ToString();
            mail = new System.Net.Mail.MailMessage();
            switch (emailTemplate)
            {
                case "NewRegistration": mail.Subject = "Registration"; break;
                case "CustomerNewRegistration": mail.Subject = "Complete your Registration"; break;
                case "PasswordRemainder": mail.Subject = "Forgot Password "; break;
                case "ChangePasswrod": mail.Subject = " Successfully changed Password"; break;
                case "CustomerPasswordRemainder": mail.Subject = "Forgot Password"; break;
                case "CustomerLoginRegistration": mail.Subject = "Registration"; break;
                default: mail.Subject = "Please verify your email address"; break;
            }
            var txtMsg = string.Empty; var templateFile = string.Empty;
            if (emailTemplate == "PasswordRemainder")
            {
                templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ResetPasswordEmail")).Value.ToString();
                txtMsg = ""; // ReadTXTFile(HttpContext.Current.Server.MapPath(templateFile));
                txtMsg = txtMsg.Replace("[websiteurl]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("PasswordReset")).Value.ToString());
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(user.Salt);
                string Encoded = System.Convert.ToBase64String(plainTextBytes);
                Encoded = Encoded.Replace("=", "*");
                txtMsg = txtMsg.Replace("[passwordresetcode]", Encoded);
                txtMsg = txtMsg.Replace("[companycode]", user.CompanyCode);
                txtMsg = txtMsg.Replace("[CompanyName]", user.CompanyName);
                txtMsg = txtMsg.Replace("[Name]", user.FirstName);
            }
            else if (emailTemplate == "CustomerPasswordRemainder")
            {
                templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ResetPasswordEmail")).Value.ToString();
                txtMsg = ""; // ReadTXTFile(HttpContext.Current.Server.MapPath(templateFile));
                txtMsg = txtMsg.Replace("[websiteurl]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("CustomerPasswordReset")).Value.ToString());
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(user.Salt);
                string Encoded = System.Convert.ToBase64String(plainTextBytes);
                Encoded = Encoded.Replace("=", "*");
                txtMsg = txtMsg.Replace("[passwordresetcode]", Encoded);
                txtMsg = txtMsg.Replace("[companycode]", user.CompanyCode);
                txtMsg = txtMsg.Replace("[[CompanyLogo]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + "/Resources/" + user.CompanyCode + ".png");
                txtMsg = txtMsg.Replace("[CompanyName]", user.CompanyName);
                txtMsg = txtMsg.Replace("[Name]", user.FirstName);

            }
            else if (emailTemplate == "ChangePasswrod")
            {
                templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ChangedPasswordEmail")).Value.ToString();
                txtMsg = ""; // ReadTXTFile(HttpContext.Current.Server.MapPath(templateFile));
                txtMsg = txtMsg.Replace("[websiteurl]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("LoginURL")).Value.ToString());
                if (!string.IsNullOrEmpty(user.CompanyLogo))
                    txtMsg = txtMsg.Replace("[[CompanyLogo]]", user.CompanyLogo);
            }
            else if (emailTemplate == "CustomerNewRegistration")
            {
                if (user.IsFirstLogin)
                    templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("CustomerNewRegistrationEmail")).Value.ToString();
                else
                    templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("LinkCompanyEmail")).Value.ToString();
                txtMsg = ""; // ReadTXTFile(HttpContext.Current.Server.MapPath(templateFile));
				txtMsg = txtMsg.Replace("[name]", user.Name);
				txtMsg = txtMsg.Replace("[websiteurl]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("LoginURL")).Value.ToString());
				txtMsg = txtMsg.Replace("[verifylink]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("VerifyOTP")).Value.ToString());
				txtMsg = txtMsg.Replace("[EMAIL]", user.EmailId).Replace("[OTP]", user.Password);
				txtMsg = txtMsg.Replace("[[CompanyLogo]]", user.FilePath);
                txtMsg = txtMsg.Replace("[OrgnizationName]", user.CompanyName);
				 
            }
            else if (emailTemplate == "CustomerLoginRegistration")
            {
                templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("CustomerLoginRegistration")).Value.ToString();
                txtMsg = ""; // ReadTXTFile(HttpContext.Current.Server.MapPath(templateFile));
                txtMsg = txtMsg.Replace("[[CompanyLogo]]", user.FilePath);
                txtMsg = txtMsg.Replace("[OrgnizationName]", user.CompanyName);
            }
            else
            {
                if (user.IsFirstLogin)
                {
                    if (CompanyType == "AD")
                    {
                        templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ADUserNewRegistrationEmail")).Value.ToString();
                    }
                    else
                    {
                        templateFile = Convert.ToString(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("StandardUserNewRegistrationEmail")).Value);
                    }
                }
                else
                    templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("LinkCompanyEmail")).Value.ToString();
                txtMsg = ""; // ReadTXTFile(HttpContext.Current.Server.MapPath(templateFile));
				    txtMsg = txtMsg.Replace("[websiteurl]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("LoginURL")).Value.ToString());
            }
            mail.To.Add(new MailAddress(user.EmailId));
            txtMsg = FormatMail(mail, txtMsg, emailTemplate);
			 
			if (user.CompanyCode != null)
                txtMsg = txtMsg.Replace("[[CompanyLogo]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + "/Resources/" + user.CompanyCode + ".png");
            txtMsg = txtMsg.Replace("[[UserIcon]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + "/Resources/UserIcon.png");
            txtMsg = txtMsg.Replace("[[LockIcon]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + "/Resources/Lock.png");
            txtMsg = txtMsg.Replace("[[TickIcon]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + "/Resources/Tick.png");
            txtMsg = txtMsg.Replace("[[ExclamationIcon]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + "/Resources/ExclamationIcon.png");
            txtMsg = txtMsg.Replace("[[VerifyButton]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + "/Resources/VerifyButton.png");
            txtMsg = txtMsg.Replace("[[ResetPasswordButton]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + "/Resources/ResetPasswordButton.png");
            txtMsg = txtMsg.Replace("[[LoginButton]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + "/Resources/LoginButton.png");
            txtMsg = txtMsg.Replace("[name]", user.FirstName);
            txtMsg = txtMsg.Replace("[email]", user.EmailId);
            txtMsg = txtMsg.Replace("[CompanyName]", user.CompanyName);
            txtMsg = txtMsg.Replace("[EmailAddress]", user.EmailId);
            if (CompanyType == "AD")
            {
                txtMsg = txtMsg.Replace("[UserId]", null);
            }
            else
            {
                txtMsg = txtMsg.Replace("[Password]", user.Password);
            }
            txtMsg = txtMsg.Replace("[newline]", "<br />");
            txtMsg = txtMsg.Replace("[AgencyName]", user.CompanyName);
            mail.Body = txtMsg;
            if (templateFile.ToLower().EndsWith(".html") || templateFile.ToLower().EndsWith(".htm"))
                mail.IsBodyHtml = true;
            mail.From = new System.Net.Mail.MailAddress(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSender")).Value.ToString(), userDisplayName);
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                client.Send(mail);
            }
            catch (Exception ex)
            {
                LogManager.Log(ex.Message, ex.StackTrace);
                LogManager.Log(ex);
                throw new BusinessException("ERROROCCURE", MessageConfig.MessageSettings["EMAILFAILED"], "CREATED");
            }
        }

        private static string ReadTXTFile(string Path)
        {
            string line;
            var sb = new StringBuilder();
            var file = new System.IO.StreamReader(Path);
            while ((line = file.ReadLine()) != null)
            {
                sb.AppendLine(line);
            }
            file.Close();
            return sb.ToString();
        }

        public static void SendNotificationUser(string NotificationUser, string Message, string Code)
        {
            SmtpClient client;
            MailMessage mail;
            string userCredential = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderEmail")).Value.ToString();
            string userCredentialPass = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderPassword")).Value.ToString();
            client = new SmtpClient(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPAddress")).Value.ToString());
            client.Port = Convert.ToInt32(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPPort")).Value.ToString());
            client.Credentials = new NetworkCredential(userCredential, userCredentialPass);
            client.EnableSsl = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableSSL")).Value.ToString());
            mail = null;
            mail = new MailMessage();
            mail.Subject = "Error Notification From "+Code;
            mail.Body = Message;
            mail.From = new MailAddress(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSender")).Value.ToString(), "MFL");
            if (!string.IsNullOrEmpty(NotificationUser))
            {
                string[] Notify = NotificationUser.Split(';');

                for (int i = 0; i < Notify.Count(); i++)
                {
                    mail.To.Add(new MailAddress(Notify[i]));
                    try
                    {
                        ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        { return true; };

                        client.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        LogManager.Log(ex);

                    }
                }

            }


        }

        //public static void SendProcessMail(HashSet<string> email, string code, string requestTitle, string requesterName, string requesterDate, FormModel form)
        //{
        //    if (email.Count > 0)
        //    {
        //        System.Net.Mail.SmtpClient client; System.Net.Mail.MailMessage mail;
        //        string userCredential = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderEmail")).Value.ToString();
        //        string userCredentialPass = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderPassword")).Value.ToString();
        //        client = new System.Net.Mail.SmtpClient(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPAddress")).Value.ToString());
        //        client.Port = Convert.ToInt32(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPPort")).Value.ToString());
        //        client.Credentials = new System.Net.NetworkCredential(userCredential, userCredentialPass);
        //        client.EnableSsl = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableSSL")).Value.ToString());
        //        mail = new System.Net.Mail.MailMessage();
        //        mail.Subject = requestTitle;
        //        var txtMsg = string.Empty; var templateFile = string.Empty;
        //        templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ProcessActivityEmail")).Value.ToString();
        //        txtMsg = ReadTXTFile(HttpContext.Current.Server.MapPath(templateFile));
        //        if (code != null)
        //            txtMsg = txtMsg.Replace("[[CompanyLogo]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + "/Resources/" + code + ".png");
        //        txtMsg = txtMsg.Replace("[[Subject]]", requestTitle);
        //        txtMsg = txtMsg.Replace("[[RequesterName]]", requesterName);
        //        txtMsg = txtMsg.Replace("[[RequesterDate]]", requesterDate);
        //        txtMsg = txtMsg.Replace("[[CompanyName]]", "Quasar Matrix");
        //        string additionalInfo = string.Empty;
        //        if (form != null)
        //        {
        //            if (form.Tab != null && form.Tab.Count > 0)
        //            {
        //                foreach (var tab in form.Tab)
        //                {
        //                    if (tab.TabMetadata != null && tab.TabMetadata.Count > 0)
        //                    {
        //                        foreach (var tabMetadata in tab.TabMetadata)
        //                        {
        //                            if (tabMetadata.Metadata != null && tabMetadata.Metadata.Id > 0)
        //                            {
        //                                additionalInfo += "<td style='text-align:left; padding-left:10px; font-size:14px; width:20%;' align='left'>" + tabMetadata.Metadata.Code + ": </td><td style='text-align:left; font-size:14px; width:80%;' align='left'>" + tabMetadata.Metadata.Value + "</td>";
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        txtMsg = txtMsg.Replace("[[AdditionalInformation]]", additionalInfo);
        //        mail.Body = txtMsg;
        //        mail.From = new System.Net.Mail.MailAddress(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSender")).Value.ToString(), "MFL Admin");
        //        mail.IsBodyHtml = true;
        //        try
        //        {
        //            foreach (var item in email)
        //                mail.To.Add(new MailAddress(item));
        //            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        //            client.Send(mail);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogManager.Log(ex);
        //        }
        //    }
        //}

        public static void SendGenericEmail(string code, IDictionary<string, object> mailBody, HashSet<string> emails, IDictionary<string, object> requestInfo, string mailFrom, string mailSubject, string template)
        {
            if (emails.Count > 0)
            {
                SmtpClient client; MailMessage mail;
                string userCredential = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderEmail")).Value.ToString();
                string userCredentialPass = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderPassword")).Value.ToString();
                client = new SmtpClient(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPAddress")).Value.ToString());
                client.Port = Convert.ToInt32(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPPort")).Value.ToString());
                client.Credentials = new NetworkCredential(userCredential, userCredentialPass);
                client.EnableSsl = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableSSL")).Value.ToString());
                mail = new MailMessage();
                mail.Subject = mailSubject;                
                var txtMsg = string.Empty; var templateFile = string.Empty;
                templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals(template)).Value.ToString();
                txtMsg = ""; // ReadTXTFile(HttpContext.Current.Server.MapPath(templateFile));
                if (!string.IsNullOrEmpty(code))
                    txtMsg = txtMsg.Replace("[[CompanyLogo]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + "/Resources/" + code + ".png");
                string additionalInfo = string.Empty;
                foreach (var item in mailBody)
                {
                    additionalInfo += "<tr><td style='text-align:left; padding-left:10px; font-size:14px; width:20%;' align='left'>" + item.Key + ": </td><td style='text-align:left; font-size:14px; width:80%;' align='left'>" + item.Value.ToString() + "</td></tr>";
                }
                txtMsg = txtMsg.Replace("[[AdditionalInformation]]", additionalInfo);
                foreach (var item in requestInfo)
                {
                    txtMsg = txtMsg.Replace("[[" + item.Key + "]]", item.Value.ToString());
                }
                mail.Body = txtMsg;
                mail.From = new MailAddress(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSender")).Value.ToString(), mailFrom);
                mail.IsBodyHtml = true;
                try
                {
                    foreach (var item in emails)
                        mail.To.Add(new MailAddress(item));
                    ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    LogManager.Log(ex);
                }
            }
        }

        public static void SendGenericActivityEmail(string code, string mailBody, HashSet<string> emails, IDictionary<string, object> requestInfo, string mailFrom, string mailSubject,string mailHeader, string template)
        {
            if (emails.Count > 0)
            {
                SmtpClient client; MailMessage mail;
                string userCredential = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderEmail")).Value.ToString();
                string userCredentialPass = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderPassword")).Value.ToString();
                client = new SmtpClient(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPAddress")).Value.ToString());
                client.Port = Convert.ToInt32(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPPort")).Value.ToString());
                client.Credentials = new NetworkCredential(userCredential, userCredentialPass);
                client.EnableSsl = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableSSL")).Value.ToString());
                mail = new MailMessage();
                mail.Subject = mailSubject;
                var txtMsg = string.Empty; var templateFile = string.Empty;
                templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals(template)).Value.ToString();
                txtMsg = ""; // ReadTXTFile(HttpContext.Current.Server.MapPath(templateFile));
                //if (!string.IsNullOrEmpty(code))
                    txtMsg = txtMsg.Replace("[[CompanyLogo]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("CompanyLogo")).Value.ToString());
                    txtMsg = txtMsg.Replace("[[FacebookIcon]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("FacebookIcon")).Value.ToString());
                    txtMsg = txtMsg.Replace("[[LinkedInIcon]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("LinkedInIcon")).Value.ToString());
                    txtMsg = txtMsg.Replace("[[TwitterIcon]]", Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ImageURL")).Value.ToString() + Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("TwitterIcon")).Value.ToString());
                string additionalInfo = string.Empty;
                //foreach (var item in mailBody)
                //{
                   // additionalInfo += "<tr><td style='text-align:left; padding-left:10px; font-size:14px; width:20%;' align='left'>" + item.Key + ": </td><td style='text-align:left; font-size:14px; width:80%;' align='left'>" + item.Value.ToString() + "</td></tr>";
                    txtMsg = txtMsg.Replace("[[Header]]", mailHeader);
                    txtMsg = txtMsg.Replace("[[Body]]", mailBody);
                //}
                txtMsg = txtMsg.Replace("[[AdditionalInformation]]", additionalInfo);
                foreach (var item in requestInfo)
                {
                    txtMsg = txtMsg.Replace("[[" + item.Key + "]]", item.Value.ToString());
                }
                mail.Body = txtMsg;
                mail.From = new MailAddress(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSender")).Value.ToString(), mailFrom);
                mail.IsBodyHtml = true;
                try
                {
                    foreach (var item in emails)
                        mail.To.Add(new MailAddress(item));
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    LogManager.Log(ex);
                }
            }
        }

        public static bool EmailNotofication(EmailSenderModel emailData)
        {
			try
			{
				string userCredential = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderEmail")).Value.ToString();
				string userCredentialPass = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderPassword")).Value.ToString();
				string userDisplayName = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderDisplayName")).Value.ToString();
				var templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ResourceFileServer")).Value.ToString() + emailData.FilePath;
				LogManager.Log(templateFile);
				if (!File.Exists(templateFile))
					return false;
				var txtMsg = ReadTXTFile(templateFile);
				txtMsg = txtMsg.Replace("[EmailBodyContent]", emailData.Body).Replace("[[CompanyLogo]]", emailData.CompanyLogo);
				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPAddress")).Value.ToString());
				mail.From = new MailAddress(userCredential, userDisplayName);
				if (templateFile.ToLower().EndsWith(".html") || templateFile.ToLower().EndsWith(".htm"))
					mail.IsBodyHtml = true;
				SmtpServer.Port = Convert.ToInt32(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPPort")).Value.ToString()); ;
				SmtpServer.UseDefaultCredentials = false;
				SmtpServer.Credentials = new NetworkCredential(userCredential, userCredentialPass);
				SmtpServer.EnableSsl = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableSSL")).Value.ToString());
				mail.Subject = emailData.Subject;
				mail.Body = txtMsg;
				if (emailData.To != null)
				{
					foreach (string toMail in emailData.To)
					{
						mail.To.Add(toMail);
					}
				}
				if (emailData.Cc != null)
				{
					foreach (string ccMail in emailData.Cc)
					{
						mail.CC.Add(ccMail);
					}
				}
				if (emailData.Bcc != null)
				{
					foreach (string bccMail in emailData.Bcc)
					{
						mail.Bcc.Add(bccMail);
					}
				}
				if (emailData.Attachments != null)
				{
					foreach (string attachedFile in emailData.Attachments)
					{
						mail.Attachments.Add(new Attachment(GetStreamFromUrl(attachedFile), Path.GetFileName(attachedFile)));
					}
				}

				SmtpServer.Send(mail);
				
			}
			catch (SmtpFailedRecipientsException ex)
			{
				for (int i = 0; i < ex.InnerExceptions.Length; i++)
				{
					SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
					if (status == SmtpStatusCode.MailboxBusy ||
						status == SmtpStatusCode.MailboxUnavailable)
					{
						LogManager.Log(status.ToString());
					}
					else
					{
						LogManager.Log("Failed to deliver message to {0}",
							ex.InnerExceptions[i].FailedRecipient);
					}
				}
			}
			catch (Exception ex)
			{
				LogManager.Log("Exception caught in RetryIfBusy(): {0}",
						ex.ToString());
			}

			return true;
		}

		public static bool MailNotification(EmailSenderModel emailData)
		{
			try
			{
				string userCredential = emailData.From; //Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderEmail")).Value.ToString();
				string userCredentialPass = emailData.Password; //Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderPassword")).Value.ToString();
				string userDisplayName = emailData.From; // Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderDisplayName")).Value.ToString();
				var templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ResourceFileServer")).Value.ToString() + emailData.FilePath;
				 
				if (!File.Exists(templateFile))
					return false;
				var txtMsg = ReadTXTFile(templateFile);
				txtMsg = txtMsg.Replace("[EmailBodyContent]", emailData.Body).Replace("[[CompanyLogo]]", emailData.CompanyLogo);
				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPAddress")).Value.ToString());
				mail.From = new MailAddress(userCredential, userDisplayName);
				if (templateFile.ToLower().EndsWith(".html") || templateFile.ToLower().EndsWith(".htm"))
					mail.IsBodyHtml = true;
				SmtpServer.Port = Convert.ToInt32(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPPort")).Value.ToString()); ;
				SmtpServer.UseDefaultCredentials = false;
				SmtpServer.Credentials = new NetworkCredential(userCredential, userCredentialPass);
				SmtpServer.EnableSsl = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableSSL")).Value.ToString());
				mail.Subject = emailData.Subject;
				mail.Body = txtMsg; //.Replace("\n", "<br/>");
			 
				if (emailData.To != null)
				{
					foreach (string toMail in emailData.To)
					{
						mail.To.Add(toMail);
					}
				}
				if (emailData.Cc != null)
				{
					foreach (string ccMail in emailData.Cc)
					{
						mail.CC.Add(ccMail);
					}
				}
				if (emailData.Bcc != null)
				{
					foreach (string bccMail in emailData.Bcc)
					{
						mail.Bcc.Add(bccMail);
					}
				}
				if (emailData.Attachments != null)
				{
					foreach (string attachedFile in emailData.Attachments)
					{
						mail.Attachments.Add(new Attachment(GetStreamFromUrl(attachedFile), Path.GetFileName(attachedFile)));
					}
				}

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                                    | SecurityProtocolType.Tls11
                                                                    | SecurityProtocolType.Tls12
                                                                    | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

                SmtpServer.Send(mail);

			}
			catch (SmtpFailedRecipientsException ex)
			{
				for (int i = 0; i < ex.InnerExceptions.Length; i++)
				{
					SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
					if (status == SmtpStatusCode.MailboxBusy ||
						status == SmtpStatusCode.MailboxUnavailable)
					{
						LogManager.Log(status.ToString());
					}
					else
					{
						LogManager.Log("Failed to deliver message to {0}",
							ex.InnerExceptions[i].FailedRecipient);
					}
				}
			}
			catch (Exception ex)
			{
				LogManager.Log("Exception caught in RetryIfBusy(): {0}",
						ex.ToString());
			}

			return true;
		}

		public static bool NormalEmailNotofication(EmailSenderModel emailData)
        {
            string userCredential = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderEmail")).Value.ToString();
            string userCredentialPass = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderPassword")).Value.ToString();
            string userDisplayName = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderDisplayName")).Value.ToString();
            //var templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ResourceFileServer")).Value.ToString() + emailData.FilePath;
            //if (!File.Exists(templateFile))
            //    return false;
            //var txtMsg = ReadTXTFile(templateFile);
            //txtMsg = txtMsg.Replace("[EmailBodyContent]", emailData.Body).Replace("[[CompanyLogo]]", emailData.CompanyLogo);
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPAddress")).Value.ToString());
            mail.From = new MailAddress(userCredential, userDisplayName);
            //if (templateFile.ToLower().EndsWith(".html") || templateFile.ToLower().EndsWith(".htm"))
            //    mail.IsBodyHtml = true;
            SmtpServer.Port = Convert.ToInt32(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPPort")).Value.ToString()); ;
            SmtpServer.Credentials = new NetworkCredential(userCredential, userCredentialPass);
            SmtpServer.EnableSsl = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableSSL")).Value.ToString());
            mail.Subject = emailData.Subject;
            mail.Body = emailData.Body;
            if (emailData.To != null)
            {
                foreach (string toMail in emailData.To)
                {
                    mail.To.Add(toMail);
                }
            }
            if (emailData.Cc != null)
            {
                foreach (string ccMail in emailData.Cc)
                {
                    mail.CC.Add(ccMail);
                }
            }
            if (emailData.Bcc != null)
            {
                foreach (string bccMail in emailData.Bcc)
                {
                    mail.Bcc.Add(bccMail);
                }
            }
            if (emailData.Attachments != null)
            {
                foreach (string attachedFile in emailData.Attachments)
                {
                    mail.Attachments.Add(new Attachment(GetStreamFromUrl(attachedFile), Path.GetFileName(attachedFile)));
                }
            }
            SmtpServer.Send(mail);
            return true;
        }

        private static Stream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;

            using (var wc = new WebClient())
                imageData = wc.DownloadData(url);
			if (imageData != null)
				return new MemoryStream(imageData);
			else
				return null;
        }

        public static async Task SendNotificationEmail(string NotificationUser, string Message, string Code)
        {
            await Task.Run(() =>
            {
                SmtpClient client;
                MailMessage mail;
                string userCredential = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderEmail")).Value.ToString();
                string userCredentialPass = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderPassword")).Value.ToString();
                client = new SmtpClient(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPAddress")).Value.ToString());
                client.Port = Convert.ToInt32(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPPort")).Value.ToString());
                client.Credentials = new NetworkCredential(userCredential, userCredentialPass);
                client.EnableSsl = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableSSL")).Value.ToString());
                mail = null;
                mail = new MailMessage();
                mail.Subject = "Error Notification From " + Code;
                mail.Body = Message;
                mail.From = new MailAddress(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSender")).Value.ToString(), "MFL");
                if (!string.IsNullOrEmpty(NotificationUser))
                {
                    string[] Notify = NotificationUser.Split(';');

                    for (int i = 0; i < Notify.Count(); i++)
                    {
                        mail.To.Add(new MailAddress(Notify[i]));
                        try
                        {
                            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                            { return true; };

                            client.Send(mail);
                        }
                        catch (Exception ex)
                        {
                            LogManager.Log(ex);

                        }
                    }
                }
            });
        }

        public static async void InvokeActivityEmail(string code, string mailBody, HashSet<string> emails, IDictionary<string, object> requestInfo, string mailFrom, string mailSubject, string mailHeader, string template)
        {
            //await Task.Run(() =>
            //{
                if (emails.Count > 0)
                {
                    string emailEngine = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailEngine")).Value.ToString();
                    if(emailEngine.ToUpper().Equals(Common.EmailEngine.Google.ToString().ToUpper()))
                    {
                        GoogleEngineSendMail(code, mailBody,emails,requestInfo,mailFrom,mailSubject,mailHeader,template);
                    }
                    else if(emailEngine.ToUpper().Equals(Common.EmailEngine.AWS.ToString().ToUpper()))
                    {

                    }
                }
            //});
        }
        private static void GoogleEngineSendMail(string CompGuid, string mailBody, HashSet<string> emails, IDictionary<string, object> requestInfo, string mailFrom, string mailSubject, string mailHeader, string template)
        {
            SmtpClient client;
            MailMessage mail;
            string userCredential = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderEmail")).Value.ToString();
            string userCredentialPass = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSenderPassword")).Value.ToString();
            client = new SmtpClient(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPAddress")).Value.ToString());
            client.Port = Convert.ToInt32(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("SMTPPort")).Value.ToString());
            client.Credentials = new NetworkCredential(userCredential, userCredentialPass);
            client.EnableSsl = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableSSL")).Value.ToString());
            mail = new MailMessage();
            mail.Subject = mailSubject;
            var txtMsg = string.Empty; var templateFile = string.Empty;
            templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals(template)).Value.ToString();
            templateFile = templateFile.Replace("@CompCode", CompGuid);
            if (!File.Exists(templateFile))
            {
                templateFile = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ProcessActivityDefaultEmail")).Value.ToString();
            }
            txtMsg = ReadTXTFile(templateFile);
            //if (!string.IsNullOrEmpty(code))
            txtMsg = txtMsg.Replace("[[CompanyLogo]]",Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("CompanyLogo")).Value.ToString().Replace("@CompCode", CompGuid));
            txtMsg = txtMsg.Replace("[[FacebookIcon]]",Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("FacebookIcon")).Value.ToString().Replace("@CompCode", CompGuid));
            txtMsg = txtMsg.Replace("[[LinkedInIcon]]",Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("LinkedInIcon")).Value.ToString().Replace("@CompCode", CompGuid));
            txtMsg = txtMsg.Replace("[[TwitterIcon]]",Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("TwitterIcon")).Value.ToString().Replace("@CompCode", CompGuid));
            string additionalInfo = string.Empty;
            //foreach (var item in mailBody)
            //{
            // additionalInfo += "<tr><td style='text-align:left; padding-left:10px; font-size:14px; width:20%;' align='left'>" + item.Key + ": </td><td style='text-align:left; font-size:14px; width:80%;' align='left'>" + item.Value.ToString() + "</td></tr>";
            txtMsg = txtMsg.Replace("[[Header]]", mailHeader);
            txtMsg = txtMsg.Replace("[[Body]]", mailBody);
            //}
            txtMsg = txtMsg.Replace("[[AdditionalInformation]]", additionalInfo);
            foreach (var item in requestInfo)
            {
                txtMsg = txtMsg.Replace("[[" + item.Key + "]]", item.Value.ToString());
            }
            mail.Body = txtMsg;
            mail.From = new MailAddress(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EmailSender")).Value.ToString(), mailFrom);
            mail.IsBodyHtml = true;
            try
            {
                foreach (var item in emails)
                    mail.To.Add(new MailAddress(item));
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                client.Send(mail);

            }
            catch (Exception ex)
            {
                LogManager.Log(ex);
            }
        }
        private static void AmazonEngineSendMail()
        {

        }
        private static void SendGridEngineSendMail()
        {

        }
    }
}





