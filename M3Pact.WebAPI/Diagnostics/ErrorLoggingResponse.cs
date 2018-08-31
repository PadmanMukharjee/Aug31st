using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace M3Pact.WebAPI.Diagnostics
{
    public class ErrorLoggingResponse
    {
        public static Task HttpExceptionMessage(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = 500;
            var errorResult = Newtonsoft.Json.JsonConvert.SerializeObject(new { Message = "OOPs " }, Newtonsoft.Json.Formatting.Indented);
            return httpContext.Response.WriteAsync(errorResult);
        }
    }
}
