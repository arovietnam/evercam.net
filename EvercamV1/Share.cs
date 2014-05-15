using Newtonsoft.Json;

namespace EvercamV1
{
    public class Share
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int ID { get; set; }

        [JsonProperty("camera_id")]
        public string CameraID { get; set; }

        [JsonProperty("sharer_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SharerID { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserID { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("kind", NullValueHandling = NullValueHandling.Ignore)]
        public string Kind { get; set; }

        [JsonProperty("rights", NullValueHandling = NullValueHandling.Ignore)]
        public string Rights { get; set; }
    }

    public class ShareInfo
    {
        [JsonProperty("id")]
        public string CameraID { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("rights", NullValueHandling = NullValueHandling.Ignore)]
        public string Rights { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("notify", NullValueHandling = NullValueHandling.Ignore)]
        public string Notify { get; set; }

        [JsonProperty("grantor", NullValueHandling = NullValueHandling.Ignore)]
        public string Grantor { get; set; }
    }

    public class ShareRequest
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("camera_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CameraID { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserID { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("rights", NullValueHandling = NullValueHandling.Ignore)]
        public string Rights { get; set; }
    }
}
