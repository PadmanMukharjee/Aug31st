using ConfigManager;

namespace M3Pact.LoggerUtility.Helpers.Interface
{
    public interface IProviderFactory
    {
        IDateTimeProvider GetDateTimeProvider();
        IPathProvider GetPathProvider();
        IConfigurationProvider GetConfigurationProvider(string configFilePath);
    }
}
