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
        /// Initializes Evercam with Client credentials
        /// </summary>
        /// <param name="api_id">Evercan API ID for a client/user</param>
        /// <param name="api_key">Evercan API Key/Secret of a client/user</param>
        /// <param name="redirect_uri">Client's registered URI at Evercam</param>
        public Evercam(string api_id, string api_key, string redirect_uri)
        {
            Client = new EvercamClient(api_id, api_key, redirect_uri);
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
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Accepted:
                        return JObject.Parse(response.Content)["models"].ToObject<List<Model>>().FirstOrDefault<Model>();
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
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
                
                SetClientCredentials(request, true);
                
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
                SetClientCredentials(request, true);
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

        /// <summary>
        /// Fetch the list of shares currently granted to a user
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <returns>List of Camera Shares</returns>
        public List<Share> GetUserShares(string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.SHARES_USERS, id), Method.GET);
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
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Fetch API credentials for an authenticated user
        /// </summary>
        /// <param name="id">Evercam user's Login ID</param>
        /// <param name="password">Evercam user's Password</param>
        /// <returns></returns>
        public string GetCredentials(string id, string password)
        {
            try
            {
                var request = new RestRequest(string.Format(API.USERS_CREDENTIALS, id), Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Add(new Parameter() { Name = "password", Value = password, Type = ParameterType.GetOrPost });
                
                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.Continue:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content).ToObject<Message>().Contents;
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
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
                request.AddParameter("text/json", JsonConvert.SerializeObject(info), ParameterType.RequestBody);
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
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns base64 encoded jpg from the camera
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <returns>Returns live image data</returns>
        public LiveImage GetLiveImage(string id)
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
                    case HttpStatusCode.Created:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Accepted:
                        return JObject.Parse(response.Content).ToObject<LiveImage>();
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
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
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
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
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
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
                SetClientCredentials(request, true);
                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                        return response.Content;
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        #region SNAPSHOTS

        /// <summary>
        /// Returns the list of all snapshots currently stored for this camera
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <returns>List of Snapshots</returns>
        public List<Snapshot> GetSnapshots(string id)
        {
            return GetAllSnapshots(string.Format(API.CAMERAS_SNAPSHOT, id));
        }

        /// <summary>
        /// Returns the snapshot stored for this camera closest to the given timestamp
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="timestamp">Timestamp</param>
        /// <returns>Snapshot</returns>
        public Snapshot GetSnapshot(string id, string timestamp)
        {
            return GetAllSnapshots(string.Format(API.CAMERAS_SNAPSHOT_TIMESTAMP, id, timestamp)).FirstOrDefault<Snapshot>();
        }

        /// <summary>
        /// Returns latest snapshot stored for this camera
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="withData">Should it send image data</param>
        /// <returns>Snapshot</returns>
        public Snapshot GetLatestSnapshot(string id, bool withData)
        {
            if (withData)
                return GetAllSnapshots(string.Format(API.CAMERAS_SNAPSHOT_LATEST, id)).FirstOrDefault<Snapshot>();
            else
                return GetAllSnapshots(string.Format(API.CAMERAS_SNAPSHOT_LATEST, id), 
                    new List<Parameter>() { 
                        new Parameter() { Name = "with_data", Value = withData, Type = ParameterType.GetOrPost } 
                    }).FirstOrDefault<Snapshot>();
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
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns list of specific hours in a given day which contains any snapshots
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="year">Year, for example 2013</param>
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
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Returns list of snapshots between two timestamps
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="from">From Unix timestamp</param>
        /// <param name="to">To Unix timestamp</param>
        /// <param name="withData">Should it send image data</param>
        /// <param name="limit">Limit number of results, default 100 with no data, 10 with data</param>
        /// <param name="page">Page number</param>
        /// <returns>List of Snapshots</returns>
        public Snapshot GetSnapshotsRange(string id, long from, long to, bool withData, int limit, int page)
        {
            return GetAllSnapshots(string.Format(API.CAMERAS_SNAPSHOT_RANGE, id),
                new List<Parameter>() {
                    new Parameter() { Name = "from", Value = from, Type = ParameterType.GetOrPost },
                    new Parameter() { Name = "to", Value = to, Type = ParameterType.GetOrPost },
                    new Parameter() { Name = "with_data", Value = withData, Type = ParameterType.GetOrPost },
                    new Parameter() { Name = "limit", Value = limit, Type = ParameterType.GetOrPost },
                    new Parameter() { Name = "page", Value = page, Type = ParameterType.GetOrPost }
                }).FirstOrDefault<Snapshot>();
        }

        /// <summary>
        /// Fetches a snapshot from the camera and stores it using the current timestamp
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <returns></returns>
        public Snapshot CreateSnapshot(string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SNAPSHOT, id), Method.POST);
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
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Stores the supplied snapshot image data for the given timestamp
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="timestamp">Snapshot Unix timestamp</param>
        /// <param name="notes">Optional text note for this snapshot</param>
        /// <param name="data">Image file</param>
        /// <returns></returns>
        public Snapshot CreateSnapshot(string id, string timestamp, string notes, byte[] data)
        {
            try
            {
                var request = new RestRequest(string.Format(API.CAMERAS_SNAPSHOT_TIMESTAMP, id, timestamp), Method.POST);
                request.RequestFormat = DataFormat.Json;

                request.Parameters.Add(new Parameter() { Name = "data", Value = data, Type = ParameterType.GetOrPost });
                request.Parameters.Add(new Parameter() { Name = "notes", Value = notes, Type = ParameterType.GetOrPost });

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
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Deletes any snapshot for this camera which exactly matches the timestamp
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="timestamp">Timestamp</param>
        /// <returns></returns>
        public string DeleteSnapshot(string id, string timestamp)
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
                        return response.Content;
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        #endregion

        #endregion

        #region SHARES

        /// <summary>
        /// Get the list of shares for a specified camera
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <returns>List of Camera Shares</returns>
        public List<Share> GetCameraShares(string id)
        {
            try
            {
                var request = new RestRequest(string.Format(API.SHARES_CAMERAS, id), Method.GET);
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
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
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
                var request = new RestRequest(string.Format(API.SHARES_CAMERAS, share.ID), Method.POST);
                request.RequestFormat = DataFormat.Json;

                request.AddParameter("email", share.Email, ParameterType.RequestBody);
                request.AddParameter("rights", share.Rights, ParameterType.RequestBody);

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
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Update an existing camera share (COMING SOON)
        /// </summary>
        /// <param name="share">Camera Share</param>
        /// <returns></returns>
        public string UpdateCameraShare(ShareInfo share)
        {
            return "TO-DO";
            //try
            //{
            //    var request = new RestRequest(string.Format(API.CAMERAS_SHARE, share.ID), Method.DELETE);

            //    SetAuthHeader();
            //    SetClientCredentials(request, true);

            //    var response = API.Client.Value.Execute(request);

            //    switch (response.StatusCode)
            //    {
            //        case HttpStatusCode.OK:
            //        case HttpStatusCode.NoContent:
            //            return response.Content;
            //    }
            //    throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
            //}
            //catch (Exception x) { throw new EvercamException(x); }
        }

        /// <summary>
        /// Delete an existing camera share
        /// </summary>
        /// <param name="id">Camera ID</param>
        /// <param name="shareId">Camera Share ID</param>
        /// <returns></returns>
        public string DeleteCameraShare(string id, int shareId)
        {
            try
            {
                var request = new RestRequest(string.Format(API.SHARES_CAMERAS, id), Method.DELETE);
                request.AddParameter("share_id", shareId, ParameterType.RequestBody);

                SetAuthHeader();
                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                        return response.Content;
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
            }
            catch (Exception x) { throw new EvercamException(x); }
        }

        #endregion

        #region PUBLIC

        /// <summary>
        /// Fetch a list of publicly discoverable cameras from within the Evercam system.
        /// </summary>
        /// <returns></returns>
        public List<Camera> GetCameras()
        {
            return GetAllCameras(API.PUBLIC_CAMERAS);
        }

        #endregion

        #region TEST

        /// <summary>
        /// Initializes Evercam with Client credentials
        /// </summary>
        /// <param name="api_id">Evercan API ID for a client/user</param>
        /// <param name="api_key">Evercan API Key/Secret of a client/user</param>
        public string TestCredentials()
        {
            try
            {
                var request = new RestRequest(API.TEST, Method.GET);
                request.RequestFormat = DataFormat.Json;

                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.Continue:
                    case HttpStatusCode.NoContent:
                        return JObject.Parse(response.Content).ToObject<Message>().Contents;
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
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
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
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

                SetClientCredentials(request, false);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["cameras"].ToObject<List<Camera>>();
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
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

                if (Auth != null && Auth.OAuth2 != null && !string.IsNullOrEmpty(Auth.OAuth2.AccessToken))
                    API.Client.Value.Authenticator = new HttpOAuth2Authenticator(Auth.OAuth2.AccessToken, Auth.OAuth2.TokenType);
                else if (Auth != null && Auth.Basic != null && !string.IsNullOrEmpty(Auth.Basic.UserName))
                    API.Client.Value.Authenticator = new HttpBasicAuthenticator(Auth.Basic.UserName, Auth.Basic.Password);

                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["snapshots"].ToObject<List<Snapshot>>();
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
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

                if (Auth != null && Auth.OAuth2 != null && !string.IsNullOrEmpty(Auth.OAuth2.AccessToken))
                    API.Client.Value.Authenticator = new HttpOAuth2Authenticator(Auth.OAuth2.AccessToken, Auth.OAuth2.TokenType);
                else if (Auth != null && Auth.Basic != null && !string.IsNullOrEmpty(Auth.Basic.UserName))
                    API.Client.Value.Authenticator = new HttpBasicAuthenticator(Auth.Basic.UserName, Auth.Basic.Password);

                if (parameters != null && parameters.Count > 0)
                    request.Parameters.AddRange(parameters);

                SetClientCredentials(request, true);

                var response = API.Client.Value.Execute(request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                        return JObject.Parse(response.Content)["snapshots"].ToObject<List<Snapshot>>();
                }
                throw new EvercamException(JObject.Parse(response.Content).ToObject<Message>().Contents, response.ErrorException);
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
            if (forceCredentials && Client == null)
                throw new EvercamException("Client credentials not presented (ID, Secret, Redirect Uri)");

            if (Client != null) {
                request.Parameters.Add(new Parameter() { Name = "api_id", Value = Client.ID, Type = ParameterType.GetOrPost });
                request.Parameters.Add(new Parameter() { Name = "api_key", Value = Client.Secret, Type = ParameterType.GetOrPost });
                request.Parameters.Add(new Parameter() { Name = "redirect_uri", Value = Client.RedirectUri, Type = ParameterType.GetOrPost });
            }
        }

        #endregion
    }

    public class Message
    {
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Contents;
    }
}
