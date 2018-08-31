using System;

namespace M3Pact.LoggerUtility.Helpers
{
    public interface IDataBaseHelper
    {
        /// <summary>
        /// Logs message in Database Table
        /// </summary>
        /// <param name="message">Message which need to be logged in Database Table</param>
        void LogInDB(string message);

        /// <summary>
        /// Logs message and exception details in Database table
        /// </summary>
        /// <param name="message">Message which need to be logged in Database Table</param>
        /// <param name="e">Exception for which details need to be logged in Database Table</param>
        void LogInDB(string message, Exception e);

        /// <summary>
        /// Logs message and additional details in Database Table
        /// </summary>
        /// <param name="message">Message which need to be logged in Database Table</param>
        /// <param name="logObject">Anonymous Object which contain additional details</param>
        void LogInDB(string message, dynamic logObject);

        /// <summary>
        /// Logs message,exception details and additional details in Database Table
        /// </summary>
        /// <param name="message">Message which need to be logged in Database Table</param>
        /// <param name="e">Exception for which details need to be logged in Database Table</param>
        /// <param name="logObject">Anonymous Object which contain additional details</param>
        void LogInDB(string message, Exception e, dynamic logObject);
    }
}
