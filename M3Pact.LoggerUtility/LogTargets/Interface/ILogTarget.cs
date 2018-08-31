using System;

namespace M3Pact.LoggerUtility.LogTargets.Interface
{
    /// <summary>
    /// Defines interface for LogTarget
    /// </summary>
    internal interface ILogTarget
    {
        /// <summary>
        /// Logs message
        /// </summary>
        void Log(string message);

        /// <summary>
        /// Logs message and exception
        /// </summary>
        /// <param name="message">log message</param>
        /// <param name="e">exception</param>
        void Log(string message, Exception e);

        /// <summary>
        /// Logs message and anonymous object
        /// </summary>
        /// <param name="message">log message</param>
        /// <param name="obj">anonymous object</param>
        void Log(string message, dynamic obj);

        /// <summary>
        /// Logs message , exception and anonymous object
        /// </summary>
        /// <param name="e">exception</param>
        /// <param name="message">log message</param>
        /// <param name="obj">anonymous object</param>
        void Log(Exception e, string message, dynamic obj);
    }
}
