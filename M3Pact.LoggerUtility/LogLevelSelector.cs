using System;
using ConfigManager;
using M3Pact.LoggerUtility;

namespace LoggerUtility
{
    /// <summary>
    /// Provides utility functions for setting Log level priority from config
    /// </summary>
    class LogLevelSelector
    {

        /// <summary>
        /// It returns the numeric value of priority of log legel specified in the config file
        /// </summary>
        /// <returns>Priority level of Log Level set in config file</returns>
        internal static int SetLogLeveLPriorityFromConfig()
        {
            var configuration = new ConfigurationProvider();
            string level = configuration.GetValue(Constants.AppConfig_Key_LogLevel);
            LogLevel loglevel;
            if (Enum.TryParse(level, out loglevel))
                return (int)loglevel;
            else
                throw new Exception("Wrong Log Level specified in Config File");
        }
    }
}
