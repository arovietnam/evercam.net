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
    public class Camera
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "owner")]
        public string Owner { get; set; }

        [DataMember(Name = "created_at")]
        public long Created_At { get; set; }

        [DataMember(Name = "updated_at")]
        public long Updated_At { get; set; }

        [DataMember(Name = "endpoints")]
        public List<string> Endpoints { get; set; }

        [DataMember(Name = "is_public")]
        public bool Is_Public { get; set; }

        [DataMember(Name = "auth")]
        public Auth Auth;

        [DataMember(Name = "snapshots")]
        public Snapshots Snapshots;

        public static List<Camera> Create(Camera camera)
        {
            HttpResponse<string> response = Unirest.Post(API.CAMERAS_URL)
                .header("accept", "application/json")
                .body<Camera>(camera)
                .asJson<string>();

            switch (response.Code)
            {
                case (int)System.Net.HttpStatusCode.BadRequest:
                    throw new Exception(response.Body);
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Common.MaxJsonLength;
            List<Camera> list = serializer.Deserialize<List<Camera>>(response.Body);
            
            return list;
        }

        public static List<Camera> Get(string cameraId)
        {
            return GetCameras(API.CAMERAS_URL + cameraId);
        }

        private static List<Camera> GetCameras(string url)
        {
            HttpResponse<string> response = Unirest.get(url)
                .header("accept", "application/json")
                .header("Authorization", "c2hha2VlbGFuanVtOmFzZGYxMjM0")
                .asString();

            switch (response.Code)
            {
                case (int)System.Net.HttpStatusCode.NotFound:
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Common.MaxJsonLength;
            CamerasList list = serializer.Deserialize<CamerasList>(response.Body);
            return list.cameras;
        }
    }
}
