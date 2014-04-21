using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EvercamV1
{
    public class Snapshot
    {
        [JsonProperty("camera", NullValueHandling = NullValueHandling.Ignore)]
        public string Camera;

        [JsonProperty("notes", NullValueHandling = NullValueHandling.Ignore)]
        public string Notes;

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public long CreatedAt;

        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone;

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data;
    }
}
