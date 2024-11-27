using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;


namespace CommonApplicationFramework.ConfigurationHandling
{
   
	/// <summary>
	/// A collection of known DB Servers
	/// </summary>
	public static class DBSServers
	{
		/// <summary>
		/// A list of known Connection Environments.
		/// </summary>
        public static readonly List<DBServerElement> DBServerList = new List<DBServerElement>();
      //  public static IEnumerable<DBServerElement> DBServerList { get { foreach (var dbServer in _dbServersList) { yield return dbServer; } } }
      

		/// <summary>
		/// Constructor.
		/// </summary>
        static DBSServers()
		{
			// Grab the Data Centers listed in the App.config and add them to our list.
			//var customSection = ConfigurationManager.GetSection(CustomConfigurationSection.SectionName) as CustomConfigurationSection;
            if (CustomConfigurationSection.CustomSectionConfig != null)
            {
                if (DBServerList.Count == 0)
                {
                    foreach (DBServerElement dbServerElement in CustomConfigurationSection.CustomSectionConfig.DBServers)
                    {
                        var dbServer = new DBServerElement() { Name = dbServerElement.Name, ConnectionString = dbServerElement.ConnectionString, ProviderName = dbServerElement.ProviderName };
                        AddDBServerInfo(dbServer);
                    }
                }
               
            }
          
		}

		/// <summary>
		/// Adds the given DataCenter to the list of DataCenters.
		/// <para>NOTE: Null, duplicate, and Invalid values will not be added.</para>
		/// </summary>
		/// <param name="dataCenter">The DataCenter to add.</param>
        public static void AddDBServerInfo(DBServerElement dbServer)
		{
            if (CustomConfigurationSection.CustomSectionConfig != null)
            {
                foreach (DBServerElement dbServerElement in CustomConfigurationSection.CustomSectionConfig.DBServers)
                {
                   if (dbServer == null || string.IsNullOrEmpty(dbServer.ConnectionString) || string.IsNullOrEmpty(dbServer.ProviderName))
                        continue;

                   if (!DBServerList.Contains(dbServer))
                       DBServerList.Add(dbServer);
                }
            }
           
		}
      
	}
}
