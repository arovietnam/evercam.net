using Newtonsoft.Json;

namespace EvercamV1
{
    public class Snapshot
    {
        /// <summary>
        /// Unique Evercam identifier for the camera
        /// </summary>
        [JsonProperty("camera", NullValueHandling = NullValueHandling.Ignore)]
        public string Camera;

        /// <summary>
        /// (optional) Note for snapshot
        /// </summary>
        [JsonProperty("notes", NullValueHandling = NullValueHandling.Ignore)]
        public string Notes;

        /// <summary>
        /// (optional) Snapshot timestamp
        /// </summary>
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public long CreatedAt;

        /// <summary>
        /// Name of the IANA/tz timezone where this camera is located
        /// </summary>
        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone;

        /// <summary>
        /// (optional) Image data
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data;
    }
}
