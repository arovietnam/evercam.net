using Newtonsoft.Json;

namespace EvercamV1
{
    public class Share
    {
        /// <summary>
        /// Unique identifier for a camera share
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int ID { get; set; }

        /// <summary>
        /// Unique identifier of the shared camera
        /// </summary>
        [JsonProperty("camera_id")]
        public string CameraID { get; set; }

        /// <summary>
        /// The unique identifier of the user who shared the camera
        /// </summary>
        [JsonProperty("sharer_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SharerID { get; set; }

        /// <summary>
        /// Unique user id of the user the camera is shared with
        /// </summary>
        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserID { get; set; }

        /// <summary>
        /// The email address of the user the camera is shared with
        /// </summary>
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>
        /// Either 'public' or 'private' depending on the share kind
        /// </summary>
        [JsonProperty("kind", NullValueHandling = NullValueHandling.Ignore)]
        public string Kind { get; set; }

        /// <summary>
        /// A comma separated list of the rights available on the share
        /// </summary>
        [JsonProperty("rights", NullValueHandling = NullValueHandling.Ignore)]
        public string Rights { get; set; }
    }

    public class ShareInfo
    {
        /// <summary>
        /// Unique identifier for a camera share
        /// </summary>
        [JsonProperty("id")]
        public string CameraID { get; set; }

        /// <summary>
        /// Email address or user name of the user to share the camera with
        /// </summary>
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>
        /// A comma separate list of the rights to be granted with the share
        /// </summary>
        [JsonProperty("rights", NullValueHandling = NullValueHandling.Ignore)]
        public string Rights { get; set; }

        /// <summary>
        /// Not currently used
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Not currently used
        /// </summary>
        [JsonProperty("notify", NullValueHandling = NullValueHandling.Ignore)]
        public string Notify { get; set; }

        /// <summary>
        /// The user name of the user who is creating the share
        /// </summary>
        [JsonProperty("grantor", NullValueHandling = NullValueHandling.Ignore)]
        public string Grantor { get; set; }
    }

    public class ShareRequest
    {
        /// <summary>
        /// Unique identifier for a camera share request
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Unique identifier of the camera to be shared
        /// </summary>
        [JsonProperty("camera_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CameraID { get; set; }

        /// <summary>
        /// The unique identifier of the user who shared the camera
        /// </summary>
        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserID { get; set; }

        /// <summary>
        /// The email address of the user the camera is shared with
        /// </summary>
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>
        /// A comma separated list of the rights to be granted on the share
        /// </summary>
        [JsonProperty("rights", NullValueHandling = NullValueHandling.Ignore)]
        public string Rights { get; set; }
    }
}
