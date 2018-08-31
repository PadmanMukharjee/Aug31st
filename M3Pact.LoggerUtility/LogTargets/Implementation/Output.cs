using System;
using M3Pact.LoggerUtility.LogTargets.Interface;
using M3Pact.LoggerUtility.Helpers;
using M3Pact.LoggerUtility.Formatters;

namespace M3Pact.LoggerUtility.LogTargets.Implementation
{
    /// <summary>
    /// LogTarget that writes output using IOutputWriter
    /// </summary>
    public class Output : ILogTarget
    {
        private readonly ILogMessageFormater _formater;
        private readonly IOutputWriter _writer;

        public Output(ILogMessageFormater formater, IOutputWriter writer)
        {
            _formater = formater;
            _writer = writer;
        }

        /// <summary>
        /// Logs message
        /// </summary>
        public void Log(string message)
        {
            _writer.Write(_formater.GetFormatedLogMessage(message));
        }

        /// <summary>
        /// Logs message and exception
        /// </summary>
        /// <param name="message">log message</param>
        /// <param name="e">exception</param>
        public void Log(string message, Exception exception)
        {
            _writer.Write(_formater.GetFormatedLogMessage(message, exception));
        }

        /// <summary>
        /// Logs message and anonymous object
        /// </summary>
        /// <param name="message">log message</param>
        /// <param name="obj">anonymous object</param>
        public void Log(string message, dynamic logObject)
        {
            _writer.Write(_formater.GetFormatedLogMessage(message, logObject));
        }

        /// <summary>
        /// Logs message , exception and anonymous object
        /// </summary>
        /// <param name="e">exception</param>
        /// <param name="message">log message</param>
        /// <param name="obj">anonymous object</param>
        public void Log(Exception exception, string message, dynamic logObject)
        {
            _writer.Write(_formater.GetFormatedLogMessage(message, exception, logObject));
        }
    }
}