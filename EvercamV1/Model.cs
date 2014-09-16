using System.Collections.Generic;
using Newtonsoft.Json;

namespace EvercamV1
{
    public class Model
    {
        /// <summary>
        /// Name of the model
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Name of the model
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Unique identifier for the vendor
        /// </summary>
        [JsonProperty("vendor_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Vendor { get; set; }

        /// <summary>
        /// Various default values used by this camera model
        /// </summary>
        [JsonProperty("defaults", NullValueHandling = NullValueHandling.Ignore)]
        public Defaults Defaults { get; set; }
    }

    public class Defaults
    {
        [JsonProperty("auth", NullValueHandling = NullValueHandling.Ignore)]
        public Auth Auth;

        [JsonProperty("snapshots", NullValueHandling = NullValueHandling.Ignore)]
        public Snapshots Snapshots;
    }

    public class Snapshots
    {
        [JsonProperty("h264", NullValueHandling = NullValueHandling.Ignore)]
        public string H264;
        [JsonProperty("lowres", NullValueHandling = NullValueHandling.Ignore)]
        public string Lowres;
        [JsonProperty("jpg", NullValueHandling = NullValueHandling.Ignore)]
        public string Jpg;
        [JsonProperty("mpeg4", NullValueHandling = NullValueHandling.Ignore)]
        public string Mpeg4;
        [JsonProperty("mobile", NullValueHandling = NullValueHandling.Ignore)]
        public string Mobile;
        [JsonProperty("mjpg", NullValueHandling = NullValueHandling.Ignore)]
        public string Mjpg;
    }
}
