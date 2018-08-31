using M3Pact.LoggerUtility.LogTargets.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3Pact.LoggerUtility.Settings
{
    public abstract class LoggerSettings
    {
        public LogLevel LogLevel { get; set; }
        internal abstract List<ILogTarget> GetLogTargets();
    }
}
