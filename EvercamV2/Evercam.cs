using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EvercamV2
{
    /// <summary>
    /// Evercam API Wrapper [Docs: http://www.evercam.io/docs/api/v1/swagger]
    /// </summary>
    public class Evercam
    {
        public Auth Auth { get; set; }

        public EvercamClient Client { get; set; }

        public static bool SANDBOX { get; set; }

        #region CONSTRUCTORS

        /// <summary>
        /// Initialize Evercam without any user authentication details. 
        /// User can only access public resources from Evercam.
        /// </summary>
        public Evercam() {
            API.Client.Value.BaseUrl = SANDBOX ? API.TEST_URL : API.LIVE_URL;
            Auth = new EvercamV2.Auth(); Client = new EvercamClient("", "", ""); 
        }

        /// <summary>
        /// Initializes Evercam with user's OAuth2.0 authentication details
        /// </summary>
        /// <param name="accesstoken"></param>
        public Evercam(string access_token)
        {
            API.Client.Value.BaseUrl = SANDBOX ? API.TEST_URL : API.LIVE_URL;

            if (Auth == null) Auth = new EvercamV2.Auth();
            Auth.OAuth2 = new OAuth2(access_token);
            try
            {
                API.SetClientAuth(Auth);
            }
            catch (TypeInitializationException x)
            {
                throw new EvercamException("File not found. Initialization requires RestSharp.dll to be included in project.", x.InnerException);
            }
            Client = new EvercamClient("", "", "");
        }

        /// <summary>
        /// Initializes Evercam with Client credentials
        /// </summary>
        /// <param name="api_id">Evercan API ID for a client/user</param>
        /// <param name="api_key">Evercan API Key/Secret of a client/user</param>
        public Evercam(string api_id, string api_key)
        {
            API.Client.Value.BaseUrl = SANDBOX ? API.TEST_URL : API.LIVE_URL;

            Auth = new EvercamV2.Auth();
            Client = new EvercamClient(api_id, api_key, "");
        }

        /// <summary>
        /// Initializes Evercam with Client credentials
        /// </summary>
        /// <param name="api_id">Evercan API ID for a client/user</param>
        /// <param name="api_key">Evercan API Key/Secret of a client/user</param>
        /// <param name="redirect_uri">Client's registered URI at Evercam</param>
        public Evercam(string api_id, string api_key, string redirect_uri)
        {
            API.Client.Value.BaseUrl = SANDBOX ? API.TEST_URL : API.LIVE_URL;

            Auth = new EvercamV2.Auth();
            Client = new EvercamClient(api_id, api_key, redirect_uri);
        }

        #endregion

        #region AUTH

        /// <summary>
        /// A simple endpoint that can be used to test whether an API id and key pair are valid
        /// </summary>
        /// <param name="api_id">The API id to be tested</param>
        /// <param name="api_key">The API key to be tested</param>
        public AuthResult TestAuth(string api_id, string api_key)
        {
            try
            {
                var request = new RestRequest(string.Format(API.AUTH_TEST, api_id, api_key), Method.GET);
                request.RequestFormat = DataFormat.Json;

                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.Continue:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content).ToObject<AuthResult>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        #endregion

        #region VENDORS

        /// <summary>
        /// Returns all known IP hardware vendors
        /// </summary>
        /// <param name="name">Name of the vendor (partial search)</param>
        /// <param name="mac">Mac address of camera</param>
        /// <returns>List<Vendor></returns>
        public List<Vendor> GetVendors(string name, string mac)
        {
            try
            {
                var request = new RestRequest(API.VENDORS, Method.GET);

                if (!string.IsNullOrEmpty(name)) request.AddParameter("name", name, ParameterType.GetOrPost);
                if (!string.IsNullOrEmpty(mac)) request.AddParameter("mac", mac, ParameterType.GetOrPost);
                
                SetClientCredentials(request, true);

                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["vendors"].ToObject<List<Vendor>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns available information for the specified vendor
        /// </summary>
        /// <param name="id">Unique identifier for the vendor</param>
        /// <returns>Model</returns>
        public Vendor GetVendor(string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.VENDORS_ID, id), Method.GET);

                SetClientCredentials(request, false);

                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["vendors"].ToObject<List<Vendor>>().FirstOrDefault<Vendor>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        #endregion

        #region MODELS

        /// <summary>
        /// Returns set of known models for a supported camera vendor
        /// </summary>
        /// <param name="id">Unique identifier for the model</param>
        /// <param name="name">Name of the model (partial search)</param>
        /// <param name="vendor_id">Unique identifier for the vendor</param>
        /// <param name="limit">Number of results per page. Defaults to 25.</param>
        /// <param name="page">Page number, starting from 0</param>
        /// <returns>List<Model></returns>
        public List<Model> GetModels(string id, string name, string vendor_id, int? limit, int? page)
        {
            try
            {
                var request = new RestRequest(API.MODELS, Method.GET);

                if (!string.IsNullOrEmpty(id)) request.AddParameter("id", id, ParameterType.GetOrPost);
                if (!string.IsNullOrEmpty(name)) request.AddParameter("name", name, ParameterType.GetOrPost);
                if (!string.IsNullOrEmpty(vendor_id)) request.AddParameter("vendor_id", vendor_id, ParameterType.GetOrPost);
                if (limit.HasValue && limit.Value <= 0) request.AddParameter("limit", "10", ParameterType.GetOrPost);
                else if (limit.HasValue) request.AddParameter("limit", limit.Value, ParameterType.GetOrPost);
                if (page.HasValue && page.Value <= 0) request.AddParameter("page", "0", ParameterType.GetOrPost);
                else if (page.HasValue) request.AddParameter("page", page.Value, ParameterType.GetOrPost);

                SetClientCredentials(request, true);

                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["models"].ToObject<List<Model>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns available information for the specified model
        /// </summary>
        /// <param name="id">Unique identifier for the model</param>
        /// <returns>Model</returns>
        public Model GetModel(string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.MODELS_ID, id), Method.GET);

                SetClientCredentials(request, false);

                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["models"].ToObject<List<Model>>().FirstOrDefault<Model>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        #endregion

        #region USERS

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
                
                SetClientCredentials(request, true);
                
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["users"].ToObject<List<User>>().FirstOrDefault<User>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Starts the new user signup process
        /// </summary>
        /// <param name="user">User Details</param>
        /// <param name="share_request_key">The key for a camera share request to be processed during the sign up</param>
        /// <returns>Returns new user details upon success</returns>
        public User CreateUser(User user, string share_request_key)
        {
            try
            {
                var request = new RestRequest(API.USERS, Method.POST);
                request.AddParameter("text/json", JsonConvert.SerializeObject(user), ParameterType.RequestBody);
                
                if (!string.IsNullOrEmpty(share_request_key)) request.AddParameter("share_request_key", share_request_key, ParameterType.RequestBody);
                
                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["users"].ToObject<List<User>>().FirstOrDefault<User>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Delete your account, any cameras you own and all stored media
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details</returns>
        public User DeleteUser(string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.USERS_ID, id), Method.DELETE);
                SetClientCredentials(request, true);
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["users"].ToObject<List<User>>().FirstOrDefault<User>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Updates full or partial data on your existing user account
        /// </summary>
        /// <param name="user">User Details</param>
        /// <returns>User Details</returns>
        public User UpdateUser(UserInfo user)
        {
            try
            {
                var request = new RestRequest(string.Format(API.USERS_ID, user.ID), Method.PATCH);
                request.AddParameter("text/json", JsonConvert.SerializeObject(user), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                SetClientCredentials(request, true);
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["users"].ToObject<List<User>>().FirstOrDefault<User>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Fetch API credentials for an authenticated user
        /// </summary>
        /// <param name="id">Evercam user's Login ID</param>
        /// <param name="password">Evercam user's Password</param>
        /// <returns></returns>
        public UserCredentials GetUserCredentials(string id, string password)
        {
            try
            {
                var request = new RestRequest(string.Format(API.USERS_CREDENTIALS, id), Method.GET);
                if (!string.IsNullOrEmpty(password)) request.AddParameter("password", password, ParameterType.GetOrPost);
                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content).ToObject<UserCredentials>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
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
                request.AddParameter("text/json", JsonConvert.SerializeObject(info), ParameterType.GetOrPost);
                request.RequestFormat = DataFormat.Json;

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Accepted:
                        return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>().FirstOrDefault<Camera>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns base64 encoded jpg from the camera
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <returns>Returns live image data</returns>
        public byte[] GetLiveImage(string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_LIVE, id), Method.GET);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return response.RawBytes;
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        ///// <summary>
        ///// Returns base64 encoded string of jpg from camera's proxy url
        ///// </summary>
        ///// <param name="id">Camera ID</param>
        ///// <returns>Returns live image as base64 string</returns>
        //public byte[] GetProxyImage(string id)
        //{
        //    try
        //    {
        //        return DownloadImage(API.PROXY_URL + id + ".jpg");
        //    }
        //    catch (Exception x) { throw new EvercamException(x); }
        //}

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
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("text/json", JsonConvert.SerializeObject(info), ParameterType.RequestBody);

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);
                
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Accepted:
                        return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>().FirstOrDefault<Camera>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Transfers the ownership of a camera from one user to another
        /// </summary>
        /// <param name="info">Updated Camera Information</param>
        /// <param name="user_id">The Evercam user name or email address for the new camera owner</param>
        /// <returns>User Details</returns>
        public Camera TransferCamera(CameraInfo info, string user_id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_ID, info.ID), Method.PUT);
                if (!string.IsNullOrEmpty(user_id)) request.AddParameter("user_id", user_id);
                request.AddParameter("text/json", JsonConvert.SerializeObject(info), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Found:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Accepted:
                        return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>().FirstOrDefault<Camera>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Updates full or partial data for an existing camera
        /// </summary>
        /// <param name="info">Updated Camera Information</param>
        /// <returns>Camera Details</returns>
        public Camera UpdateCamera(CameraInfo info)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_ID, info.ID), Method.PATCH);
                if (info.IsDiscoverable != null)
                    request.AddParameter("discoverable", info.IsDiscoverable.ToString().ToLower(), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, true);
                
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Found:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Accepted:
                        return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>().FirstOrDefault<Camera>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Deletes a camera from Evercam along with any stored media
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <returns>User details</returns>
        public Camera DeleteCamera(string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_ID, id), Method.DELETE);
                SetClientCredentials(request, true);
                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>().FirstOrDefault<Camera>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns data for a specified set of cameras. The ultimate intention would be to expand this functionality to be a more general search. The current implementation is as a basic absolute match list capability.
        /// </summary>
        /// <param name="ids">Comma separated list of camera identifiers for the cameras being queried</param>
        /// <param name="user_id">The Evercam user name or email address for the new camera owner</param>
        /// <param name="include_shared">Set to true to include cameras shared with the user in the fetch</param>
        /// <returns>List of Camera objects</returns>
        public List<Camera> GetCameras(string ids, string user_id, bool? exclude_shared)
        {
            try
            {
                var request = new RestRequest(API.CAMERAS, Method.GET);
                if (!string.IsNullOrEmpty(ids)) request.AddParameter("ids", ids);
                if (!string.IsNullOrEmpty(user_id)) request.AddParameter("user_id", user_id);
                if (exclude_shared.HasValue) request.AddParameter("exclude_shared", exclude_shared.Value);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns list of log messages for given camera
        /// </summary>
        /// <param name="id">Unique identifier for the camera</param>
        /// <param name="from">From Unix timestamp</param>
        /// <param name="to">To Unix timestamp</param>
        /// <param name="limit">Number of results per page. Defaults to 50</param>
        /// <param name="page">Page number, starting from 0</param>
        /// <param name="types">Comma separated list of log types: created, accessed, edited, viewed, captured</param>
        /// <returns>List of Camera Logs as messages</returns>
        public LogMessages GetLogMessages(string id, int? from, int? to, int? limit, int? page, string types)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_LOGS, id), Method.GET);
                if (from.HasValue && from.Value > 0) request.AddParameter("from", from.Value);
                if (to.HasValue && to.Value > 0) request.AddParameter("to", to.Value);
                if (limit.HasValue && limit.Value > 0) request.AddParameter("limit", limit.Value);
                if (page.HasValue && page.Value > -1) request.AddParameter("page", page.Value);
                if (!string.IsNullOrEmpty(types)) request.AddParameter("types", types);

                request.AddParameter("objects", false);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JsonConvert.DeserializeObject<LogMessages>(response.Content);
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns list of log objects for given camera
        /// </summary>
        /// <param name="id">Unique identifier for the camera</param>
        /// <param name="from">From Unix timestamp</param>
        /// <param name="to">To Unix timestamp</param>
        /// <param name="limit">Number of results per page. Defaults to 50</param>
        /// <param name="page">Page number, starting from 0</param>
        /// <param name="types">Comma separated list of log types: created, accessed, edited, viewed, captured</param>
        /// <returns>List of Camera Logs as objects</returns>
        public LogObjects GetLogObjects(string id, int? from, int? to, int? limit, int? page, string types)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_LOGS, id), Method.GET);
                if (from.HasValue && from.Value > 0) request.AddParameter("from", from.Value);
                if (to.HasValue && to.Value > 0) request.AddParameter("to", to.Value);
                if (limit.HasValue && limit.Value > 0) request.AddParameter("limit", limit.Value);
                if (page.HasValue && page.Value > -1) request.AddParameter("page", page.Value);
                if (!string.IsNullOrEmpty(types)) request.AddParameter("types", types);

                request.AddParameter("objects", true);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JsonConvert.DeserializeObject<LogObjects>(response.Content);
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        #endregion

        #region ARCHIVES

        /// <summary>
        /// Returns data for a specified set of archives. The ultimate intention would be to expand this functionality to be a more general search. The current implementation is as a basic absolute match list capability.
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <returns>List of Archives objects</returns>
        public List<Camera> GetArchives(string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.ARCHIVES, id), Method.GET);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["archives"].ToObject<List<Camera>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns data of pending archive. The ultimate intention would be to expand this functionality to be a more general search. The current implementation is as a basic absolute match list capability.
        /// </summary>
        /// <param name="ids">Camera ID</param>
        /// <param name="user_id">Unique Camera Id</param>
        /// <returns>Archive objects</returns>
        public Archive GetArchive(string camera_id, string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.ARCHIVES_UPDATE, camera_id, id), Method.GET);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["archives"].ToObject<List<Archive>>().FirstOrDefault<Archive>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns data of first pending archive.
        /// </summary>
        /// <returns>First Pending Archive objects</returns>
        public Archive GetPendingArchive()
        {
            try
            {
                var request = new RestRequest(API.ARCHIVES_PENDING, Method.GET);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["archives"].ToObject<List<Archive>>().FirstOrDefault<Archive>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Updates full or partial of Archive
        /// </summary>
        /// <param name="user">Archive Details</param>
        /// <returns>Archive Details</returns>
        public Archive UpdateArchive(ArchiveInfo archive)
        {
            try
            {
                var request = new RestRequest(string.Format(API.ARCHIVES_UPDATE, archive.CameraId, archive.ID), Method.PATCH);
                request.AddParameter("text/json", JsonConvert.SerializeObject(archive), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                SetClientCredentials(request, true);
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["archives"].ToObject<List<Archive>>().FirstOrDefault<Archive>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        #endregion

        #region SNAPSHOTS

        /// <summary>
        /// Returns the list of all snapshots currently stored for this camera
        /// </summary>
        /// <param name="id">Unique identifier for the camera</param>
        /// <param name="from">From Unix timestamp</param>
        /// <param name="to">To Unix timestamp</param>
        /// <param name="limit">The maximum number of cameras to retrieve. Defaults to 100, cannot be more than 1000</param>
        /// <param name="page">Page number, starting from 0</param>
        /// <returns>List of Snapshots</returns>
        public List<Snapshot> GetSnapshots(string id, long? from, long? to, int? limit, int? page)
        {
            List<Parameter> param = new List<Parameter>();
            
            if (from.HasValue) param.Add(new Parameter() { Name = "from", Value = from, Type = ParameterType.GetOrPost });
            if (to.HasValue) param.Add(new Parameter() { Name = "to", Value = to, Type = ParameterType.GetOrPost });
            if (limit.HasValue) param.Add(new Parameter() { Name = "limit", Value = limit, Type = ParameterType.GetOrPost });
            if (page.HasValue) param.Add(new Parameter() { Name = "page", Value = page, Type = ParameterType.GetOrPost });

            return GetAllSnapshots(string.Format(API.CAMERAS_SNAPSHOT, id), param);
        }

        /// <summary>
        /// Fetches a snapshot from the camera and stores it using the current timestamp
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="notes">Optional text note for this snapshot</param>
        /// <param name="with_data">Should it return image data?</param>
        /// <returns></returns>
        public Snapshot CreateSnapshot(string id, string notes, bool? with_data)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SNAPSHOT, id), Method.POST);
                if (!string.IsNullOrEmpty(notes)) request.AddParameter("notes", notes, ParameterType.GetOrPost);
                if (with_data.HasValue) request.AddParameter("with_data", with_data.ToString().ToLower(), ParameterType.GetOrPost);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content).ToObject<Snapshot>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns the snapshot stored for this camera closest to the given timestamp
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="with_data">Should it send image data?</param>
        /// <param name="range">Time range in seconds around specified timestamp. Default range is one second (so it matches only exact timestamp)</param>
        /// <returns>Snapshot</returns>
        public Snapshot GetSnapshot(string id, string timestamp, bool? with_data, int? range)
        {
            List<Parameter> param = new List<Parameter>();

            if (with_data.HasValue) param.Add(new Parameter() { Name = "with_data", Value = with_data.ToString().ToLower(), Type = ParameterType.GetOrPost });
            if (range.HasValue) param.Add(new Parameter() { Name = "range", Value = range, Type = ParameterType.GetOrPost });

            return GetAllSnapshots(string.Format(API.CAMERAS_SNAPSHOT_TIMESTAMP, id, timestamp), param).FirstOrDefault<Snapshot>();
        }

        /// <summary>
        /// Stores the supplied snapshot image data for the given timestamp
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="timestamp">Snapshot Unix timestamp</param>
        /// <param name="file">Image file path</param>
        /// <param name="notes">Optional text note for this snapshot</param>
        /// <returns></returns>
        public Snapshot StoreSnapshot(string id, string timestamp, string file, string notes)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SNAPSHOT_TIMESTAMP, id, timestamp), Method.POST);
                request.AddFile("data", file);
                if (!string.IsNullOrEmpty(notes)) request.AddParameter("notes", notes, ParameterType.GetOrPost);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["snapshots"].ToObject<List<Snapshot>>().FirstOrDefault<Snapshot>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Stores the supplied snapshot image data for the given timestamp
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="timestamp">Snapshot Unix timestamp</param>
        /// <param name="data">Image file data</param>
        /// <param name="notes">Optional text note for this snapshot</param>
        /// <returns></returns>
        public Snapshot StoreSnapshot(string id, string timestamp, byte[] data, string notes)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SNAPSHOT_TIMESTAMP, id, timestamp), Method.POST);
                request.RequestFormat = DataFormat.Json;

                request.AddParameter("data", data, ParameterType.GetOrPost);
                if (!string.IsNullOrEmpty(notes)) request.AddParameter("notes", notes, ParameterType.GetOrPost);

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["snapshots"].ToObject<List<Snapshot>>().FirstOrDefault<Snapshot>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Deletes any snapshot for this camera which exactly matches the timestamp
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="timestamp">Snapshot Unix timestamp</param>
        /// <returns></returns>
        public Snapshot DeleteSnapshot(string id, string timestamp)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SNAPSHOT_TIMESTAMP, id, timestamp), Method.DELETE);

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["snapshots"].ToObject<List<Snapshot>>().FirstOrDefault<Snapshot>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns list of specific hours in a given day which contains any snapshots
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="year">Year, for example 2015</param>
        /// <param name="month">Month, for example 11</param>
        /// <param name="day">Day, for example 17</param>
        /// <returns>List of hours</returns>
        public List<int> GetSnapshotHours(string id, int year, int month, int day)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SNAPSHOT_HOURS, id, year, month, day), Method.GET);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Found:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["hours"].ToObject<List<int>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns list of specific days in a given month which contains any snapshots
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="year">Year, for example 2013</param>
        /// <param name="month">Month, for example 11</param>
        /// <returns>List of days</returns>
        public List<int> GetSnapshotDays(string id, int year, int month)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SNAPSHOT_DAYS, id, year, month), Method.GET);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Found:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["days"].ToObject<List<int>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns latest snapshot stored for this camera
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="withData">Should it send image data?</param>
        /// <returns>Snapshot</returns>
        public Snapshot GetLatestSnapshot(string id, bool? withData)
        {
            List<Parameter> param = new List<Parameter>();

            if (withData.HasValue) param.Add(new Parameter() { Name = "with_data", Value = withData.ToString().ToLower(), Type = ParameterType.GetOrPost });

            return GetAllSnapshots(string.Format(API.CAMERAS_SNAPSHOT_LATEST, id), param).FirstOrDefault<Snapshot>();
        }

        #endregion

        #region SHARES

        /// <summary>
        /// Get the list of shares for a specified camera
        /// </summary>
        /// <param name="id">The unique identifier for the camera</param>
        /// <param name="user_id">The unique identifier for the user the camera is shared with</param>
        /// <returns></returns>
        public List<Share> GetCameraShares(string id, string user_id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SHARES, id), Method.GET);
                
                if (!string.IsNullOrEmpty(user_id)) request.AddParameter("user_id", user_id, ParameterType.GetOrPost);
                
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Found:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["shares"].ToObject<List<Share>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Create a new camera share
        /// </summary>
        /// <param name="share">Camera Share</param>
        /// <returns>Details of new Camera Share</returns>
        public Share CreateCameraShare(ShareInfo share)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SHARES, share.CameraID), Method.POST);
                request.RequestFormat = DataFormat.Json;

                request.AddParameter("email", share.Email, ParameterType.GetOrPost);
                request.AddParameter("rights", share.Rights, ParameterType.GetOrPost);
                if (!string.IsNullOrEmpty(share.Message)) request.AddParameter("message", share.Message, ParameterType.GetOrPost);
                if (!string.IsNullOrEmpty(share.Notify)) request.AddParameter("notify", share.Notify, ParameterType.GetOrPost);
                if (!string.IsNullOrEmpty(share.Grantor)) request.AddParameter("grantor", share.Grantor, ParameterType.GetOrPost);

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["shares"].ToObject<List<Share>>().FirstOrDefault();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Update an existing camera share
        /// </summary>
        /// <param name="id">The unique identifier of the camera share to be updated</param>
        /// <param name="rights">A comma separate list of the rights to be set on the share.</param>
        /// <returns>Updated Share Details</returns>
        public Share UpdateCameraShare(int id, string rights)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SHARES, id), Method.PATCH);
                request.AddParameter("rights", rights, ParameterType.GetOrPost);

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["shares"].ToObject<List<Share>>().FirstOrDefault<Share>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Delete an existing camera share
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="share_id">Camera Share ID</param>
        /// <returns></returns>
        public Share DeleteCameraShare(string id, int share_id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SHARES, id), Method.DELETE);
                request.AddParameter("share_id", share_id, ParameterType.GetOrPost);

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["shares"].ToObject<List<Share>>().FirstOrDefault<Share>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Fetch the list of share requests currently outstanding for a given camera
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="status">The request status to fetch, either 'PENDING', 'USED' or 'CANCELLED'</param>
        /// <returns>List of Camera Share Reqeusts</returns>
        public List<ShareRequest> GetCameraShareRequests(string id, string status)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SHARES_REQUESTS, id), Method.GET);
                if (!string.IsNullOrEmpty(status)) request.AddParameter("status", status, ParameterType.GetOrPost);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Found:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["share_requests"].ToObject<List<ShareRequest>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Cancels a pending camera share request for a given camera
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="email">The email address of user the camera was shared with</param>
        /// <param name="shareId">Camera Share ID</param>
        /// <returns></returns>
        public ShareRequest DeleteCameraShareRequest(string id, string email)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SHARES_REQUESTS, id), Method.DELETE);
                request.AddParameter("email", email, ParameterType.RequestBody);

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["share_requests"].ToObject<List<ShareRequest>>().FirstOrDefault<ShareRequest>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Updates a pending camera share request
        /// </summary>
        /// <param name="id">The unique identifier of the camera share request to update</param>
        /// <param name="rights">The new set of rights to be granted for the share</param>
        /// <param name="email">The email address of user the camera was shared with</param>
        /// <returns>Update Share Request Details</returns>
        public ShareRequest UpdateCameraShareRequest(string id, string rights, string email)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SHARES_REQUESTS, id), Method.PATCH);
                request.AddParameter("rights", rights, ParameterType.GetOrPost);
                request.AddParameter("email", rights, ParameterType.GetOrPost);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, true);
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["share_requests"].ToObject<List<ShareRequest>>().FirstOrDefault<ShareRequest>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        #endregion

        #region WEBHOOKS

        /// <summary>
        /// Create a new webhook
        /// </summary>
        /// <param name="id">Unique identifier for the camera</param>
        /// <param name="user_id">Unique identifier for the user</param>
        /// <param name="url">Webhook URL</param>
        /// <returns>Details of new Camera Share</returns>
        public WebHook CreateCameraWebhook(string id, string user_id, string url)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_WEBHOOKS, id), Method.POST);
                request.RequestFormat = DataFormat.Json;

                request.AddParameter("user_id", user_id, ParameterType.GetOrPost);
                request.AddParameter("url", url, ParameterType.GetOrPost);

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["webhooks"].ToObject<List<WebHook>>().FirstOrDefault();
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns list of webhooks for a given camera
        /// </summary>
        /// <param name="id">Unique identifier for the camera</param>
        /// <param name="webhook_id">Unique identifier for the webhook</param>
        /// <returns></returns>
        public List<WebHook> GetCameraWebhooks(string id, string webhook_id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_WEBHOOKS, id), Method.GET);
                if (!string.IsNullOrEmpty(webhook_id)) request.AddParameter("webhook_id", webhook_id, ParameterType.GetOrPost);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Found:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["webhooks"].ToObject<List<WebHook>>();
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Deletes specified webhook
        /// </summary>
        /// <param name="id">Unique identifier for the camera</param>
        /// <param name="webhook_id">Unique identifier for the webhook</param>
        /// <returns></returns>
        public WebHook DeleteCameraWebhook(string id, string webhook_id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_WEBHOOKS_ID, id, webhook_id), Method.DELETE);
                request.AddParameter("webhook_id", webhook_id, ParameterType.GetOrPost);

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["webhooks"].ToObject<List<WebHook>>().FirstOrDefault<WebHook>();
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Updates webhook URL
        /// </summary>
        /// <param name="id">Unique identifier for the camera</param>
        /// <param name="webhook_id">Unique identifier for the webhook</param>
        /// <param name="url">Webhook URL</param>
        /// <returns>Updated webhook details</returns>
        public WebHook UpdateCameraWebhook(string id, string webhook_id, string url)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_WEBHOOKS_ID, id, webhook_id), Method.PATCH);
                request.AddParameter("url", url, ParameterType.GetOrPost);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, true);
                
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content)["webhooks"].ToObject<List<WebHook>>().FirstOrDefault();
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        #endregion

        #region PUBLIC

        /// <summary>
        /// Fetch a list of publicly discoverable cameras from within the Evercam system
        /// </summary>
        /// <returns></returns>
        public List<PublicCamera> GetPublicCameras(int? offset, int? limit, bool? case_sensitive, string id_starts_with, string id_ends_with, string id_contains, string is_near_to, float? within_distance)
        {
            try
            {
                var request = new RestRequest(API.PUBLIC_CAMERAS, Method.GET);
                if (offset.HasValue) request.AddParameter("offset", offset.Value, ParameterType.GetOrPost);
                if (limit.HasValue) request.AddParameter("limit", limit.Value, ParameterType.GetOrPost);
                if (case_sensitive.HasValue) request.AddParameter("case_sensitive", case_sensitive.Value, ParameterType.GetOrPost);
                if (!string.IsNullOrEmpty(id_starts_with)) request.AddParameter("id_starts_with", id_starts_with, ParameterType.GetOrPost);
                if (!string.IsNullOrEmpty(id_ends_with)) request.AddParameter("id_ends_with", id_ends_with, ParameterType.GetOrPost);
                if (!string.IsNullOrEmpty(id_contains)) request.AddParameter("id_contains", id_contains, ParameterType.GetOrPost);
                if (!string.IsNullOrEmpty(is_near_to)) request.AddParameter("is_near_to", is_near_to, ParameterType.GetOrPost);
                if (within_distance.HasValue) request.AddParameter("within_distance", within_distance.Value, ParameterType.GetOrPost);

                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["cameras"].ToObject<List<PublicCamera>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Fetch nearest publicly discoverable camera from within the Evercam system.If location isn't provided requester's IP address is used
        /// </summary>
        /// <returns></returns>
        public List<PublicCamera> GetNearestPublicCameras(string near_to)
        {
            try
            {
                var request = new RestRequest(API.PUBLIC_CAMERAS_NEAREST, Method.GET);
                if (!string.IsNullOrEmpty(near_to)) request.AddParameter("near_to", near_to, ParameterType.GetOrPost);

                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["cameras"].ToObject<List<PublicCamera>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns jpg from nearest publicly discoverable camera from within the Evercam system.If location isn't provided requester's IP address is used
        /// </summary>
        /// <returns></returns>
        public byte[] GetNearestPublicCameraSnapshot(string near_to)
        {
            try
            {
                var request = new RestRequest(API.PUBLIC_CAMERAS_NEAREST_SNAPSHOT, Method.GET);
                if (!string.IsNullOrEmpty(near_to)) request.AddParameter("near_to", near_to, ParameterType.GetOrPost);

                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return response.RawBytes;
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
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
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["vendors"].ToObject<List<Vendor>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
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

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// PRIVATE: Get list of all camera snapshots
        /// </summary>
        /// <param name="url">API Endpoint</param>
        /// <returns>List of Snapshots</returns>
        public List<Snapshot> GetAllSnapshots(string url)
        {
            try
            {
                var request = new RestRequest(url, Method.GET);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["snapshots"].ToObject<List<Snapshot>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// PRIVATE: Get list of all camera snapshots
        /// </summary>
        /// <param name="url">API Endpoint</param>
        /// <param name="parameters">List of additional request parameters</param>
        /// <returns>List of Snapshots</returns>
        public List<Snapshot> GetAllSnapshots(string url, List<Parameter> parameters)
        {
            try
            {
                var request = new RestRequest(url, Method.GET);
                request.RequestFormat = DataFormat.Json;

                SetAuthHeader();
                SetClientCredentials(request, false);

                if (parameters != null && parameters.Count > 0)
                    request.Parameters.AddRange(parameters);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["snapshots"].ToObject<List<Snapshot>>();
                }
                try
                {
                    throw new EvercamException(JObject.Parse(response.Content).ToObject<Error>().Message, response.ErrorException);
                }
                catch
                {
                    throw new EvercamException(response.Content);
                }
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Sets authentication header if provided by user (OAuth or Basic)
        /// </summary>
        private void SetAuthHeader()
        {
            if (Auth != null && Auth.OAuth2 != null && !string.IsNullOrEmpty(Auth.OAuth2.AccessToken))
                API.Client.Value.Authenticator = new HttpOAuth2Authenticator(Auth.OAuth2.AccessToken, Auth.OAuth2.TokenType);
            else if (Auth != null && Auth.Basic != null && !string.IsNullOrEmpty(Auth.Basic.UserName))
                API.Client.Value.Authenticator = new HttpBasicAuthenticator(Auth.Basic.UserName, Auth.Basic.Password);
        }

        /// <summary>
        /// Adds client credentials as parameters to given request
        /// </summary>
        /// <param name="request">RestRequest</param>
        /// <param name="forceCredentials">Make sure if request requires client credentials as must, errors if not specified</param>
        private void SetClientCredentials(RestRequest request, bool forceCredentials)
        {
            if (forceCredentials && (Client == null || string.IsNullOrEmpty(Client.ID) || string.IsNullOrEmpty(Client.Secret)))
                throw new EvercamException("Client credentials not presented (ID, Secret, Redirect Uri)");

            if (Client != null && !string.IsNullOrEmpty(Client.ID) && !string.IsNullOrEmpty(Client.Secret))
            {
                request.AddParameter("api_id", Client.ID, ParameterType.QueryString);
                request.AddParameter("api_key", Client.Secret, ParameterType.QueryString);
                if (!string.IsNullOrEmpty(Client.RedirectUri))
                    request.AddParameter("redirect_uri", Client.RedirectUri, ParameterType.QueryString);
            }
        }

        public static byte[] DownloadImage(string url)
        {
            byte[] bytes = new byte[] { };
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (MemoryStream ms = new MemoryStream(100000))
                    {
                        if (response.ContentType.Contains("image") && stream != null)
                        {
                            stream.CopyTo(ms);
                            bytes = ms.ToArray();
                        }
                    }
                }
            }

            return bytes;
        }

        #endregion
    }

    public class Error
    {
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message;
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code;
        [JsonProperty("context", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Context;
    }
}
