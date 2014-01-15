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

        public Camera Create(Auth auth)
        {
            HttpResponse<string> response = Unirest.Post(API.CAMERAS_URL)
                .header("accept", "application/json")
                .header("authorization", auth.Basic.Encoded)
                .body<Camera>(this)
                .asJson<string>();

            switch (response.Code)
            {
                case (int)System.Net.HttpStatusCode.BadRequest:
                case (int)System.Net.HttpStatusCode.NotFound:
                    return new Camera();
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Common.MaxJsonLength;
            
            return serializer.Deserialize<Camera>(response.Body);
        }

        public static Camera Get(string cameraId, Auth auth)
        {
            return GetCameras(API.CAMERAS_URL + cameraId, auth).FirstOrDefault<Camera>();
        }

        private static List<Camera> GetCameras(string url, Auth auth)
        {
            HttpResponse<string> response = Unirest.get(url)
                .header("accept", "application/json")
                .header("authorization", "basic " + auth.Basic.Encoded)
                .asString();

            switch (response.Code)
            {
                case (int)System.Net.HttpStatusCode.NotFound:
                    return new List<Camera>();
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Common.MaxJsonLength;
            CamerasList list = serializer.Deserialize<CamerasList>(response.Body);
            return list.cameras;
        }
    }
}
