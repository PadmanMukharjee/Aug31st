using IdentityServer4.Models;
using IdentityServer4.Stores;
using Meridian.AuthServer.Business.IdentityServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meridian.AuthServer.Business.IdentityServer.Stores
{
    public class ResourceStore : IResourceStore
    {
        private readonly IList<BusinessModel.APIResource> _apiResources;

        public ResourceStore(IList<BusinessModel.APIResource> apiResources)
        {
            _apiResources = apiResources;
        }

        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            var apiResource = _apiResources.FirstOrDefault(c => c.Name == name);
            ApiResource resource = null;
            if (apiResource != null)
            {
                resource = new ApiResource(apiResource.Name, apiResource.Name, new List<string>() { AuthConstant.Role });
            }
            return Task.FromResult(resource);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var apiResourcesList = new List<ApiResource>();
            var apiResourceList = _apiResources;

            foreach (var item in scopeNames)
            {
                var apiResource = apiResourceList.FirstOrDefault(c => c.Name == item);
                if (apiResource != null)
                {
                    apiResourcesList.Add(new ApiResource(apiResource.Name, apiResource.Name, 
                        new List<string>() { AuthConstant.Role }));
                }
            }

            return Task.FromResult(apiResourcesList.AsEnumerable());
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var identityResourcesList = new List<IdentityResource>();

            var identityResource = GetIdentityResource();

            foreach (var item in scopeNames)
            {
                var identity = identityResource.FirstOrDefault(c => c.Name == item);
                if (identity != null)
                {
                    identityResourcesList.Add(new IdentityResource(identity.Name, identity.DisplayName, identity.UserClaims));
                }
            }

            return Task.FromResult(identityResource.AsEnumerable());
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var apiResourcesList = new List<ApiResource>();
            foreach (var item in _apiResources)
            {
                apiResourcesList.Add(new ApiResource(item.Name, item.Name, new List<string>() { AuthConstant.Role }));
            }
            var result = new Resources(GetIdentityResource(), apiResourcesList);

            return Task.FromResult(result);
        }

        private List<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource(AuthConstant.Roles, new List<string>() { AuthConstant.Role })
            };
        }
    }
}
