using System;
using System.Collections.Generic;
using System.Text;
using M3Pact.LoggerUtility.Formatters;
using M3Pact.LoggerUtility.Helpers.Implementation;
using M3Pact.LoggerUtility.LogTargets.Implementation;
using M3Pact.LoggerUtility.LogTargets.Interface;

namespace M3Pact.LoggerUtility.Settings
{
    public class ConsoleLoggerSettings : LoggerSettings
    {
        internal override List<ILogTarget> GetLogTargets()
        {
            var targets = new List<ILogTarget>();
            var formater = new LogMessageFormater(LogLevel, new ProviderFactory(), "|");
            var target = new Output(formater, new ConsoleWriter());
            targets.Add(target);
            return targets;
        }
    }
}
