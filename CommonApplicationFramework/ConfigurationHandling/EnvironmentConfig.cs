using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonApplicationFramework.ConfigurationHandling
{
    
   
    ////}
	/// <summary>
	/// A collection of known Environments.
	/// </summary>
	public static class Environments
	{
		/// <summary>
		/// A list of known Environments.
		/// </summary>
        public static readonly List<EnvironmentsCollection> EnvironmentsList = new List<EnvironmentsCollection>();
        public static readonly EnvironmentsCollection Configurations = new EnvironmentsCollection();
       // public static IEnumerable<Environment> EnvironmentsList { get { foreach (var environment in _environmentsList) { yield return environment; } } }
        
		/// <summary>
		/// Constructor.
		/// </summary>
		static Environments()
		{
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            string hostName = config.Build().GetSection("HostName").Value;
            // Grab the Environments listed in the Api.config and add them to our list.
            if (CustomConfigurationSection.CustomSectionConfig != null)
			{
                if (EnvironmentsList.Count == 0)
                {
                    foreach (EnvironmentsCollection env in CustomConfigurationSection.CustomSectionConfig.Environments)
                    {
                        AddEnvironment(env);
                    }
                    Configurations = EnvironmentsList.Find(x => x.Name.Equals(hostName));
                }
			}
		}

		/// <summary>
		/// Adds the given Environment to the list of Environments.
		/// <para>NOTE: Null and duplicate values will not be added.</para>
		/// </summary>
		/// <param name="environment">The Environment to add.</param>
		public static void AddEnvironment(EnvironmentsCollection  environment)
		{
            if (environment == null)
				return;
            if (!EnvironmentsList.Contains(environment))
            {
                EnvironmentsCollection env = new EnvironmentsCollection();
                env.Name = environment.Name;
                env.Settings = new List<EnvironmentElement>();
                foreach (EnvironmentElement envElement in environment)
                {
                    var element = new EnvironmentElement() { Key = envElement.Key, Value = envElement.Value };
                    env.Settings.Add(element);                   
                }
                EnvironmentsList.Add(env);
            }            
		}        
	}
}
