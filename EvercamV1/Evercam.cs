using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EvercamV1
{
    /// <summary>
    /// Evercam API Wrapper [Docs: http://www.evercam.io/docs/api/v1/swagger]
    /// </summary>
    public class Evercam
    {
        private Auth _auth = new Auth();
        public Auth Auth
        {
            get { return _auth; }
            set { _auth = value; }
        }

        #region CONSTRUCTORS

        /// <summary>
        /// Initialize Evercam without any user authentication details. 
        /// User can only access public resources from Evercam.
        /// </summary>
        public Evercam() {  }

        /// <summary>
        /// Initializes Evercam with user's Basic authentication details
        /// </summary>
        /// <param name="username">User Name</param>
        /// <param name="password">User Password</param>
        public Evercam(string username, string password)
        {
            _auth.Basic = new Basic(username, password);
            try
            {
                API.SetClientAuth(_auth);
            }
            catch (TypeInitializationException x)
            {
                throw new EvercamException("File not found. Initialization requires RestSharp.dll to be included in project.", x.InnerException);
            }
        }

        /// <summary>
        /// Initializes Evercam with user's OAuth2.0 authentication details
        /// </summary>
        /// <param name="accesstoken"></param>
        public Evercam(string accesstoken)
        {
            _auth.OAuth2 = new OAuth2(accesstoken);
            try
            {
                API.SetClientAuth(_auth);
            }
            catch (TypeInitializationException x)
            {
                throw new EvercamException("File not found. Initialization requires RestSharp.dll to be included in project.", x.InnerException);
            }
        }

        #endregion


        #region VENDORS

        /// <summary>
        /// Get a list of all camera vendors
        /// </summary>
        /// <returns>List<Vendor></returns>
        public List<Vendor> GetVendors()
        {
            return GetAllVendors(API.VENDORS);
        }

        /// <summary>
        /// Get a list of all camera vendors filtered by given vendor name
        /// </summary>
        /// <param name="name">Vendor Name</param>
        /// <returns>List<Vendor></returns>
        public List<Vendor> GetVendorsByName(string name)
        {
            List<Vendor> allVendors = GetAllVendors(API.VENDORS);
            List<Vendor> list = new List<Vendor>();

            try
            {
                foreach (Vendor v in allVendors)
                {
                    if (v.Name.ToLower().Equals(name.ToLower()))
                        list.Add(v);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }

            return list;
        }

        /// <summary>
        /// Get a list of all camera vendors filtered by given MAC address
        /// </summary>
        /// <param name="mac">MAC Address</param>
        /// <returns>List<Vendor></returns>
        public List<Vendor> GetVendorsByMac(string mac)
        {
            return GetAllVendors(string.Format(API.VENDORS_MAC, mac));
        }

        #endregion

        #region MODELS

        /// <summary>
        /// Get list of all supported camera vendors
        /// </summary>
        /// <returns>List<Vendor></returns>
        public List<Vendor> GetAllVendors()
        {
            return GetAllVendors(API.MODELS);
        }

        /// <summary>
        /// Get list of known camera models for given vendor ID
        /// </summary>
        /// <param name="id">Vendor ID</param>
        /// <returns>List<Vendor></returns>
        public List<Vendor> GetVendorsById(string vendorId)
        {
            return GetAllVendors(string.Format(API.MODELS_VENDOR, vendorId));
        }

        /// <summary>
        /// Get details of a particular camera model with given vendor ID and model ID
        /// </summary>
        /// <param name="vendorId">Vendor ID</param>
        /// <param name="modelId">Model ID</param>
        /// <returns>Model</returns>
        public Model GetModel(string vendorId, string modelId)
        {
            try
            {
                var request = new RestRequest(string.Format(API.MODELS_VENDOR_MODEL, vendorId, modelId), Method.GET);
                request.RequestFormat = DataFormat.Json;
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new EvercamException(response.Content, response.ErrorException);
                }

                return JObject.Parse(response.Content)["models"].ToObject<List<Model>>().FirstOrDefault<Model>();
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        #endregion

        #region USERS

        /// <summary>
        /// Returns the set of cameras owned by a particular user
        /// If user has provided NO authentication details, then ONLY public cameras will be returned. 
        /// OAuth2 is default authentication scheme, alternatively Basic authentication is used.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of Camera objects</returns>
        public List<Camera> GetCameras(string userId)
        {
            return GetAllCameras(string.Format(API.USERS_CAMERA, userId));
        }

        /// <summary>
        /// Returns available information for the user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details</returns>
        public User GetUser(string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.USERS_ID, id), Method.GET);
                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.Unauthorized:
                        throw new EvercamException(response.Content, response.ErrorException);
                }

                return JObject.Parse(response.Content)["users"].ToObject<List<User>>().FirstOrDefault<User>();
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Starts the new user signup process
        /// </summary>
        /// <returns>Returns new user details upon success</returns>
        public User CreateUser(User user)
        {
            try
            {
                var request = new RestRequest(API.USERS, Method.POST);
                request.AddParameter("text/json", JsonConvert.SerializeObject(user), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new EvercamException(response.Content, response.ErrorException);
                }

                return JObject.Parse(response.Content)["users"].ToObject<List<User>>().FirstOrDefault<User>();
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Delete your account, any cameras you own and all stored media
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details</returns>
        public string DeleteUser(string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.USERS_ID, id), Method.DELETE);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                        throw new EvercamException(response.Content, response.ErrorException);
                }

                return response.Content;
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Updates full or partial data on your existing user account
        /// </summary>
        /// <param name="user">User Details</param>
        /// <returns>User Details</returns>
        public User UpdateUser(User user)
        {
            try
            {
                var request = new RestRequest(string.Format(API.USERS_ID, user.ID), Method.PATCH);
                request.AddParameter("text/json", JsonConvert.SerializeObject(user), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                        throw new EvercamException(response.Content, response.ErrorException);
                }

                return JObject.Parse(response.Content)["users"].ToObject<List<User>>().FirstOrDefault<User>();
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns the set of camera and other rights you have granted and have been granted (COMING SOON)
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>User Rights information</returns>
        public string GetUserRights(string userId)
        {
            return "TO-DO";
        }

        #endregion

        #region CAMERAS

        /// <summary>
        /// Returns all data for a given camera
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <returns>Camera Details</returns>
        public Camera GetCamera(string id)
        {
            return GetAllCameras(string.Format(API.CAMERAS_ID, id)).FirstOrDefault<Camera>();
        }

        /// <summary>
        /// Creates a new camera owned by the authenticating user
        /// </summary>
        /// <returns>Returns new camera details upon success</returns>
        public Camera CreateCamera(Camera camera)
        {
            try
            {
                var request = new RestRequest(API.CAMERAS, Method.POST);
                request.AddParameter("text/json", JsonConvert.SerializeObject(camera), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                        throw new EvercamException(response.Content, response.ErrorException);
                }

                return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>().FirstOrDefault<Camera>();
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Updates full or partial data for an existing camera
        /// </summary>
        /// <param name="camera">Camera Details</param>
        /// <returns>User Details</returns>
        public Camera UpdateCamera(Camera camera)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_ID, camera.ID), Method.PATCH);
                request.AddParameter("text/json", JsonConvert.SerializeObject(camera), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                        throw new EvercamException(response.Content, response.ErrorException);
                }

                return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>().FirstOrDefault<Camera>();
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Deletes a camera from Evercam along with any stored media
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <returns>User details</returns>
        public string DeleteCamera(string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_ID, id), Method.DELETE);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                        throw new EvercamException(response.Content, response.ErrorException);
                }

                return response.Content;
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns the list of all snapshots currently stored for this camera (COMING SOON)
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <returns></returns>
        public string GetSnapshots(string id)
        {
            return "TO-DO";
        }

        /// <summary>
        /// Returns the snapshot stored for this camera closest to the given timestamp (COMING SOON)
        /// </summary>
        /// <param name="cameraId">Camera ID</param>
        /// <param name="timestamp">Timestamp</param>
        /// <returns></returns>
        public string GetSnapshots(string cameraId, string timestamp)
        {
            return "TO-DO";
        }

        /// <summary>
        /// Fetches a snapshot from the camera and stores it using the current timestamp (COMING SOON)
        /// </summary>
        /// <param name="cameraId">Camera ID</param>
        /// <returns></returns>
        public string CreateSnapshot(string cameraId)
        {
            return "TO-DO";
        }

        /// <summary>
        /// Stores the supplied snapshot image data for the given timestamp (COMING SOON)
        /// </summary>
        /// <param name="cameraId">Camera ID</param>
        /// <param name="timestamp">Timestamp</param>
        /// <returns></returns>
        public string UpdateSnapshot(string cameraId, string timestamp)
        {
            return "TO-DO";
        }

        /// <summary>
        /// Deletes any snapshot for this camera which exactly matches the timestamp (COMING SOON)
        /// </summary>
        /// <param name="cameraId">Camera ID</param>
        /// <param name="timestamp">Timestamp</param>
        /// <returns></returns>
        public string DeleteSnapshot(string cameraId, string timestamp)
        {
            return "TO-DO";
        }

        #endregion


        #region PRIVATE FUNCTIONS

        /// <summary>
        /// PRIVATE: Get list of all vendors
        /// </summary>
        /// <param name="url">API Endpoint</param>
        /// <returns>List of Vendors</returns>
        private List<Vendor> GetAllVendors(string url)
        {
            try
            {
                var request = new RestRequest(url, Method.GET);
                request.RequestFormat = DataFormat.Json;
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new EvercamException(response.Content);
                }

                return JObject.Parse(response.Content)["vendors"].ToObject<List<Vendor>>();
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// PRIVATE: Get list of all cameras
        /// </summary>
        /// <param name="url">API Endpoint</param>
        /// <returns>List of Cameras</returns>
        private List<Camera> GetAllCameras(string url)
        {
            try
            {
                var request = new RestRequest(url, Method.GET);
                request.RequestFormat = DataFormat.Json;

                if (_auth != null && _auth.OAuth2 != null && !string.IsNullOrEmpty(_auth.OAuth2.AccessToken))
                    API.Client.Value.Authenticator = new HttpOAuth2Authenticator(_auth.OAuth2.AccessToken, _auth.OAuth2.TokenType);
                else if (_auth != null && _auth.Basic != null && !string.IsNullOrEmpty(_auth.Basic.UserName))
                    API.Client.Value.Authenticator = new HttpBasicAuthenticator(_auth.Basic.UserName, _auth.Basic.Password);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.Unauthorized:
                        throw new EvercamException(response.Content);
                }

                return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>();
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        #endregion
    }
}
