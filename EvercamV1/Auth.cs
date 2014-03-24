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

namespace EvercamV1
{
    public class Auth
    {
        [JsonProperty("basic", NullValueHandling = NullValueHandling.Ignore)]
        public Basic Basic { get; set; }

        [JsonProperty("oauth", NullValueHandling = NullValueHandling.Ignore)]
        public OAuth2 OAuth2 { get; set; }

        public Auth()
        {
        }

        public Auth(Basic basic)
        {
            Basic = basic;
        }

        public Auth(OAuth2 oauth2)
        {
            OAuth2 = oauth2;
        }
    }


    public class Basic
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
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

    public class OAuth2
    {
        [JsonProperty("token_type", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }
        [JsonProperty("token_type", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenType { get; set; }
        [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public int ExpiresIn { get; set; }
        [JsonProperty("expires_at", NullValueHandling = NullValueHandling.Ignore)]
        public long ExpiresAt { get; set; }

        public OAuth2()
        {
        }

        public OAuth2(string accesstoken)
        {
            AccessToken = accesstoken;
            TokenType = "bearer";
            ExpiresIn = 3599;
            ExpiresAt = Utils.ToUnixTimestamp(DateTime.Now.AddSeconds(ExpiresIn));
        }

        public OAuth2(string accesstoken, string tokentype)
        {
            AccessToken = accesstoken;
            TokenType = tokentype;
            ExpiresIn = 3599;
            ExpiresAt = Utils.ToUnixTimestamp(DateTime.Now.AddSeconds(ExpiresIn));
        }

        public OAuth2(string accesstoken, string tokentype, int expiresin)
        {
            AccessToken = accesstoken;
            TokenType = tokentype;
            ExpiresIn = expiresin;
            ExpiresAt = Utils.ToUnixTimestamp(DateTime.Now.AddSeconds(ExpiresIn));
        }
    }
}
