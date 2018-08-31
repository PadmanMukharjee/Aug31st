using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Meridian.AuthServer.Business.IdentityServer.Services;
using Meridian.AuthServer.Business.IdentityServer.Stores;
using Meridian.AuthServer.Business.IdentityServer.Validator;
using Meridian.AuthServer.BusinessModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Meridian.AuthServer.Api.Extensions
{
    public static class AuthenticationService
    {
        public static void AddAuthServer(this IServiceCollection services, IConfiguration configuration)
        {
            AddSigninCredential(services, configuration);
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IResourceOwnerPasswordValidator, CustomResourceOwnerPasswordValidator>();
            services.AddTransient<IClientStore, ClientStore>();
            services.AddTransient<IResourceStore, ResourceStore>();
            services.AddSingleton(GetAPIResources(configuration));
            services.AddSingleton(GetClientApps(configuration));
        }

        private static void AddSigninCredential(IServiceCollection services, IConfiguration configuration)
        {
            var keyIssuer = "456151";//configuration["AuthenticationSettings:KeyStoreIssuer"];

            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certificates = store.Certificates.Find(X509FindType.FindByIssuerName, keyIssuer, true);
            if (certificates?.Count > 0)
            {
                services.AddIdentityServer().AddSigningCredential(certificates[0]);
            }
            else
            {
                services.AddIdentityServer()
                    .AddDeveloperSigningCredential();
            }
        }

        private static IList<Application> GetClientApps(IConfiguration configuration)
        {
            return new List<Application>
            {
                new Application(){
                    Key="M3pact",
                    Secret = "secret",
                    Name = "M3PACT",
                    TypeId = 3,  // Use 3 to test from Postman
                    RedirectUrl ="http://localhost:57615/src/app/login/Index.html",
                    ApplicationAPIResources = new List<ApplicationAPIResource>(){ new ApplicationAPIResource() { APIResourceName= "M3Pact_API" } }           
                }
            };
        }

        private static IList<APIResource> GetAPIResources(IConfiguration configuration)
        {
            return new List<APIResource>
            {
                new APIResource()
                {
                    Name="M3Pact_API"
                }
            };
        }
    }
}

