using System.Collections.Generic;

namespace ConfigManager
{
    /// <summary>
    /// Provides implementation for ConfigurationProvider and provids methods GetValue
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {
        private IConfigurationParser configParser;

        /// <summary>
        /// Constructor optionally accepts config file name. 
        /// If nothing is passed considers App.config as config file
        /// </summary>
        /// <param name="configFilePath">Config file name</param>
        public ConfigurationProvider(string configFilePath = null)
        {
            IConfigurationParserFactory configParserFactory = new ConfigurationParserFactory();
            configParser = configParserFactory.GetConfigurationParser(configFilePath);
        }

        /// <summary>
        /// Gets value from config file by specifying key i.e, section name in config file as an argument.
        /// </summary>
        /// <param name="key">section name in config file</param>
        /// <returns>return value in string fomrat</returns>
        public string GetValue(string key)
        {
            return configParser.GetValue(key);
        }

        /// <summary>
        /// Utility function to get list of targets from Notification config.
        /// </summary>
        /// <returns></returns>
        public List<string> GetList()
        {
            return configParser.GetList();
        }
    }
}