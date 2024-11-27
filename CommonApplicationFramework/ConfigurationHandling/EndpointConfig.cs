using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace CommonApplicationFramework.ConfigurationHandling
{
   
	/// <summary>
	/// A collection of known Endpoints.
	/// </summary>
	public static class Endpoints
	{
		/// <summary>
		/// A list of known Connection Environments.
		/// </summary>
        public static readonly List<EndpointElement> EndpointsList = new List<EndpointElement>();
       // public static IEnumerable<EndpointElement> EndpointsList { get { foreach (var endpoint in _endpointsList) { yield return endpoint; } } }
		
		/// <summary>
		/// Constructor.
		/// </summary>
		static Endpoints()
		{
           

			// Grab the Endpoints listed in the App.config and add them to our list.
            //var customSection = CustomSectionConfig.GetSection(CustomConfigurationSection.SectionName) as CustomConfigurationSection;
            if (CustomConfigurationSection.CustomSectionConfig != null)
            {
                if (EndpointsList.Count == 0)
                {
                    foreach (EndpointElement endpointElement in CustomConfigurationSection.CustomSectionConfig.Endpoints)
                    {
                        var endpoint = new EndpointElement() { Name = endpointElement.Name, Address = endpointElement.Address };
                        AddEndpoint(endpoint);
                    }
                }
            }	
		}

        /// <summary>
        /// Adds the given Endpoint to the list of Endpoints.
        /// <para>NOTE: Null, duplicate, and Invalid values will not be added.</para>
        /// </summary>
        /// <param name="endpoint">The Endpoint to add.</param>
        public static void AddEndpoint(EndpointElement endpoint)
        {
            if (endpoint == null)
                return;

            if (!EndpointsList.Contains(endpoint))
                EndpointsList.Add(endpoint);
        }

        /////// <summary>
        /////// Removes the given Endpoint from the list of Endpoints.
        /////// </summary>
        /////// <param name="endpoint">The Endpoint to remove.</param>
        ////public static void RemoveEndpoint(Endpoint endpoint)
        ////{
        ////    if (endpoint == null)
        ////        return;

        ////    if (_endpointsList.Contains(endpoint))
        ////        _endpointsList.Remove(endpoint);
        ////}
	}
}
