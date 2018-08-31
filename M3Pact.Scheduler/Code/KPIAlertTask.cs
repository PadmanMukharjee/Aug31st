using M3Pact.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Business.AlertsAndEscalation;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using ConfigManager;
using M3Pact.LoggerUtility;

namespace AppWithScheduler.Code
{
    public class KPIAlertTask : IScheduledTask
    {
        static IAlertAndEscalation _alertAndEscalation;
        IConfiguration _configuration;
        ILogger _logger;
        public string Schedule { get; set; }

        public KPIAlertTask(IAlertAndEscalation alertAndEscalation,IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            _alertAndEscalation = alertAndEscalation;
            _configuration = configuration;
            Schedule = _configuration.GetSection("SchedulerConfig").GetSection("ScheduleTime").Value.ToString();            
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _alertAndEscalation.InsertDeviatedMetricKPi();
            _alertAndEscalation.SendAlertDaily();
            await Task.Delay(5000, cancellationToken);
        }

        public static string GetConfigurationKey(string key)
        {
            ConfigManager.IConfigurationProvider config = new ConfigManager.ConfigurationProvider();           
            return config.GetValue(key);
        }
    }
}