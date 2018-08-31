using System.Collections.Generic;

namespace ConfigManager
{
    /// <summary>
    /// Defines interface for configuration Parsers like 
    /// </summary>
    public interface IConfigurationParser
    {
        /// <summary>
        /// Function to get values correspondig to specified keys from configuration file
        /// </summary>
        /// <param name="key">Key corresponding to expected return value in config file</param>
        string GetValue(string key);

        /// <summary>
        /// To be used only for getting targets from notification config
        /// </summary>
        List<string> GetList();
    }
}
