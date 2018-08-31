using System;
using System.Collections.Generic;
using System.Text;
using M3Pact.LoggerUtility.LogTargets.Interface;

namespace M3Pact.LoggerUtility.Settings
{
    public class CompoisteLoggerSettings : LoggerSettings
    {
        private readonly IEnumerable<LoggerSettings> loggerSettings;

        public CompoisteLoggerSettings(IEnumerable<LoggerSettings> loggerSettings)
        {
            this.loggerSettings = loggerSettings;
        }

        internal override List<ILogTarget> GetLogTargets()
        {
            var targets = new List<ILogTarget>();
            foreach (var setting in loggerSettings)
            {
                targets.AddRange(setting.GetLogTargets());
            }
            return targets;
        }
    }
}
