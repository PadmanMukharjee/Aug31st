using Microsoft.AspNetCore.Http;
using System.Linq;

namespace M3Pact.Business.Common
{
    public class Helper
    {
        private IHttpContextAccessor _httpContextAccessor;
        public Helper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserRole()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        }

        public string GetUserID()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value;
        }
    }
}
