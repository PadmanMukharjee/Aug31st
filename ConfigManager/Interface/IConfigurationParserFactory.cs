namespace ConfigManager
{
    /// <summary>
    /// Defines interface for ConfigParserFactory
    /// </summary>
    public interface IConfigurationParserFactory
    {
        /// <summary>
        /// returns IConfigParser object base on the file path specified
        /// </summary>
        IConfigurationParser GetConfigurationParser(string configFilePath);
    }
}
