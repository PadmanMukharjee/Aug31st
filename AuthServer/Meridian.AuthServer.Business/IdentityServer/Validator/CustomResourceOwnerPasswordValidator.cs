using IdentityModel;
using IdentityServer4.Validation;
using Meridian.AuthServer.BusinessInterfaces;
using System.Threading.Tasks;

namespace Meridian.AuthServer.Business.IdentityServer.Validator
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserLoginBusiness _userBusiness;

        public CustomResourceOwnerPasswordValidator(IUserLoginBusiness userLoginBusiness)
        {
            _userBusiness = userLoginBusiness;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = _userBusiness.ValidateUserCredentials(context.UserName, context.Password, context.Request.Client.ClientName);
            if (user != null)
            {
                context.Result = new GrantValidationResult(user.UserId.ToString(), OidcConstants.AuthenticationMethods.Password);
            }
            return Task.FromResult(0);
        }
    }
}
