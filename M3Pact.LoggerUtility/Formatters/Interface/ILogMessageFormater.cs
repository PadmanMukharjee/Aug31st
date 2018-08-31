using System;

namespace M3Pact.LoggerUtility.Formatters
{
    /// <summary>
    /// Defines interface for LogMessageFormater object
    /// </summary>
    public interface ILogMessageFormater
    {
        string GetFormatedLogMessage(string message);

        string GetFormatedLogMessage(string message, Exception ex);

        string GetFormatedLogMessage(string message, dynamic additionalData);

        string GetFormatedLogMessage(string message, Exception ex, dynamic additionalData);

    }
}
