﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace EvercamV1
{
    public class Vendor
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("known_macs", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> KnownMacs { get; set; }

        [JsonProperty("is_supported", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsSupported { get; set; }

        [JsonProperty("models", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Models { get; set; }
    }
}
