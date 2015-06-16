using Newtonsoft.Json;

namespace EvercamV2
{
    public class Snapshot
    {
        /// <summary>
        /// (optional) Snapshot timestamp
        /// </summary>
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public long CreatedAt;

        /// <summary>
        /// (optional) Note for snapshot
        /// </summary>
        [JsonProperty("notes", NullValueHandling = NullValueHandling.Ignore)]
        public string Notes;

        /// <summary>
        /// (optional) Image data
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data;

        /// <summary>
        /// Converts base64 data string to bytes array
        /// </summary>
        /// <returns>byte[]</returns>
        public byte[] ToBytes()
        {
            return Utility.ToBytes(this.Data);
        }
    }
}
