using IdentityServer4.Models;
using IdentityServer4.Stores;
using Meridian.AuthServer.Business.IdentityServer.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meridian.AuthServer.Business.IdentityServer.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly IConfiguration _configuration;
        private readonly IList<BusinessModel.Application> _applications;
        //private readonly IApplicationRepository _clientAppRepository;

        public ClientStore(IConfiguration Configuration,
            IList<BusinessModel.Application> applications)
        {
            _configuration = Configuration;
            _applications = applications;
            //_clientAppRepository = clientAppRepository;
        }

        public Task<Client> FindClientByIdAsync(string clientApplicationKey)
        {
            IdentityClient identityClient = null;
            var clientAppInfo = _applications.FirstOrDefault(a => a.Key == clientApplicationKey);
            if (clientAppInfo != null)
            {
                ApplicationType applicationType = clientAppInfo.TypeId.HasValue
                    ? (ApplicationType)clientAppInfo.TypeId : ApplicationType.Web_MVC;

                identityClient = new IdentityClient(_configuration, applicationType, clientAppInfo.ApplicationAPIResources)
                {
                    ClientId = clientAppInfo.Key,
                    ClientName = clientAppInfo.Name,
                    ClientSecret = clientAppInfo.Secret,
                    RedirectUri = clientAppInfo.RedirectUrl,
                };
            }
            return Task.FromResult((Client)identityClient);
        }
    }
}
