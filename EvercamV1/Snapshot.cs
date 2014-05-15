using Newtonsoft.Json;

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
