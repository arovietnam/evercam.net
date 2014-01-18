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

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }

        [JsonProperty("endpoints")]
        public List<string> Endpoints { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty("auth")]
        public Auth Auth;

        [JsonProperty("snapshots")]
        public Snapshots Snapshots;

        public Camera Create(Auth auth)
        {
            try
            {
                var request = new RestRequest("cameras/", Method.POST);
                request.AddParameter("text/json", JsonConvert.SerializeObject(this), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                API.Client.Authenticator = new HttpBasicAuthenticator(auth.Basic.UserName, auth.Basic.Password);
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

        public static Camera Get(string cameraId, Auth auth)
        {
            return GetCameras(API.CAMERAS + cameraId, auth).FirstOrDefault<Camera>();
        }

        public MemoryStream GetLiveImage(string streamUrl, bool useAuth)
        {
            try
            {
                API.Client.BaseUrl = streamUrl;
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;

                if (useAuth)
                    API.Client.Authenticator = new HttpBasicAuthenticator(Auth.Basic.UserName, Auth.Basic.Password);

                var response = API.Client.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.Unauthorized:
                        throw new Exception(response.Content);
                }
                return new MemoryStream(response.RawBytes);
            }
            catch (Exception x) { throw new Exception("Error Occured: " + x.Message); }
        }

        internal static List<Camera> GetCameras(string url)
        {
            try
            {
                var request = new RestRequest(url, Method.GET);
                request.RequestFormat = DataFormat.Json;

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

        internal static List<Camera> GetCameras(string url, Auth auth)
        {
            try
            {
                var request = new RestRequest(url, Method.GET);
                request.RequestFormat = DataFormat.Json;

                API.Client.Authenticator = new HttpBasicAuthenticator(auth.Basic.UserName, auth.Basic.Password);
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
}
