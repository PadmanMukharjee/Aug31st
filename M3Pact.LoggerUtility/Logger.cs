using System;
using System.Collections.Generic;
using M3Pact.LoggerUtility.LogTargets.Interface;
using M3Pact.LoggerUtility.Settings;

namespace M3Pact.LoggerUtility
{
    /// <summary>
    /// Provides logging interface
    /// </summary>
    public class Logger: ILogger
    {
        private readonly LoggerSettings _loggerSettings;

        /// <summary>
        /// Constructor is used to set the value of instance variables depending on the log level & set the log settings
        /// </summary>
        /// <param name="loggerSettings"></param>
        public Logger(LoggerSettings loggerSettings)
        {
            this._loggerSettings = loggerSettings;
        }

        /// <summary>
        /// Calls Log method on each target specified in config file when the log level of the message is greater than or equal to
        /// the log level specified in config file
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        private void LogMessage(LogLevel level, string message)
        {
            if (_loggerSettings.LogLevel <= level)
            {
                foreach (ILogTarget target in _loggerSettings.GetLogTargets())
                {
                    target.Log(message);
                }
            }
        }

        /// <summary>
        /// Calls Log method on each target specified in config file when the log level of the message is greater than or equal to
        /// the log level specified in config file
        /// </summary>
        /// <param name="e"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        private void LogMessage(Exception e, LogLevel level, string message)
        {
            if (_loggerSettings.LogLevel <= level)
            {
                foreach (ILogTarget target in _loggerSettings.GetLogTargets())
                {
                    target.Log(message, e);
                }
            }
        }

        /// <summary>
        /// Calls Log method on each target specified in config file when the log level of the message is greater than or equal to
        /// the log level specified in config file
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        private void LogMessage(LogLevel level, string message, dynamic logObject)
        {
            if (_loggerSettings.LogLevel <= level)
            {
                foreach (ILogTarget target in _loggerSettings.GetLogTargets())
                {
                    target.Log(message, logObject);
                }
            }
        }

        /// <summary>
        /// Calls Log method on each target specified in config file when the log level of the message is greater than or equal to
        /// the log level specified in config file
        /// </summary>
        /// <param name="level"></param>
        /// <param name="e"></param>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        private void LogMessage(LogLevel level, Exception e, string message, dynamic logObject)
        {
            if (_loggerSettings.LogLevel <= level)
            {
                foreach (ILogTarget target in _loggerSettings.GetLogTargets())
                {
                    target.Log(e, message, logObject);
                }
            }
        }

        /// <summary>
        /// It logs INFO level message into the targets specified in config file
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            LogMessage(LogLevel.Info, message);
        }

        /// <summary>
        /// It logs INFO level message into the targets specified in config file
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        public void Info(Exception e, string message)
        {
            LogMessage(e, LogLevel.Info, message);
        }

        /// <summary>
        /// It logs INFO level message into the targets specified in config file
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Info(string message, dynamic logObject)
        {
            LogMessage(LogLevel.Info, message, logObject);
        }

        /// <summary>
        /// It logs INFO level message into the targets specified in config file
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Info(Exception e, string message, dynamic logObject)
        {
            LogMessage(LogLevel.Info, e, message, logObject);
        }

