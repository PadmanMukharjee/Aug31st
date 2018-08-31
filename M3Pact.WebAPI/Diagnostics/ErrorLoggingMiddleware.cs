using M3Pact.LoggerUtility;
using Microsoft.AspNetCore.Http;
using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace M3Pact.WebAPI.Diagnostics
{
    public class ErrorLoggingMiddleware
    {
        readonly RequestDelegate _next;


        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            try
            {
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

    }
}
