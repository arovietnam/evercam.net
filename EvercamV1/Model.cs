using System.Collections.Generic;
using Newtonsoft.Json;

namespace EvercamV1
{
    public class Model
    {
        /// <summary>
        /// Name of the model
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Unique identifier for the vendor
        /// </summary>
        [JsonProperty("vendor", NullValueHandling = NullValueHandling.Ignore)]
        public string Vendor { get; set; }

        /// <summary>
        /// String array of all models known to share the same defaults
        /// </summary>
        [JsonProperty("known_models", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> KnownModels { get; set; }

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
        [JsonProperty("jpg", NullValueHandling = NullValueHandling.Ignore)]
        public string Jpg;
    }
}
