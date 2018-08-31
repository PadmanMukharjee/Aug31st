using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3Pact.LoggerUtility.Settings
{
    public static class LoggerServiceCollectionExtensions
    {
        /// <summary>
        /// register the logger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns>returns Iservice Collection with Logger dependency </returns>
        public static IServiceCollection AddLogger(
           this IServiceCollection services,
           LoggerSettings settings)
        {
            services.AddSingleton(settings);
            services.AddSingleton<ILogger, Logger>();
            return services;
        }
    }
}
