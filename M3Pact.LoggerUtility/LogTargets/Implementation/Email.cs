using System;
using M3Pact.LoggerUtility.LogTargets.Interface;
using M3Pact.LoggerUtility.Formatters;
using Notification.Helpers.Interface;

namespace M3Pact.LoggerUtility.LogTargets.Implementation
{
    /// <summary>
    /// LogTarget that Logs by sending email
    /// </summary>
    public class Email : ILogTarget
    {
        private readonly ILogMessageFormater _formater;
        private readonly IEmailHelper _sender;

        public Email(ILogMessageFormater formater, IEmailHelper sender)
        {
            _formater = formater;
            _sender = sender;
        }

        /// <summary>
        /// Logs message
        /// </summary>
        public void Log(string message)
        {
            _sender.Send(_formater.GetFormatedLogMessage(message));
        }

        /// <summary>
        /// Logs message and exception
        /// </summary>
        /// <param name="message">log message</param>
        /// <param name="e">exception</param>
        public void Log(string message, Exception exception)
        {
            _sender.Send(_formater.GetFormatedLogMessage(message, exception));
        }

        /// <summary>
        /// Logs message and anonymous object
        /// </summary>
        /// <param name="message">log message</param>
        /// <param name="obj">anonymous object</param>
        public void Log(string message, dynamic logObject)
        {
            _sender.Send(_formater.GetFormatedLogMessage(message, logObject));
        }

        /// <summary>
        /// Logs message , exception and anonymous object
        /// </summary>
        /// <param name="e">exception</param>
        /// <param name="message">log message</param>
        /// <param name="obj">anonymous object</param>
        public void Log(Exception exception, string message, dynamic logObject)
        {
            _sender.Send(_formater.GetFormatedLogMessage(message, exception, logObject));
        }
    }
}
