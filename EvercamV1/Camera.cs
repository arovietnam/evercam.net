using System;
using System.IO;
using System.Drawing;
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
    public class Camera
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("owner", NullValueHandling = NullValueHandling.Ignore)]
        public string Owner { get; set; }

        [JsonProperty("vendor", NullValueHandling = NullValueHandling.Ignore)]
        public string Vendor { get; set; }

        [JsonProperty("model", NullValueHandling = NullValueHandling.Ignore)]
        public string Model { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public long CreatedAt { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public long UpdatedAt { get; set; }

        [JsonProperty("last_polled_at", NullValueHandling = NullValueHandling.Ignore)]
        public long LastPolledAt { get; set; }

        [JsonProperty("last_online_at", NullValueHandling = NullValueHandling.Ignore)]
        public long LastOnlineAt { get; set; }

        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }

        [JsonProperty("is_online", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsOnline { get; set; }
        
        [JsonProperty("is_public", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPublic { get; set; }

        [JsonProperty("external_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalUrl { get; set; }

        [JsonProperty("internal_url", NullValueHandling = NullValueHandling.Ignore)]
        public string InternalUrl { get; set; }
        
        [JsonProperty("jpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string JpegUrl{ get; set; }

        [JsonProperty("cam_username", NullValueHandling = NullValueHandling.Ignore)]
        public string CameraUsername { get; set; }

        [JsonProperty("cam_password", NullValueHandling = NullValueHandling.Ignore)]
        public string CameraPassword { get; set; }

        [JsonProperty("mac_address", NullValueHandling = NullValueHandling.Ignore)]
        public string MacAddress { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public Location Location;

        [JsonProperty("auth", NullValueHandling = NullValueHandling.Ignore)]
        public Auth Auth;

        

        public byte[] GetLiveImage(string streamUrl)
        {
            string baseUrl = API.Client.Value.BaseUrl;
            IAuthenticator apiAuth = API.Client.Value.Authenticator;
            try
            {
                API.Client.Value.BaseUrl = streamUrl;
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;

                if (Auth != null && Auth.OAuth2 != null && !string.IsNullOrEmpty(Auth.OAuth2.AccessToken))
                    API.Client.Value.Authenticator = new HttpOAuth2Authenticator(Auth.OAuth2.AccessToken);
                if (Auth != null && Auth.Basic != null && !string.IsNullOrEmpty(Auth.Basic.UserName))
                    API.Client.Value.Authenticator = new HttpBasicAuthenticator(Auth.Basic.UserName, Auth.Basic.Password);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.Unauthorized:
                        throw new EvercamException(response.Content, response.ErrorException);
                }
                return response.RawBytes;
            }
            catch (EvercamException x) { throw; }
            catch (Exception x) { throw new EvercamException(x); }
            finally { API.Client.Value.BaseUrl = baseUrl; API.Client.Value.Authenticator = apiAuth; }
        }

        public byte[] GetLiveImage()
        {
            string baseUrl = API.Client.Value.BaseUrl;
            IAuthenticator apiAuth = API.Client.Value.Authenticator;
            try
            {
                API.Client.Value.BaseUrl = ExternalUrl + JpegUrl;
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;

                if (Auth != null && Auth.OAuth2 != null && !string.IsNullOrEmpty(Auth.OAuth2.AccessToken))
                    API.Client.Value.Authenticator = new HttpOAuth2Authenticator(Auth.OAuth2.AccessToken);
                if (Auth != null && Auth.Basic != null && !string.IsNullOrEmpty(Auth.Basic.UserName))
                    API.Client.Value.Authenticator = new HttpBasicAuthenticator(Auth.Basic.UserName, Auth.Basic.Password);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.Unauthorized:
                        throw new EvercamException(response.Content, response.ErrorException);
                }
                return response.RawBytes;
            }
            catch (EvercamException x) { throw; }
            catch (Exception x) { throw new EvercamException(x); }
            finally { API.Client.Value.BaseUrl = baseUrl; API.Client.Value.Authenticator = apiAuth; }
        }
    }

    public class CameraInfo
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty("external_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalUrl { get; set; }

        [JsonProperty("internal_url", NullValueHandling = NullValueHandling.Ignore)]
        public string InternalUrl { get; set; }

        [JsonProperty("jpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string JpgUrl { get; set; }

        [JsonProperty("cam_username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("cam_password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }
    }

    public class CameraTestInfo
    {
        [JsonProperty("external_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalUrl { get; set; }

        [JsonProperty("jpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string JpgUrl { get; set; }

        [JsonProperty("cam_username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("cam_password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }
    }

    public class CameraShare
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("camera_id")]
        public string CameraID { get; set; }

        [JsonProperty("external_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalUrl { get; set; }

        [JsonProperty("jpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string JpgUrl { get; set; }

        [JsonProperty("cam_username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("cam_password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }
    }
    
    public class Location
    {
        [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        public double Latitude;

        [JsonProperty("lng", NullValueHandling = NullValueHandling.Ignore)]
        public double Longitude;
    }
}
