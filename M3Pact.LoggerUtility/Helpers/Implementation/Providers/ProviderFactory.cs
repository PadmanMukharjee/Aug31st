using ConfigManager;
using M3Pact.LoggerUtility.Helpers.Interface;

namespace M3Pact.LoggerUtility.Helpers.Implementation
{
    /// <summary>
    /// Factory class for privders like Configuration, Datetime, Path etc
    /// </summary>
    public class ProviderFactory : IProviderFactory
    {
        /// <summary>
        /// Returns an Instance of IConfigurationProvider
        /// </summary>
        /// <returns></returns>
        public IConfigurationProvider GetConfigurationProvider(string filePath)
        {
            return new ConfigurationProvider(filePath);
        }

        /// <summary>
        /// Returns an instance of IDateTimeProvider
        /// </summary>
        /// <returns></returns>
        public IDateTimeProvider GetDateTimeProvider()
        {
            return new DateTimeProvider();
        }

        /// <summary>
        /// Returns an instance of IPathProvider
        /// </summary>
        /// <returns></returns>
        public IPathProvider GetPathProvider()
        {
            return new PathProvider();
        }
    }
}
