using System.Collections.Generic;
using Newtonsoft.Json;

namespace EvercamV1
{
    public class Vendor
    {
        /// <summary>
        /// Unique identifier for the vendor
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Name of the vendor
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// String array of MAC prefixes the vendor uses
        /// </summary>
        [JsonProperty("known_macs", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> KnownMacs { get; set; }

        /// <summary>
        /// (optional) Whether or not this vendor produces Evercam supported cameras
        /// </summary>
        [JsonProperty("is_supported", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsSupported { get; set; }

        /// <summary>
        /// (optional) String array of models currently known for this vendor
        /// </summary>
        [JsonProperty("models", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Models { get; set; }
    }
}
