using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Threading.Tasks;

using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Evercam.V1
{
    public class Auth
    {
        [JsonProperty("basic")]
        public Basic Basic { get; set; }

        public Auth()
        {
        }

        public Auth(string username, string password)
        {
            Basic = new Basic(username, password);
        }
    }


    public class Basic
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public Basic()
        {
        }

        public Basic(string username, string password)
        {
            UserName = username;
            Password = password;
        }
    }
}
