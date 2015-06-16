using Newtonsoft.Json;

namespace EvercamV2
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

    public class AuthResult
    {
        [JsonProperty("authenticated", NullValueHandling = NullValueHandling.Ignore)]
        public bool Authenticated;
        [JsonProperty("source_ip", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceIP;
        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public string Timestamp;
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

        public OAuth2()
        {
        }

        public OAuth2(string accesstoken)
        {
            AccessToken = accesstoken;
            TokenType = "bearer";
        }

        public OAuth2(string accesstoken, string tokentype)
        {
            AccessToken = accesstoken;
            TokenType = tokentype;
        }
    }
}
