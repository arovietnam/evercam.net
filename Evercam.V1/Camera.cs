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

namespace Evercam.V1
{
    public class Camera
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("vendor")]
        public string Vendor { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }

        [JsonProperty("last_polled_at")]
        public long LastPolledAt { get; set; }

        [JsonProperty("last_online_at")]
        public long LastOnlineAt { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("mac_address")]
        public string MacAddress{ get; set; }

        [JsonProperty("endpoints")]
        public List<string> Endpoints { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty("is_online")]
        public bool IsOnline { get; set; }

        [JsonProperty("location")]
        public Location Location;

        [JsonProperty("auth")]
        public Auth Auth;

        [JsonProperty("snapshots")]
        public Snapshots Snapshots;

        public Camera Create(Auth auth, AuthMode mode)
        {
            try
            {
                var request = new RestRequest("cameras.json", Method.POST);
                request.AddParameter("text/json", JsonConvert.SerializeObject(this), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                if (mode == AuthMode.Basic)
                    API.Client.Authenticator = new HttpBasicAuthenticator(auth.Basic.UserName, auth.Basic.Password);
                else if (mode == AuthMode.OAuth2)
                    API.Client.Authenticator = new HttpOAuth2Authenticator(auth.OAuth2.AccessToken);
                
                var response = API.Client.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new Exception(response.Content);
                }

                return JsonConvert.DeserializeObject<Camera>(response.Content);
            }
            catch (Exception x) { throw new Exception("Error Occured: " + x.Message); }
        }

        public Camera Delete(Auth auth, AuthMode mode)
        {
            try
            {
                var request = new RestRequest("cameras/" + ID + ".json", Method.DELETE);

                if (mode == AuthMode.Basic)
                    API.Client.Authenticator = new HttpBasicAuthenticator(auth.Basic.UserName, auth.Basic.Password);
                else if (mode == AuthMode.OAuth2)
                    API.Client.Authenticator = new HttpOAuth2Authenticator(auth.OAuth2.AccessToken);

                var response = API.Client.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new Exception(response.Content);
                }

                return JsonConvert.DeserializeObject<Camera>(response.Content);
            }
            catch (Exception x) { throw new Exception("Error Occured: " + x.Message); }
        }

        public Camera Update(Auth auth, AuthMode mode)
        {
            try
            {
                var request = new RestRequest("cameras/" + ID + ".json", Method.PATCH);
                request.AddParameter("text/json", JsonConvert.SerializeObject(this), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                if (mode == AuthMode.Basic)
                    API.Client.Authenticator = new HttpBasicAuthenticator(auth.Basic.UserName, auth.Basic.Password);
                else if (mode == AuthMode.OAuth2)
                    API.Client.Authenticator = new HttpOAuth2Authenticator(auth.OAuth2.AccessToken);

                var response = API.Client.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new Exception(response.Content);
                }

                return JsonConvert.DeserializeObject<Camera>(response.Content);
            }
            catch (Exception x) { throw new Exception("Error Occured: " + x.Message); }
        }

        public static Camera Get(string cameraId)
        {
            return GetCameras(API.CAMERAS + cameraId, null, AuthMode.None).FirstOrDefault<Camera>();
        }

        public static Camera Get(string cameraId, Auth auth, AuthMode mode)
        {
            return GetCameras(API.CAMERAS + cameraId, auth, mode).FirstOrDefault<Camera>();
        }

        public byte[] GetLiveImage(string streamUrl)
        {
            try
            {
                API.Client.BaseUrl = streamUrl;
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;

                if (Auth.OAuth2 != null && !string.IsNullOrEmpty(Auth.OAuth2.AccessToken))
                    API.Client.Authenticator = new HttpOAuth2Authenticator(Auth.OAuth2.AccessToken);
                if (Auth.Basic != null && !string.IsNullOrEmpty(Auth.Basic.UserName))
                    API.Client.Authenticator = new HttpBasicAuthenticator(Auth.Basic.UserName, Auth.Basic.Password);

                var response = API.Client.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.Unauthorized:
                        throw new Exception(response.Content);
                }
                return response.RawBytes;
            }
            catch (Exception x) { throw new Exception("Error Occured: " + x.Message); }
        }

        public byte[] GetLiveImage()
        {
            try
            {
                API.Client.BaseUrl = Endpoints[0] + Snapshots.Jpg;
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;

                if (Auth.OAuth2 != null && !string.IsNullOrEmpty(Auth.OAuth2.AccessToken))
                    API.Client.Authenticator = new HttpOAuth2Authenticator(Auth.OAuth2.AccessToken);
                if (Auth.Basic != null && !string.IsNullOrEmpty(Auth.Basic.UserName))
                    API.Client.Authenticator = new HttpBasicAuthenticator(Auth.Basic.UserName, Auth.Basic.Password);

                var response = API.Client.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.Unauthorized:
                        throw new Exception(response.Content);
                }
                return response.RawBytes;
            }
            catch (Exception x) { throw new Exception("Error Occured: " + x.Message); }
        }

        //internal static List<Camera> GetCameras(string url)
        //{
        //    try
        //    {
        //        var request = new RestRequest(url, Method.GET);
        //        request.RequestFormat = DataFormat.Json;

        //        var response = API.Client.Execute(request);

        //        switch (response.StatusCode)
        //        {
        //            case HttpStatusCode.NotFound:
        //                throw new Exception(response.Content);
        //        }

        //        return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>();
        //    }
        //    catch (Exception x) { throw new Exception("Error Occured: " + x.Message); }
        //}

        internal static List<Camera> GetCameras(string url, Auth auth, AuthMode mode)
        {
            try
            {
                var request = new RestRequest(url, Method.GET);
                request.RequestFormat = DataFormat.Json;

                if (mode == AuthMode.Basic)
                    API.Client.Authenticator = new HttpBasicAuthenticator(auth.Basic.UserName, auth.Basic.Password);
                else if (mode == AuthMode.OAuth2)
                    API.Client.Authenticator = new HttpOAuth2Authenticator(auth.OAuth2.AccessToken);

                var response = API.Client.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new Exception(response.Content);
                }

                return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>();
            }
            catch (Exception x) { throw new Exception("Error Occured: " + x.Message); }
        }
    }

    public class Location
    {
        [JsonProperty("lat")]
        public string Latitude;

        [JsonProperty("lng")]
        public string Longitude;
    }
}
