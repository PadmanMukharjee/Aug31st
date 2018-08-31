using ConfigManager;
using M3Pact.LoggerUtility.Helpers.Interface;
using M3Pact.LoggerUtility.Helpers.Interface.Providers;
using M3Pact.LoggerUtility.Settings;
using System;
using System.IO;

namespace M3Pact.LoggerUtility.Helpers.Implementation
{
    /// <summary>
    /// A Frequency Based file Name provider.
    /// This class provides the file name for the log file based on the
    /// freqeuncy configured in AppSettings
    /// </summary>
    public class FrequencyBaseFileNameProvider : IFileNameProvider
    {
        private readonly string _filePath;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IPathProvider _pathProvider;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly int _frequency;
        private readonly IFrequencyBasedFileNameProviderFactory _providerFactory;

        public FrequencyBaseFileNameProvider(IFrequencyBasedFileNameProviderFactory providerFactory,FlatFileLoggerSettings flatFileLoggerSettings)
        {
            _providerFactory = providerFactory;
            _dateTimeProvider = _providerFactory.GetDateTimeProvider();
            _pathProvider = _providerFactory.GetPathProvider();
            //_configurationProvider = _providerFactory.GetConfigurationProvider();
            //_filePath = _configurationProvider.GetValue(Constants.AppConfig_Key_FlatFilePath);
            //_frequency = Convert.ToInt32(_configurationProvider.GetValue(Constants.LogFrequencyForFlatFile));

            _filePath = flatFileLoggerSettings.FilePath;
            _frequency = flatFileLoggerSettings.Frequency;
        }

        /// <summary>
        /// Gets the file Name
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            var filePath = _pathProvider.Combine(_pathProvider.GetDirectoryPath(), _filePath);
            var fileNameWithExtenstion = _pathProvider.GetFileName(filePath);

            if (!Path.GetExtension(fileNameWithExtenstion).ToLower().Contains(Constants.AllowedFileExtensionsForLogging))
                throw new FormatException(Constants.InvalidFileFormatException);

            if (_frequency == 0)
            {
                return _pathProvider.Combine(_pathProvider.GetDirectoryPath(), _filePath);
            }

            FormatAndStoreFileNameIfRequired(fileNameWithExtenstion);
            return _providerFactory.GetPreviousFileName();
        }

        /// <summary>
        /// Formats the file name If Requried based on frequency and stores it
        /// </summary>
        /// <param name="fileName"></param>
        private void FormatAndStoreFileNameIfRequired(string fileName)
        {
            var date = _dateTimeProvider.Now.Date;
            if (string.IsNullOrWhiteSpace(_providerFactory.GetPreviousFileName())
                || _providerFactory.GetPreviousDate().AddDays(_frequency) == date)
            {
                var dateString = date.ToString(Constants.LogFileDateFormat);
                _providerFactory.SetPreviousDate(date);
                _providerFactory.SetPreviousFileName(string.Concat(fileName.Split('.')[0],
                "_",
                dateString,
                Constants.AllowedFileExtensionsForLogging));
            }
        }
    }
}
