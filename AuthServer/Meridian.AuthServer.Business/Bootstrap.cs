using Meridian.AuthServer.DataModel;
using Meridian.AuthServer.Repository;
using Meridian.AuthServer.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meridian.AuthServer.Business
{
    public class Bootstrap
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            //var connection = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(this.Configuration, "M3PactConnection");
            services.AddDbContext<M3PactContext>(options => options.UseSqlServer(configuration.GetConnectionString("M3PactConnection")));
            services.AddScoped<IUserLoginRepository, UserLoginRepository>();
            //services.AddScoped<IApplicationRepository, ApplicationRepository>();
            //services.AddScoped<IAPIResourceRepository, APIResourceRepository>();
        }
    }
}