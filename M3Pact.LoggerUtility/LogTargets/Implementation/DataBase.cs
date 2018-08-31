using System;
using M3Pact.LoggerUtility.LogTargets.Interface;
using M3Pact.LoggerUtility.Helpers;

namespace M3Pact.LoggerUtility.LogTargets.Implementation
{
    /// <summary>
    /// LogTarget that logs to database
    /// </summary>
    public class DataBase : ILogTarget
    {
        private readonly IDataBaseHelper _dbHelper;
        public DataBase(IDataBaseHelper helper)
        {
            _dbHelper = helper;
        }

        /// <summary>
        /// Logs message
        /// </summary>
        public void Log(string message)
        {
            _dbHelper.LogInDB(message);
        }

        /// <summary>
        /// Logs message and exception
        /// </summary>
        /// <param name="message">log message</param>
        /// <param name="e">exception</param>
        public void Log(string message, Exception exception)
        {
            _dbHelper.LogInDB(message, exception);
        }

        /// <summary>
        /// Logs message and anonymous object
        /// </summary>
        /// <param name="message">log message</param>
        /// <param name="obj">anonymous object</param>
        public void Log(string message, dynamic logObject)
        {
            _dbHelper.LogInDB(message, logObject);
        }

        /// <summary>
        /// Logs message , exception and anonymous object
        /// </summary>
        /// <param name="e">exception</param>
        /// <param name="message">log message</param>
        /// <param name="obj">anonymous object</param>
        public void Log(Exception exception, string message, dynamic logObject)
        {
            _dbHelper.LogInDB(message, exception, logObject);
        }
    }
}
