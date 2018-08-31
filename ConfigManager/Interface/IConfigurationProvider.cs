namespace ConfigManager
{
    /// <summary>
    /// Defines interface for Configuration Provider
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Get value for the specified key from configuration file
        /// </summary>
        string GetValue(string key);
    }
}
