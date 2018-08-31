using M3Pact.LoggerUtility.Helpers.Interface;
using Newtonsoft.Json;
using System;
using ConfigManager;
namespace M3Pact.LoggerUtility.Formatters
{
    public class LogMessageFormater : ILogMessageFormater
    {
        private readonly LogLevel _logLevel;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly string _paramDelimiterInLog;


        /// <summary>
        /// Creates an instance of LogMessageFormatter.
        /// To get formatted message call GetFormatedLogMessage on it.
        /// </summary>
        public LogMessageFormater(LogLevel level, IProviderFactory providerFactory, string delimeter)
        {
            _logLevel = level;
            _dateTimeProvider = providerFactory.GetDateTimeProvider();
            //_paramDelimiterInLog = providerFactory.GetConfigurationProvider().GetValue(Constants.AppConfig_Key_ParamDelimiterInLog);
            _paramDelimiterInLog = delimeter;
        }

        /// <summary>
        /// Returns formatted Log message in string fromat.
        /// </summary>
        public string GetFormatedLogMessage(string message, Exception exception)
        {
            return GetFormatedLogMessage(message, exception, null);
        }

        /// <summary>
        /// Returns formatted Log message in string fromat.
        /// </summary>
        public string GetFormatedLogMessage(string message)
        {
            return GetFormatedLogMessage(message, null, null);
        }
        /// <summary>
        /// Returns formatted Log message in string fromat.
        /// </summary>
        public string GetFormatedLogMessage(string message, dynamic additionalData)
        {
            return GetFormatedLogMessage(message, null, additionalData);
        }
        /// <summary>
        /// Returns formatted Log message in string fromat.
        /// </summary>
        public string GetFormatedLogMessage(string message, Exception ex, dynamic additionalData)
        {
            string additionalDataInFormat = GetAdditionalParameters(additionalData);
            return string.Join(_paramDelimiterInLog, _logLevel.ToString(), _dateTimeProvider.Now, ex?.Message, ex?.StackTrace, message, additionalDataInFormat);
        }
        /// <summary>
        /// Utility function to get additional parameters from the dynamic object in string format
        /// </summary>
        private string GetAdditionalParameters(dynamic additionalData)
        {
            if (additionalData != null)
            {
                string serializeAdditionalData = JsonConvert.SerializeObject(additionalData);
                serializeAdditionalData = serializeAdditionalData.Replace("\"", "");
                string[] additionalDataParamsArray = serializeAdditionalData.Substring(1, serializeAdditionalData.Length - 2).Split(',');
                return string.Join(_paramDelimiterInLog, additionalDataParamsArray);
            }
            return null; ;
        }
    }
}
