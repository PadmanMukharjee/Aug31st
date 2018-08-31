using M3Pact.LoggerUtility.Formatters;
using M3Pact.LoggerUtility.Helpers;
using M3Pact.LoggerUtility.Helpers.Implementation;
using M3Pact.LoggerUtility.Helpers.Implementation.Providers;
using M3Pact.LoggerUtility.LogTargets.Implementation;
using M3Pact.LoggerUtility.LogTargets.Interface;
using System.Collections.Generic;

namespace M3Pact.LoggerUtility.Settings
{
    public class FlatFileLoggerSettings: LoggerSettings
    {
        public string FilePath { get; set; }
        public int Frequency { get; set; }

        internal override List<ILogTarget> GetLogTargets()
        {
            var targets = new List<ILogTarget>();
            var formater = new LogMessageFormater(LogLevel, new ProviderFactory(), "|");
            var target = new Output(formater,
                        new FileWriter(
                            new FrequencyBaseFileNameProvider(
                                new FrequencyBasedFileNameProviderFactory(
                                    new ProviderFactory()), this
                                )));
            targets.Add(target);
            return targets;
        }
    }
}
