using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using ConfigurationSection = System.Configuration.ConfigurationSection;

namespace CommonApplicationFramework.ConfigurationHandling
{
    public class APIConfigurationManager : ConfigurationSection
    {
        public APIConfigurationManager()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
            NameValueSectionHandler handler = new NameValueSectionHandler();
            string path = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            //path = path.Substring(0, path.Length - 3);
            //Configure Error Configurations
            if (File.Exists(path + "\\wwwroot\\App_Data\\MessageConfigurations.config"))
            {
                fileMap.ExeConfigFilename = path + "\\wwwroot\\App_Data\\MessageConfigurations.config";
                MessageConfig.MessageCodeConfig = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                MessageConfig EC = new MessageConfig();
            }
            //Configure Query Configurations
            if (File.Exists(path + "\\wwwroot\\App_Data\\QueryConfigurations.config"))
            {
                fileMap.ExeConfigFilename = path + "\\wwwroot\\App_Data\\QueryConfigurations.config";
                QueryConfig.QuerySectionConfig = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                QueryConfig QC = new QueryConfig();
            }
            //Configure Environment Configuration
            if (File.Exists(path + "\\wwwroot\\App_Data\\Api.config"))
            {
                fileMap.ExeConfigFilename = path + "\\wwwroot\\App_Data\\Api.config";
                CustomConfigurationSection.CustomConfig = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                CustomConfigurationSection.CustomSectionConfig = CustomConfigurationSection.CustomConfig.GetSection(CustomConfigurationSection.SectionName) as CustomConfigurationSection;
            }
            // string connectionString = DBSServers.DBServerList.Find(x => x.Name.Equals(ConfigurationManager.AppSettings["HostName"].ToString())).ConnectionString;
            //string url = Endpoints.EndpointsList.Find(x => x.Name.Equals(ConfigurationManager.AppSettings["HostName"].ToString())).Address;
            //EnvironmentsCollection env = Environments.EnvironmentsList.Find(x => x.Name.Equals(ConfigurationManager.AppSettings["HostName"]));
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            string hostName = config.Build().GetSection("HostName").Value;

            if(Endpoints.EndpointsList!=null && Endpoints.EndpointsList.Count>0)
            {
                string url = Endpoints.EndpointsList.Find(x => x.Name.Equals(hostName)).Address;
                EnvironmentsCollection env = Environments.EnvironmentsList.Find(x => x.Name.Equals(hostName));
            }


        }
    }   
}
