namespace Meridian.AuthServer.Business.IdentityServer.Models
{
    public enum ApplicationType
    {
        Web_MVC = 1,
        Web_JS,
        NativeApp
    }

    public class AuthConstant
    {
        public const string Role = "role";
        public const string Roles = "roles";
        public const string SignIn_OIDC = "/signin-oidc";
        public const string ClaimUserID = "user_id";
    }
}
