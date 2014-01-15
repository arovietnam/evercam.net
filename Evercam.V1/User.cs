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
    public class User
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "forename")]
        public string ForeName { get; set; }
        
        [DataMember(Name = "lastname")]
        public string LastName { get; set; }

        [DataMember(Name = "username")]
        public string UserName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "created_at")]
        public long Created_At { get; set; }

        [DataMember(Name = "updated_at")]
        public long Updated_At { get; set; }

        [DataMember(Name = "confirmed_at")]
        public long Confirmed_At { get; set; }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <returns>Returns new user details upon success</returns>
        public User Create()
        {
            HttpResponse<string> response = Unirest.Post(API.USERS_URL)
                .header("accept", "application/json")
                .body<User>(this)
                .asJson<string>();

            switch (response.Code)
            {
                case (int)System.Net.HttpStatusCode.BadRequest:
                case (int)System.Net.HttpStatusCode.NotFound:
                    return new User();
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Common.MaxJsonLength;
            return serializer.Deserialize<User>(response.Body);
        }

        /// <summary>
        /// Get all public cameras owned by given user ID
        /// </summary>
        /// <param name="userId">Cameras' Owner ID</param>
        /// <returns>List<Camera></returns>
        public static List<Camera> GetAllCameras(string userId)
        {
            return GetCameras(API.USERS_URL + userId + "/cameras/");
        }

        /// <summary>
        /// Get all public cameras owned by given user ID and also
        /// cameras which are shared with user represented by 'auth' details
        /// </summary>
        /// <param name="userId">Camera owner ID</param>
        /// <param name="auth">Auth details of user with whom Camera Owner has shared some cameras</param>
        /// <returns>List<Camera></returns>
        public static List<Camera> GetAllCameras(string userId, Auth auth)
        {
            return GetCameras(API.USERS_URL + userId + "/cameras/", auth);
        }

        private static List<Camera> GetCameras(string url)
        {
            HttpResponse<string> response = Unirest.get(url)
                    .header("accept", "application/json")
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

        private static List<Camera> GetCameras(string url, Auth auth)
        {
            HttpResponse<string> response = Unirest.get(url)
                .header("accept", "application/json")
                .header("authorization", auth.Basic.Encoded)
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

    [DataContract]
    class CamerasList
    {
        [DataMember(Name = "cameras")]
        public List<Camera> cameras;
    }
}
