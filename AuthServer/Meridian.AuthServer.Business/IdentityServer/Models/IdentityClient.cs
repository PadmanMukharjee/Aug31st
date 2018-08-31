using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Meridian.AuthServer.Business.IdentityServer.Models
{
    public class IdentityClient : IdentityServer4.Models.Client
    {
        private ApplicationType _applicationType;

        public string RedirectUri
        {
            set
            {
                if (value != null && (_applicationType == ApplicationType.Web_JS || value.Contains(AuthConstant.SignIn_OIDC)))
                {
                    RedirectUris = new List<string>() { value };
                }
                else
                {
                    RedirectUris = new List<string>() { value + AuthConstant.SignIn_OIDC };
                }
            }
        }

        public string ClientSecret
        {
            set
            {
                ClientSecrets = new List<Secret>() { new Secret(value.Sha256()) };
            }
        }

        public IdentityClient(IConfiguration configuration, ApplicationType applicationType, 
            IEnumerable<BusinessModel.ApplicationAPIResource> apiClientScopes)
        {
            _applicationType = applicationType;
           
            RefreshTokenUsage = TokenUsage.OneTimeOnly;
            AlwaysIncludeUserClaimsInIdToken = true;
            UpdateAccessTokenClaimsOnRefresh = true;
            RequireConsent = false;
            AllowOfflineAccess = Convert.ToBoolean(configuration["AuthenticationSettings:AllowRefreshToken"]);
            IdentityTokenLifetime = Convert.ToInt32(configuration["AuthenticationSettings:IdentityTokenLifetime"]);
            AccessTokenLifetime = Convert.ToInt32(configuration["AuthenticationSettings:AccessTokenLifetime"]);

            UpdateClientSpecificSettings(_applicationType);
            UpdateClientScopes(apiClientScopes);
        }

        private void UpdateClientSpecificSettings(ApplicationType applicationType)
        {
            switch (applicationType)
            {
                case ApplicationType.Web_MVC:
                    this.AllowedGrantTypes = GrantTypes.Hybrid;
                    break;
                case ApplicationType.Web_JS:
                    this.AllowedGrantTypes = GrantTypes.Implicit;
                    this.AllowAccessTokensViaBrowser = true;
                    break;
                case ApplicationType.NativeApp:
                    this.AllowedGrantTypes = GrantTypes.ResourceOwnerPassword;
                    break;
                default:
                    this.AllowedGrantTypes = GrantTypes.Hybrid;
                    break;
            }
        }

        private void UpdateClientScopes(IEnumerable<BusinessModel.ApplicationAPIResource> clientApiResourse)
        {
            var scopes = new List<string>()
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Address,
                IdentityServerConstants.StandardScopes.OfflineAccess,
                "roles"
            };
            scopes.AddRange(clientApiResourse?.Select(i => i.APIResourceName));
            this.AllowedScopes = scopes;
        }
    }
}
