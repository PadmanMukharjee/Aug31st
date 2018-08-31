using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.LoggerUtility;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace M3Pact.WebAPI.Diagnostics
{
    public class IdentityMiddleware
    {
        readonly RequestDelegate _next;

        public IdentityMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// To Create the clientContext from clams at the time of request
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    UserHelper.Instance(CreateUserContext(httpContext));
                }
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ex = ex.InnerException;
                PreserveStackTrace(ex);
                await ErrorLoggingResponse.HttpExceptionMessage(httpContext, ex);
            }
        }
        static void PreserveStackTrace(Exception e)
        {
            var ctx = new StreamingContext(StreamingContextStates.CrossAppDomain);
            var si = new SerializationInfo(typeof(Exception), new FormatterConverter());
            var ctor = typeof(Exception).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);

            e.GetObjectData(si, ctx);
            ctor.Invoke(e, new object[] { si, ctx });
        }

        /// <summary>
        /// To create Usercontext from the clames
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public UserContext CreateUserContext(HttpContext httpContext)
        {
            UserContext user = new UserContext();

            user.Email = httpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.Email)?.Value;
            user.FirstName = httpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.GivenName)?.Value;
            user.LastName = httpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.FamilyName)?.Value;
            user.Role = httpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.Role)?.Value;
            user.UserId = httpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.UserId)?.Value;
            user.FullName = httpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.Name)?.Value;

            return user;
        }
    }
}
