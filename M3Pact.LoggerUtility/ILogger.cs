using System;
using System.Collections.Generic;
using System.Text;

namespace M3Pact.LoggerUtility
{
    public interface ILogger
    {
        void Debug(Exception e, string message);
        void Debug(Exception e, string message, dynamic logObject);
        void Debug(string message);
        void Debug(string message, dynamic logObject);
        void Error(Exception e, string message);
        void Error(Exception e, string message, dynamic logObject);
        void Error(string message);
        void Error(string message, dynamic logObject);
        void Fatal(Exception e, string message);
        void Fatal(Exception e, string message, dynamic logObject);
        void Fatal(string message);
        void Fatal(string message, dynamic logObject);
        void Info(Exception e, string message);
        void Info(Exception e, string message, dynamic logObject);
        void Info(string message);
        void Info(string message, dynamic logObject);
        void Log(Exception e, LogLevel logLevel, string message);
        void Log(LogLevel logLevel, Exception e, string message, dynamic logObject);
        void Log(LogLevel logLevel, string message);
        void Log(LogLevel logLevel, string message, dynamic logObject);
        void Trace(Exception e, string message);
        void Trace(Exception e, string message, dynamic logObject);
        void Trace(string message);
        void Trace(string message, dynamic logObject);
        void Warn(Exception e, string message);
        void Warn(Exception e, string message, dynamic logObject);
        void Warn(string message);
        void Warn(string message, dynamic logObject);
    }
}
