using ConfigManager;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace M3Pact.WebAPI.Controllers.Admin
{
    [Produces("application/json")]
    [Route("api/PowerBI")]
    public class PowerBIController : Controller
    {
        [HttpGet]
        public AccessToken GetToken()
        {
            var appsettings = new ConfigurationProvider();

            string client_id = appsettings.GetValue("client_id");
            string username = appsettings.GetValue("username");
            string password = appsettings.GetValue("password");


            string apiUrI = "https://login.windows.net/common/oauth2/token";
            string requestBody = "grant_type=password&scope=openid&resource=https://analysis.windows.net/powerbi/api&client_id=" + client_id + "&username=" + username + "&password=" + password;

            //Prepare OAuth request 
            WebRequest webRequest = WebRequest.Create(apiUrI);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(requestBody);
            webRequest.ContentLength = bytes.Length;

            using (Stream outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }

            using (WebResponse webResponse = webRequest.GetResponse())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AccessToken));
                //Get deserialized object from JSON stream
                AccessToken token = (AccessToken)serializer.ReadObject(webResponse.GetResponseStream());
                //token.status = true;
                return token;
            }
        }

        public class AccessToken
        {
            public string token_type;
            public string scope { get; set; }
            public string expires_in { get; set; }
            public string expires_on { get; set; }
            public string not_before { get; set; }
            public string resource { get; set; }
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public string id_token { get; set; }
        }
    }
}