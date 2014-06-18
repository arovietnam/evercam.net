using Newtonsoft.Json;
using System.Collections.Generic;

namespace EvercamV1
{
    public class LogMessages
    {
        [JsonProperty("logs")]
        public List<string> Logs { get; set; }

        [JsonProperty("pages")]
        public int Pages { get; set; }
    }

    public class LogObjects
    {
        [JsonProperty("logs")]
        public List<Log> Logs { get; set; }

        [JsonProperty("camera_exid")]
        public string CameraID { get; set; }

        [JsonProperty("camera_name")]
        public string CameraName { get; set; }

        [JsonProperty("pages")]
        public int Pages { get; set; }
    }

    public class Log
    {
        [JsonProperty("who")]
        public string Who { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("done_at")]
        public int DoneAt { get; set; }

        [JsonProperty("extra")]
        public string Extra { get; set; }
    }
}
