using M3Pact.LoggerUtility.Helpers.Interface.Providers;
using System;
using M3Pact.LoggerUtility.Helpers.Interface;
using ConfigManager;

namespace M3Pact.LoggerUtility.Helpers.Implementation.Providers
{
    /// <summary>
    /// Factory for providing all the dependencies for the FrequencyBasedFileName provider
    /// </summary>
    public class FrequencyBasedFileNameProviderFactory : IFrequencyBasedFileNameProviderFactory
    {
        private IProviderFactory _providerFactory;
        private static string previousFileName;
        private static DateTime previousFileCreatedDate;

        public FrequencyBasedFileNameProviderFactory(IProviderFactory providerFactory)
        {
            _providerFactory = providerFactory;
        }

        /// <summary>
        /// Returns a instance of the IConfigurationProvider
        /// </summary>
        /// <returns></returns>
        public IConfigurationProvider GetConfigurationProvider(string filePath)
        {
            return _providerFactory.GetConfigurationProvider(filePath);
        }

        /// <summary>
        /// Returns an instance of IDateTimeProvider
        /// </summary>
        /// <returns></returns>
        public IDateTimeProvider GetDateTimeProvider()
        {
            return _providerFactory.GetDateTimeProvider();
        }

        /// <summary>
        /// Returns an instance of IPathProvider
        /// </summary>
        /// <returns></returns>
        public IPathProvider GetPathProvider()
        {
            return _providerFactory.GetPathProvider();
        }

        /// <summary>
        /// Returns the Previous date of the logged file
        /// </summary>
        /// <returns></returns>
        public DateTime GetPreviousDate()
        {
            return previousFileCreatedDate;
        }

        /// <summary>
        /// Returns the previous logged file name
        /// </summary>
        /// <returns></returns>
        public string GetPreviousFileName()
        {
            return previousFileName;
        }

        /// <summary>
        /// Sets the Previous log file Created Date
        /// </summary>
        /// <param name="date">Current log file created Date</param>
        public void SetPreviousDate(DateTime date)
        {
            previousFileCreatedDate = date.Date;
        }

        /// <summary>
        /// Sets the filename of currently created log file
        /// </summary>
        /// <param name="fileName">File name of currently created log file</param>
        public void SetPreviousFileName(string fileName)
        {
            previousFileName = fileName;
        }
    }
}
