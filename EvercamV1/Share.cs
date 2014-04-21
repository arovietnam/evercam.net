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
    public class Share
    {
        [JsonProperty("sharer_id", NullValueHandling = NullValueHandling.Ignore)]
        public int ID { get; set; }

        [JsonProperty("camera_id")]
        public string CameraID { get; set; }

        [JsonProperty("sharer_id", NullValueHandling = NullValueHandling.Ignore)]
        public int SharerID { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public int UserID { get; set; }

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
        public string ID { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("rights", NullValueHandling = NullValueHandling.Ignore)]
        public string Rights { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("notify", NullValueHandling = NullValueHandling.Ignore)]
        public string Notify { get; set; }
    }
}
