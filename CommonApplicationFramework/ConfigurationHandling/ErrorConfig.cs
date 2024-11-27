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
    public class ErrorConfig : ConfigurationSection
    {
        private static NameValueCollection _errorCodeSettings = new NameValueCollection();
        public static NameValueCollection ErrorCodeSettings { get { return _errorCodeSettings; } }
        public static Configuration ErrorCodeConfig;

        public ErrorConfig()
        {
            NameValueSectionHandler handler = new NameValueSectionHandler();
            XmlDocument sectionXmlDoc = new XmlDocument();
            sectionXmlDoc.Load(new StringReader(ErrorCodeConfig.GetSection("ErrorCodeConfiguration").SectionInformation.GetRawXml()));
            _errorCodeSettings = handler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;          
        }
    }
}
