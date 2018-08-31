using Microsoft.AspNetCore.Authorization;


namespace M3Pact.WebAPI.Filters
{
    public class AuthorizationFilter : AuthorizeAttribute
    {      
        public AuthorizationFilter()
        {
            
        }
        public AuthorizationFilter(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}
