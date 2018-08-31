using AppWithScheduler.Code;
using AppWithScheduler.Code.Scheduling;
using M3Pact.Business;
using M3Pact.DomainModel.DomainModels;
using M3Pact.LoggerUtility;
using M3Pact.LoggerUtility.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace AppWithScheduler
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services
            services.AddCors();
            services.AddMvc();

            // Add scheduled tasks & scheduler
            services.AddSingleton<IScheduledTask, KPIAlertTask>();
            services.AddSingleton<IConfiguration>(Configuration);         
            services.AddScheduler((sender, args) =>
            {
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });

            var connection = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(this.Configuration, "M3PactConnection");
            services.AddDbContext<M3PactContext>(options => options.UseSqlServer(connection));            

            DatabaseLoggerSettings dataBaseLoggerSettings = new DatabaseLoggerSettings
            {
                DbSettings = new DbSettings
                {
                    Columns = new List<Column>{
                        new Column{ MapTo="Time",Name="Time",Type="DATETIME"},
                        new Column{ MapTo="LogLevel",Name="LogLevel",Type="TEXT"},
                        new Column{ MapTo="Message",Name="Message",Type="TEXT"},
                        new Column{ MapTo="Exception",Name="Exception",Type="TEXT"},
                        new Column{ MapTo="StackTrace",Name="StackTrace",Type="TEXT"},
                    },
                    ConnectionString = connection,
                    TableName = "Log"
                },
                LogLevel = M3Pact.LoggerUtility.LogLevel.Error
            };

            services.AddDbContext<M3PactContext>(options => options.UseSqlServer(connection));
            services.AddLogger(
               new CompoisteLoggerSettings(
                   new List<LoggerSettings>{
                        dataBaseLoggerSettings
                   }));            
            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
