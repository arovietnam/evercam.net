using Newtonsoft.Json;

namespace EvercamV2
{
    public class EvercamClient
    {
        public string ID { get; set; }
        public string Secret { get; set; }
        public string RedirectUri { get; set; }

        public EvercamClient(string id, string secret, string redirectUri) {
            ID = id;
            Secret = secret;
            RedirectUri = redirectUri;
        }
    }

    public class ResponseToken
    {
        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }
        [JsonProperty("token_type", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenType { get; set; }
        [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public int ExpiresIn { get; set; }
    }

    internal class AccessTokenRequest
    {
        [JsonProperty("client_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientID { get; set; }
        [JsonProperty("secret", NullValueHandling = NullValueHandling.Ignore)]
        public string Secret { get; set; }
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
        [JsonProperty("redirect_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string RedirectUri { get; set; }
        [JsonProperty("grant_type", NullValueHandling = NullValueHandling.Ignore)]
        public string GrantType { get; set; }
    }

    internal class RefreshTokenRequest
    {
        [JsonProperty("client_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientID { get; set; }
        [JsonProperty("secret", NullValueHandling = NullValueHandling.Ignore)]
        public string Secret { get; set; }
        [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }
        [JsonProperty("grant_type", NullValueHandling = NullValueHandling.Ignore)]
        public string GrantType { get; set; }
    }
}
