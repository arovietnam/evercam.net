using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Threading.Tasks;

using unirest_net;
using unirest_net.http;
using unirest_net.request;

using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace Evercam.V1
{
    [DataContract]
    public class Auth
    {
        [DataMember(Name = "basic")]
        public NetworkCredential Basic { get; set; }

        public static string GetBasic(string username, string password)
        {
            HttpResponse<string> response = Unirest.get("http://api.evercam.io/v1/")
                .header("accept", "application/json")
                .header("Authorization", "c2hha2VlbGFuanVtOmFzZGYxMjM0")
                .asString();

            switch (response.Code)
            {
                case (int)System.Net.HttpStatusCode.NotFound:
                    return "";
            }

            return response.Body.ToString();
        }
    }
}
