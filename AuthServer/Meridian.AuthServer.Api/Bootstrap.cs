using Meridian.AuthServer.Business;
using Meridian.AuthServer.BusinessInterfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meridian.AuthServer.AuthServer.Api
{
    public class Bootstrap
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserLoginBusiness, UserLoginBusiness>();
            services.AddScoped<IPasswordHasher, PasswordHash>();
            //will register dependency of repository layer
            Business.Bootstrap.Register(services, configuration);           
        }
    }
}
