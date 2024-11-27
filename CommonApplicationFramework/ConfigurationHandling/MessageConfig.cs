using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Xml;

namespace CommonApplicationFramework.ConfigurationHandling
{
    public class MessageConfig : ConfigurationSection
    {
        private static NameValueCollection _messageSettings = new NameValueCollection();
        public static NameValueCollection MessageSettings { get { return _messageSettings; } }
        public static Configuration MessageCodeConfig;

        public MessageConfig()
        {
            NameValueSectionHandler handler = new NameValueSectionHandler();
            XmlDocument sectionXmlDoc = new XmlDocument();
            sectionXmlDoc.Load(new StringReader(MessageCodeConfig.GetSection("MessageCodeConfiguration").SectionInformation.GetRawXml()));
            _messageSettings = handler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;          
        }
    }
}
