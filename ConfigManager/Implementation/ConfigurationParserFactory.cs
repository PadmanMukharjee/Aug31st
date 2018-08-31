using System;
using System.IO;

namespace ConfigManager
{
    /// <summary>
    /// Provides GetConfigurationParser function to get configuration parser based on file name and extension
    /// </summary>
    public class ConfigurationParserFactory : IConfigurationParserFactory
    {
        /// <summary>
        /// Function creates and returns an object of IConfigurationParser based on file name 
        /// and extention of configFilePath provided as argument. If configFilePath is set to
        /// null then App.config is used as configuration file. 
        /// </summary>
        /// <param name="configFilePath">Configuration File Name</param>
        /// <returns>an object of IConfigurationParser</returns>
        public IConfigurationParser GetConfigurationParser(string configFilePath)
        {
            if (configFilePath == null || configFilePath.Equals("appsettings.json"))
            {
                configFilePath = "appsettings.json";
                bool isAppsettings = true;
                return new JsonConfigurationParser(configFilePath, isAppsettings);

            }

            string fileExtension = Path.GetExtension(configFilePath);

            switch (fileExtension)
            {
                case ".json":
                    return new JsonConfigurationParser(configFilePath);

                case ".xml":
                    return new XMLConfigurationParser(configFilePath);

                default:
                    throw new Exception("File not recognized");
            }
        }
    }
}
