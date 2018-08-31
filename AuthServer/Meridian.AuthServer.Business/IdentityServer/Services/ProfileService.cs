using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Meridian.AuthServer.Business.IdentityServer.Models;
using Meridian.AuthServer.BusinessModel;
using Meridian.AuthServer.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Meridian.AuthServer.Business.IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserLoginRepository _userRepository;

        public ProfileService(IUserLoginRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                if (!string.IsNullOrEmpty(context.Subject.Identity.Name))
                {
                    var user = _userRepository.GetUserByEmail(context.Subject.Identity.Name);
                    UpdateClaims(user, context);
                }
                else
                {
                    //subject was set to my user id.
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                    if (!string.IsNullOrEmpty(userId?.Value))
                    {
                        var user = _userRepository.GetUserById(userId.Value);
                        UpdateClaims(user, context);
                    }
                }
            }
            catch (Exception ex)
            {
                //log your error
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                if (!string.IsNullOrEmpty(userId?.Value))
                {
                    UserLogin user = _userRepository.GetUserById(userId.Value);
                    if (user != null)
                    {
                        if (user.RecordStatus == "A")
                        {
                            context.IsActive = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //handle error logging
            }
        }

        private void UpdateClaims(UserLogin user, ProfileDataRequestContext context)
        {
            if (user != null)
            {
                context.IssuedClaims = GetUserClaims(user, context.RequestedClaimTypes);
            }
        }

        private List<Claim> GetUserClaims(UserLogin user, IEnumerable<string> requestedClaimTypes)
        {
            var claims = new List<Claim>()
            {
                new Claim(AuthConstant.ClaimUserID, user.UserId.ToString() ?? string.Empty),
                new Claim(JwtClaimTypes.Name, (!string.IsNullOrEmpty(user.FirstName)
                && !string.IsNullOrEmpty(user.LastName)) ? (user.FirstName + " " + user.LastName) : string.Empty),
                new Claim(JwtClaimTypes.GivenName, user.FirstName  ?? string.Empty),
                new Claim(JwtClaimTypes.FamilyName, user.LastName  ?? string.Empty),
                new Claim(JwtClaimTypes.Email, user.Email  ?? string.Empty),
                //new Claim("ClientId", Convert.ToString(user.ClientRowID)),
                //new Claim("TechnicianId", user.TechnicianNo ?? string.Empty),
            };

            //roles
            claims.Add(new Claim(JwtClaimTypes.Role, user.RoleName));   
            return claims;
        }
    }
}
