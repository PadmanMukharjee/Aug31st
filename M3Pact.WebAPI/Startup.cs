using LoggerUtility;
using System;
using M3Pact.Business;
using M3Pact.DomainModel.DomainModels;
using M3Pact.WebAPI.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using M3Pact.LoggerUtility;
using M3Pact.LoggerUtility.Settings;
using System.Collections.Generic;

namespace M3Pact.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();
            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });
            AddApiClientAuthentication(services);

            var connection = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(this.Configuration, "M3PactConnection");

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
                LogLevel = LogLevel.Error
            };

            services.AddDbContext<M3PactContext>(options => options.UseSqlServer(connection));
            services.AddLogger(
               new CompoisteLoggerSettings(
                   new List<LoggerSettings>{
                        dataBaseLoggerSettings
                   }));
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.RegisterServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "M3Pact API",
                    Description = "M3Pact API for Client,Deposits and Admins",
                    TermsOfService = "None"
                });
            });
        }

        private void AddApiClientAuthentication(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
         .AddIdentityServerAuthentication(options =>
         {
             options.Authority = Configuration["AuthenticationSettings:AuthServerUri"];
             options.ApiName = Configuration["AuthenticationSettings:ApiName"];
             options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["AuthenticationSettings:RequireHttps"]);
             options.SaveToken = true;
         });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            env.WebRootPath = System.IO.Directory.GetCurrentDirectory();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var logger = app.ApplicationServices.GetService<Logger>();
            app.UseAuthentication();
            app.UseMiddleware<IdentityMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "M3Pact API V1");
            });
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseMvc();
        }
    }
}
