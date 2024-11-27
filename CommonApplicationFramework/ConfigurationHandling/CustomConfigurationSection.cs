using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CommonApplicationFramework.ConfigurationHandling
{
	public class CustomConfigurationSection : ConfigurationSection
	{
		/// <summary>
		/// The name of this section in the app.config.
		/// </summary>
        public static Configuration CustomConfig;
        public static CustomConfigurationSection CustomSectionConfig;
		public const string SectionName = "CustomConfigurationSection";

        private const string EnvironmentCollectionName = "Environments";
		private const string EndpointCollectionName = "Endpoints";
        private const string DBServerCollectionName = "DBServers";

               
        [ConfigurationProperty(EnvironmentCollectionName)]
        [ConfigurationCollection(typeof(EnvironmentCollection), AddItemName = "Environment")]
        public EnvironmentCollection Environments { get { return (EnvironmentCollection)base[EnvironmentCollectionName]; } }
        
		[ConfigurationProperty(EndpointCollectionName)]
		[ConfigurationCollection(typeof(EndpointCollection), AddItemName = "add")]
		public EndpointCollection Endpoints { get { return (EndpointCollection)base[EndpointCollectionName]; } }


        [ConfigurationProperty(DBServerCollectionName)]
        [ConfigurationCollection(typeof(DBServerCollection), AddItemName = "add")]
        public DBServerCollection DBServers { get { return (DBServerCollection)base[DBServerCollectionName]; } }

		
	}

    public class EnvironmentElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }
    }

    public class EnvironmentsCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("Settings")]
        [ConfigurationCollection(typeof(EnvironmentElementCollection))]
        public List<EnvironmentElement> Settings
        {
            get { return (List<EnvironmentElement>)this["Settings"]; }
            set { this["Settings"] = value; }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EnvironmentElement)element).Key;
        }
      
    }

    public class EnvironmentElementCollection : ConfigurationElementCollection
    {
        
        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EnvironmentElement)element).Key;
        }
    }

    public class EnvironmentCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
           return new EnvironmentsCollection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EnvironmentsCollection)element).Name;
        }
    }

    public class EndpointCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EndpointElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EndpointElement)element).Name;
        }
    }

    public class EndpointElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("address", IsRequired = true)]
        public string Address
        {
            get { return (string)this["address"]; }
            set { this["address"] = value; }
        }       
    }

    public class DBServerCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DBServerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DBServerElement)element).Name;
        }
    }

    public class DBServerElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return (string)this["connectionString"]; }
            set { this["connectionString"] = value; }
        }

        [ConfigurationProperty("providerName", IsRequired = true)]
        public string ProviderName
        {
            get { return (string)this["providerName"]; }
            set { this["providerName"] = value; }
        }
    }
}
