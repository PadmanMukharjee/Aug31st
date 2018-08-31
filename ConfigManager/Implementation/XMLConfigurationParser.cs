using System.Xml;
using System.Configuration;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

namespace ConfigManager
{
    /// <summary>
    /// Parses XML configuration files and provides method to get values from configuration file
    /// </summary>
    public class XMLConfigurationParser : IConfigurationParser
    {
        private readonly XmlDocument _xmlDoc;
        private bool isConfigFileAppConfig;

        /// <summary>
        /// Constructor accepts xml file path and optional bool isAppConfig as argument. It reads configuration data
        /// from specified config file.
        /// </summary>
        /// <param name="isAppConfig">Boolean whose value is true if config file is App.config </param>
        public XMLConfigurationParser(string filePath, bool isAppConfig = false)
        {
            string path = Directory.GetCurrentDirectory();
            _xmlDoc = new XmlDocument();
            _xmlDoc.LoadXml(File.ReadAllText(filePath));
            isConfigFileAppConfig = isAppConfig;
        }

        /// <summary>
        /// Gets value corresponding to the specifed string argument  from the config file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            if (isConfigFileAppConfig)
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }

            switch (key)
            {
                case "MailFrom":
                case "MailTo":
                case "SMTPPort":
                case "SMTPHost":
                case "FromPassword":
                case "MailSubject":
                    return _xmlDoc.SelectSingleNode("/NotificationConfig/EmailConfig/" + key).InnerText;
                default:
                    throw new Exception("key not found in config file");
            }
        }

        /// <summary>
        /// Utility function that is meant to be used in Notification Project to get list of targets
        /// </summary>
        /// <returns>List of Targets from in config file</returns>
        public List<string> GetList()
        {
            List<string> targets = new List<string>();
            XmlNodeList targetsList = _xmlDoc.DocumentElement.SelectNodes("/NotificationConfig/Targets");
            foreach (XmlNode node in targetsList)
            {
                foreach (XmlNode child in node)
                {
                    targets.Add(child.Attributes["name"].Value);
                }
            }
            return targets;
        }
    }
}
