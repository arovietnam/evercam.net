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
    public class User
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("forename")]
        public string ForeName { get; set; }
        [JsonProperty("lastname")]
        public string LastName { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }
        [JsonProperty("confirmed_at")]
        public long ConfirmedAt { get; set; }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <returns>Returns new user details upon success</returns>
        public User Create()
        {
            try
            {
                var request = new RestRequest("users/", Method.POST);
                request.AddParameter("text/json", JsonConvert.SerializeObject(this), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                var response = API.Client.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new Exception(response.Content);
                }

                return JsonConvert.DeserializeObject<User>(response.Content);
            }
            catch (Exception x) { throw new Exception("Error Occured: " + x.Message); }
        }

        /// <summary>
        /// Get all public cameras owned by given user ID
        /// </summary>
        /// <param name="userId">Cameras' Owner ID</param>
        /// <returns>List<Camera></returns>
        public static List<Camera> GetAllCameras(string userId)
        {
            return Camera.GetCameras(API.USERS + userId + "/cameras/");
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
            return Camera.GetCameras(API.USERS + userId + "/cameras/", auth);
        }
    }
}
