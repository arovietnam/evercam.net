using Newtonsoft.Json;

namespace EvercamV2
{
    public class Archive
    {
        /// <summary>
        /// Unique Evercam username
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Archive camera_id
        /// </summary>
        [JsonProperty("camera_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CameraId { get; set; }

        /// <summary>
        /// Archive title
        /// </summary>
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>
        /// Unix timestamp at creation
        /// </summary>
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public long CreatedAt { get; set; }

        /// <summary>
        /// Unix timestamp Archive from date
        /// </summary>
        [JsonProperty("from_date", NullValueHandling = NullValueHandling.Ignore)]
        public long FromDate { get; set; }

        /// <summary>
        /// Unix timestamp Archive from date
        /// </summary>
        [JsonProperty("to_date", NullValueHandling = NullValueHandling.Ignore)]
        public long ToDate { get; set; }

        /// <summary>
        /// Archive Status
        /// </summary>
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public ArchiveStatus Status { get; set; }

        /// <summary>
        /// Archive Requested By
        /// </summary>
        [JsonProperty("requested_by", NullValueHandling = NullValueHandling.Ignore)]
        public string RequestedBy { get; set; }

        /// <summary>
        /// Embed time in Archive
        /// </summary>
        [JsonProperty("embed_time", NullValueHandling = NullValueHandling.Ignore)]
        public bool EmbedTime { get; set; }

        /// <summary>
        /// Is public archive
        /// </summary>
        [JsonProperty("public", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPublic { get; set; }

        /// <summary>
        /// Total Frames
        /// </summary>
        [JsonProperty("frames", NullValueHandling = NullValueHandling.Ignore)]
        public int Frames { get; set; }

        public ArchiveInfo GetInfo()
        {
            return new ArchiveInfo()
            {
                ID = this.ID,
                Title = this.Title,
                Status = (ArchiveStatus)this.Status,
                IsPublic = this.IsPublic,
                Frames = this.Frames
            };
        }
    }

    public class ArchiveInfo
    {
        /// <summary>
        /// Unique Evercam camera id
        /// </summary>
        [JsonProperty("id")]
        public string CameraId { get; set; }

        /// <summary>
        /// Unique Evercam archive id
        /// </summary>
        [JsonProperty("archive_id")]
        public string ID { get; set; }

        /// <summary>
        /// Archive title
        /// </summary>
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>
        /// Archive status
        /// </summary>
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public ArchiveStatus Status { get; set; }

        /// <summary>
        /// Is archive public
        /// </summary>
        [JsonProperty("public", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPublic { get; set; }

        /// <summary>
        /// Total Frames
        /// </summary>
        [JsonProperty("frames", NullValueHandling = NullValueHandling.Ignore)]
        public int Frames { get; set; }
    }

    public enum ArchiveStatus
    {
        Pending = 0,
        Processing = 1,
        completed = 2,
        Failed = 3,
    }
}
