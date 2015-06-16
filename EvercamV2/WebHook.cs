using Newtonsoft.Json;

namespace EvercamV2
{
    public class WebHook
    {
        /// <summary>
        /// Unique identifier of the webhook
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Unique identifier of the camera to be shared
        /// </summary>
        [JsonProperty("camera_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CameraID { get; set; }

        /// <summary>
        /// Url which will receive webhook data
        /// </summary>
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string URL { get; set; }
    }
}