        /// <summary>
        /// It logs DEBUG level message into the targets specified in config file
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            LogMessage(LogLevel.Debug, message);
        }

        /// <summary>
        ///  It logs DEBUG level message into the targets specified in config file
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        public void Debug(Exception e, string message)
        {
            LogMessage(e, LogLevel.Debug, message);
        }

        /// <summary>
        ///  It logs DEBUG level message into the targets specified in config file
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Debug(string message, dynamic logObject)
        {
            LogMessage(LogLevel.Debug, message, logObject);
        }

        /// <summary>
        ///  It logs DEBUG level message into the targets specified in config file
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Debug(Exception e, string message, dynamic logObject)
        {
            LogMessage(LogLevel.Debug, e, message, logObject);
        }

        /// <summary>
        /// It logs ERROR level message into the targets specified in config file
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            LogMessage(LogLevel.Error, message);
        }

        /// <summary>
        /// It logs ERROR level message into the targets specified in config file
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        public void Error(Exception e, string message)
        {
            LogMessage(e, LogLevel.Error, message);
        }

        /// <summary>
        /// It logs ERROR level message into the targets specified in config file
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Error(string message, dynamic logObject)
        {
            LogMessage(LogLevel.Error, message, logObject);
        }

        /// <summary>
        /// It logs ERROR level message into the targets specified in config file
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Error(Exception e, string message, dynamic logObject)
        {
            LogMessage(LogLevel.Error, e, message, logObject);
        }

        /// <summary>
        /// It logs WARN level message into the targets specified in config file
        /// </summary>
        /// <param name="message"></param>
        public void Warn(string message)
        {
            LogMessage(LogLevel.Warn, message);
        }

        /// <summary>
        /// It logs WARN level message into the targets specified in config file
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        public void Warn(Exception e, string message)
        {
            LogMessage(e, LogLevel.Warn, message);
        }

        /// <summary>
        /// It logs WARN level message into the targets specified in config file
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Warn(string message, dynamic logObject)
        {
            LogMessage(LogLevel.Warn, message, logObject);
        }

        /// <summary>
        /// It logs WARN level message into the targets specified in config file
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Warn(Exception e, string message, dynamic logObject)
        {
            LogMessage(LogLevel.Warn, e, message, logObject);
        }

        /// <summary>
        /// It logs message with FATAL level into the targets specified in config file
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(string message)
        {
            LogMessage(LogLevel.Fatal, message);
        }

        /// <summary>
        /// It logs message with FATAL level into the targets specified in config file
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        public void Fatal(Exception e, string message)
        {
            LogMessage(e, LogLevel.Fatal, message);
        }

        /// <summary>
        /// It logs message with FATAL level into the targets specified in config file
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Fatal(string message, dynamic logObject)
        {
            LogMessage(LogLevel.Fatal, message, logObject);
        }

        /// <summary>
        /// It logs message with FATAL level into the targets specified in config file
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Fatal(Exception e, string message, dynamic logObject)
        {
            LogMessage(LogLevel.Fatal, e, message, logObject);
        }

        /// <summary>
        /// It log TRACE level message into the targets specified in config file
        /// </summary>
        /// <param name="message"></param>
        public void Trace(string message)
        {
            LogMessage(LogLevel.Trace, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        public void Trace(Exception e, string message)
        {
            LogMessage(e, LogLevel.Trace, message);
        }

        /// <summary>
        ///  It log TRACE level message into the targets specified in config file
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Trace(string message, dynamic logObject)
        {
            LogMessage(LogLevel.Trace, message, logObject);
        }

        /// <summary>
        ///  It log TRACE level message into the targets specified in config file
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Trace(Exception e, string message, dynamic logObject)
        {
            LogMessage(LogLevel.Trace, e, message, logObject);
        }

        /// <summary>
        /// It calls the specific log function based on the passed loglevel
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        public void Log(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    Trace(message);
                    break;

                case LogLevel.Debug:
                    Debug(message);
                    break;

                case LogLevel.Info:
                    Info(message);
                    break;

                case LogLevel.Error:
                    Error(message);
                    break;

                case LogLevel.Warn:
                    Warn(message);
                    break;

                case LogLevel.Fatal:
                    Fatal(message);
                    break;
            }
        }

        /// <summary>
        /// It calls the specific log function based on the passed loglevel
        /// </summary>
        /// <param name="e"></param>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        public void Log(Exception e, LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    Trace(e, message);
                    break;

                case LogLevel.Debug:
                    Debug(e, message);
                    break;

                case LogLevel.Info:
                    Info(e, message);
                    break;

                case LogLevel.Error:
                    Error(e, message);
                    break;

                case LogLevel.Warn:
                    Warn(e, message);
                    break;

                case LogLevel.Fatal:
                    Fatal(e, message);
                    break;
            }
        }

        /// <summary>
        /// It calls the specific log function based on the passed loglevel
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Log(LogLevel logLevel, string message, dynamic logObject)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    Trace(message, logObject);
                    break;

                case LogLevel.Debug:
                    Debug(message, logObject);
                    break;

                case LogLevel.Info:
                    Info(message, logObject);
                    break;

                case LogLevel.Error:
                    Error(message, logObject);
                    break;

                case LogLevel.Warn:
                    Warn(message, logObject);
                    break;

                case LogLevel.Fatal:
                    Fatal(message, logObject);
                    break;
            }
        }

        /// <summary>
        /// It calls the specific log function based on the passed loglevel
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="e"></param>
        /// <param name="message"></param>
        /// <param name="logObject"></param>
        public void Log(LogLevel logLevel, Exception e, string message, dynamic logObject)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    Trace(e, message, logObject);
                    break;

                case LogLevel.Debug:
                    Debug(e, message, logObject);
                    break;

                case LogLevel.Info:
                    Info(e, message, logObject);
                    break;

                case LogLevel.Error:
                    Error(e, message, logObject);
                    break;

                case LogLevel.Warn:
                    Warn(e, message, logObject);
                    break;

                case LogLevel.Fatal:
                    Fatal(e, message, logObject);
                    break;
            }
        }
    }
}
