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
        public Auth Auth { get; set; }

        public EvercamClient Client { get; set; }

        #region CONSTRUCTORS

        /// <summary>
        /// Initialize Evercam without any user authentication details. 
        /// User can only access public resources from Evercam.
        /// </summary>
        public Evercam() { Auth = new EvercamV1.Auth(); }

        /// <summary>
        /// Initializes Evercam with user's Basic authentication details
        /// </summary>
        /// <param name="username">User Name</param>
        /// <param name="password">User Password</param>
        public Evercam(string username, string password)
        {
            if (Auth == null) Auth = new EvercamV1.Auth();
            Auth.Basic = new Basic(username, password);
            try
            {
                API.SetClientAuth(Auth);
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
            if (Auth == null) Auth = new EvercamV1.Auth();
            Auth.OAuth2 = new OAuth2(accesstoken);
            try
            {
                API.SetClientAuth(Auth);
            }
            catch (TypeInitializationException x)
            {
                throw new EvercamException("File not found. Initialization requires RestSharp.dll to be included in project.", x.InnerException);
            }
        }

        /// <summary>
        /// Initializes Evercam with Client credentials
        /// </summary>
        /// <param name="client"></param>
        public Evercam(EvercamClient client)
        {
            Client = client;
        }

        #endregion


        #region OAuth2 Grant-Code-Flow
        
        /// <summary>
        /// Returns new access token details resulted from the auth. code exchange request to Evercam. 
        /// It also updates Auth values with new access token details. 
        /// Method requires Client details to be set before this call.
        /// </summary>
        /// <param name="authCode">Client request auth. code</param>
        /// <returns>ResponseToken</returns>
        public ResponseToken GetAccessToken(string authCode)
        {
            // checks if application has already set its Client credentials or not
            if (Client == null)
                throw new EvercamException("Client credentials not presented (ID, Secret, Redirect Uri)");

            // prepares access token request
            AccessTokenRequest access = new AccessTokenRequest();
            access.ClientID = Client.ID;
            access.Secret = Client.Secret;
            access.RedirectUri = Client.RedirectUri;
            access.Code = authCode;
            access.GrantType = API.ACCESS_GRANT_TYPE;

            try
            {
                var request = new RestRequest(API.TOKEN, Method.POST);
                request.AddParameter("text/json", JsonConvert.SerializeObject(access), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.BadRequest:
                        throw new EvercamException(response.Content, response.ErrorException);
                }
                
                // receives response token and updates OAuth2 instance values
                ResponseToken token = JObject.Parse(response.Content)["tokens"].ToObject<List<ResponseToken>>().FirstOrDefault<ResponseToken>();
                Auth.OAuth2.AccessToken = token.AccessToken;
                Auth.OAuth2.TokenType = token.TokenType;
                Auth.OAuth2.ExpiresIn = token.ExpiresIn;

                return token;
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns new access token details resulted from the refresh token exchange request to Evercam. 
        /// It also updates Auth values with new access token details. 
        /// Method requires Client details to be set before this call.
        /// </summary>
        /// <param name="refreshToken">Client refresh token</param>
        /// <returns>ResponseToken</returns>
        public ResponseToken GetRefreshToken(string refreshToken)
        {
            // checks if application has already set its Client credentials or not
            if (Client == null)
                throw new EvercamException("Client credentials not presented (ID, Secret, Redirect Uri)");

            // prepares refresh token request
            RefreshTokenRequest access = new RefreshTokenRequest();
            access.ClientID = Client.ID;
            access.Secret = Client.Secret;
            access.RefreshToken = refreshToken;
            access.GrantType = API.REFRESH_TOKEN_TYPE;

            try
            {
                var request = new RestRequest(API.TOKEN, Method.POST);
                request.AddParameter("text/json", JsonConvert.SerializeObject(access), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.BadRequest:
                        throw new EvercamException(response.Content, response.ErrorException);
                }

                // receives response token and updates OAuth2 instance values
                ResponseToken token = JObject.Parse(response.Content)["tokens"].ToObject<List<ResponseToken>>().FirstOrDefault<ResponseToken>();
                Auth.OAuth2.AccessToken = token.AccessToken;
                Auth.OAuth2.TokenType = token.TokenType;
                Auth.OAuth2.ExpiresIn = token.ExpiresIn;

                if (string.IsNullOrEmpty(token.TokenType))
                    token.TokenType = Auth.OAuth2.TokenType;
                
                return token;
            }
            catch (Exception x) { throw new EvercamException(x); }
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
                
                AddClientCredentials(request, true);
                
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.Forbidden:
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
                AddClientCredentials(request, true);
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
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
                AddClientCredentials(request, true);
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
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
        /// Tests if given camera parameters are correct
        /// </summary>
        /// <param name="info">Camera information to test</param>
        /// <returns>Returns Camera Details</returns>
        public Camera TestCamera(CameraTestInfo info)
        {
            try
            {
                var request = new RestRequest(API.CAMERAS_TEST, Method.GET);
                request.AddParameter("text/json", JsonConvert.SerializeObject(info), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                        throw new EvercamException(response.Content, response.ErrorException);
                }

                return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>().FirstOrDefault<Camera>();
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns all data for a given camera
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <returns>Returns Camera Details</returns>
        public Camera GetCamera(string id)
        {
            return GetAllCameras(string.Format(API.CAMERAS_ID, id)).FirstOrDefault<Camera>();
        }

        /// <summary>
        /// Creates a new camera owned by the authenticating user
        /// </summary>
        /// <param name="info">New Camera Information</param>
        /// <returns>Returns new camera details upon success</returns>
        public Camera CreateCamera(CameraInfo info)
        {
            try
            {
                var request = new RestRequest(API.CAMERAS, Method.POST);
                request.AddParameter("text/json", JsonConvert.SerializeObject(info), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                AddClientCredentials(request, true);
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
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
        /// <param name="info">Updated Camera Information</param>
        /// <returns>User Details</returns>
        public Camera UpdateCamera(CameraInfo info)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_ID, info.ID), Method.PATCH);
                request.AddParameter("text/json", JsonConvert.SerializeObject(info), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                AddClientCredentials(request, true);
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
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
                AddClientCredentials(request, true);
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
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
                
                if (Auth != null && Auth.OAuth2 != null && !string.IsNullOrEmpty(Auth.OAuth2.AccessToken))
                    API.Client.Value.Authenticator = new HttpOAuth2Authenticator(Auth.OAuth2.AccessToken, Auth.OAuth2.TokenType);
                else if (Auth != null && Auth.Basic != null && !string.IsNullOrEmpty(Auth.Basic.UserName))
                    API.Client.Value.Authenticator = new HttpBasicAuthenticator(Auth.Basic.UserName, Auth.Basic.Password);

                AddClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.Unauthorized:
                        throw new EvercamException(response.Content);
                }

                return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>();
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Adds client credentials as parameters to given request
        /// </summary>
        /// <param name="request">RestRequest</param>
        /// <param name="forceCredentials">Make sure if request requires client credentials as must, errors if not specified</param>
        private void AddClientCredentials(RestRequest request, bool forceCredentials)
        {
            if (forceCredentials && Client == null)
                throw new EvercamException("Client credentials not presented (ID, Secret, Redirect Uri)");

            if (Client != null) {
                request.Parameters.Add(new Parameter() { Name = "api_id", Value = Client.ID, Type = ParameterType.GetOrPost });
                request.Parameters.Add(new Parameter() { Name = "api_key", Value = Client.Secret, Type = ParameterType.GetOrPost });
            }
        }

        #endregion
    }
}
