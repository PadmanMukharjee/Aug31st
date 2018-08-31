using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using System;

namespace ConfigManager
{
    /// <summary>
    /// Parses json configuration files and provides method to get values from configuration file
    /// </summary>
    public class JsonConfigurationParser : IConfigurationParser
    {
        private static IConfigurationRoot Configuration { get; set; }

        private string configFileName;
        bool isAppsettings;

        /// <summary>
        /// Constructor accepts json file path as argument. It reads configuration data
        /// from specified config file.
        /// </summary>
        /// <param name="filePath">file path</param>
        public JsonConfigurationParser(string filePath, bool isAppsettings = false)
        {
            configFileName = filePath;
            var builder = new ConfigurationBuilder()
                        .AddJsonFile(filePath);

            Configuration = builder.Build();
            this.isAppsettings = isAppsettings;
        }

        /// <summary>
        /// Gets value corresponding to the specifed string argument  from the config file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            if (isAppsettings)
            {
                return Configuration[key];
            }
            switch (key)
            {
                case "MailFrom":
                case "MailTo":
                case "SMTPPort":
                case "SMTPHost":
                case "FromPassword":
                case "MailSubject":
                    return Configuration["EmailConfig:" + key];
                default:
                    throw new Exception("Key not found in config file");
            }
        }

        /// <summary>
        /// Utility function that is meant to be used in Notification Project to get list of targets
        /// </summary>
        /// <returns>List of Targets from in config file</returns>
        public List<string> GetList()
        {
            JObject configObject = JObject.Parse(File.ReadAllText(configFileName));
            List<string> targets = configObject.GetValue("Targets").ToObject<List<string>>();

            return targets;
        }

    }
}
