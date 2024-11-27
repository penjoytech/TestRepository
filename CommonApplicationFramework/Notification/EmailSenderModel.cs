using System.Collections.Generic;

namespace CommonApplicationFramework.Notification
{
    public class EmailSenderModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string Status { get; set; }

        public string EmailId { get; set; }

        public string Salt { get; set; }

        public bool IsFirstLogin { get; set; }

        public string CompanyCode { get; set; }

        public string CompanyName { get; set; }

        public string CompanyLogo { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

		public string From { get; set; }

		public List<string> To { get; set; }

        public List<string> Cc { get; set; }

        public List<string> Bcc { get; set; }

        public List<string> Attachments { get; set; }

        public string FilePath { get; set; }
        public string UserId { get; set; }
    }
}
