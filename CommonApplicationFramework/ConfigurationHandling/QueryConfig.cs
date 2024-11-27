#region copyright
// <copyright file="QueryConfigurationManager.cs" company="Pentechs.com">
//     Pentechs.com
// </copyright>
// <author>Pentechs Architect</author>
#endregion

namespace CommonApplicationFramework.ConfigurationHandling
{
    #region namespaces
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.IO;
    using System.Xml;
    #endregion

    /// <summary>
    /// this class contain namevaluecollection and it will inherit ConfigurationSection
    /// </summary>
    public class QueryConfig : ConfigurationSection
    {
        public static Configuration QuerySectionConfig;

        private static NameValueCollection _learningPortal = new NameValueCollection();
        public static NameValueCollection LPQuerySettings { get { return _learningPortal; } }

        private static NameValueCollection _FlutterQueryConfigurations = new NameValueCollection();
        public static NameValueCollection FlutterQueryQuerySettings { get { return _FlutterQueryConfigurations; } }


        public QueryConfig()
        {
            NameValueSectionHandler handler = new NameValueSectionHandler();
            XmlDocument sectionXmlDoc = new XmlDocument();
            if (QuerySectionConfig.GetSection("LearningPortal") != null)
            {
                sectionXmlDoc.Load(new StringReader(QuerySectionConfig.GetSection("LearningPortal").SectionInformation.GetRawXml()));
                _learningPortal = handler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
            }
            if(QuerySectionConfig.GetSection("FlutterQueryConfigurations") != null)
            {
                sectionXmlDoc.Load(new StringReader(QuerySectionConfig.GetSection("FlutterQueryConfigurations").SectionInformation.GetRawXml()));
                _FlutterQueryConfigurations = handler.Create(null, null, sectionXmlDoc.DocumentElement) as NameValueCollection;
            }
            
        }
    }
}


