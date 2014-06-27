using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace EvercamV1
{
    public class Camera
    {
        /// <summary>
        /// Unique Evercam identifier for the camera
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Human readable or friendly name for the camera
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Username of camera owner
        /// </summary>
        [JsonProperty("owner", NullValueHandling = NullValueHandling.Ignore)]
        public string Owner { get; set; }

        /// <summary>
        /// (optional) Unique identifier for the camera vendor
        /// </summary>
        [JsonProperty("vendor", NullValueHandling = NullValueHandling.Ignore)]
        public string Vendor { get; set; }

        /// <summary>
        /// (optional) The name for the camera vendor
        /// </summary>
        [JsonProperty("vendor_name", NullValueHandling = NullValueHandling.Ignore)]
        public string VendorName { get; set; }

        /// <summary>
        /// (optional) Name of the camera model
        /// </summary>
        [JsonProperty("model", NullValueHandling = NullValueHandling.Ignore)]
        public string Model { get; set; }

        /// <summary>
        /// Unix timestamp at creation
        /// </summary>
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public long CreatedAt { get; set; }

        /// <summary>
        /// Unix timestamp at last update
        /// </summary>
        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public long UpdatedAt { get; set; }

        /// <summary>
        /// (optional) Unix timestamp at last heartbeat poll
        /// </summary>
        [JsonProperty("last_polled_at", NullValueHandling = NullValueHandling.Ignore)]
        public long LastPolledAt { get; set; }

        /// <summary>
        /// (optional) Unix timestamp of the last successful heartbeat of the camera
        /// </summary>
        [JsonProperty("last_online_at", NullValueHandling = NullValueHandling.Ignore)]
        public long LastOnlineAt { get; set; }

        /// <summary>
        /// Name of the IANA/tz timezone where this camera is located
        /// </summary>
        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }

        /// <summary>
        /// (optional) Whether or not this camera is currently online
        /// </summary>
        [JsonProperty("is_online", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsOnline { get; set; }
        
        /// <summary>
        /// Whether or not this camera is publically available
        /// </summary>
        [JsonProperty("is_public", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPublic { get; set; }

        /// <summary>
        /// (optional) External host of the camera
        /// </summary>
        [JsonProperty("external_host", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalHost { get; set; }

        /// <summary>
        /// (optional) Internal host of the camera
        /// </summary>
        [JsonProperty("internal_host", NullValueHandling = NullValueHandling.Ignore)]
        public string InternalHost { get; set; }

        /// <summary>
        /// (optional) External http port of the camera
        /// </summary>
        [JsonProperty("external_http_port", NullValueHandling = NullValueHandling.Ignore)]
        public int ExternalHttpPort { get; set; }

        /// <summary>
        /// (optional) Internal http port of the camera
        /// </summary>
        [JsonProperty("internal_http_port", NullValueHandling = NullValueHandling.Ignore)]
        public int InternalHttpPort { get; set; }

        /// <summary>
        /// (optional) External rtsp port of the camera
        /// </summary>
        [JsonProperty("external_rtsp_port", NullValueHandling = NullValueHandling.Ignore)]
        public int ExternalRtspPort { get; set; }

        /// <summary>
        /// (optional) Internal rtsp port of the camera
        /// </summary>
        [JsonProperty("internal_rtsp_port", NullValueHandling = NullValueHandling.Ignore)]
        public int InternalRtspPort { get; set; }

        /// <summary>
        /// (optional) Short snapshot url using evr.cm url shortener
        /// </summary>
        [JsonProperty("jpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string JpegUrl { get; set; }

        /// <summary>
        /// (optional) RTSP url using evr.cm dynamic DNS
        /// </summary>
        [JsonProperty("rtsp_url", NullValueHandling = NullValueHandling.Ignore)]
        public string RtspUrl { get; set; }

        /// <summary>
        /// (optional) Camera username
        /// </summary>
        [JsonProperty("cam_username", NullValueHandling = NullValueHandling.Ignore)]
        public string CameraUsername { get; set; }

        /// <summary>
        /// (optional) Camera password
        /// </summary>
        [JsonProperty("cam_password", NullValueHandling = NullValueHandling.Ignore)]
        public string CameraPassword { get; set; }

        /// <summary>
        /// (optional) The physical network MAC address of the camera
        /// </summary>
        [JsonProperty("mac_address", NullValueHandling = NullValueHandling.Ignore)]
        public string MacAddress { get; set; }

        /// <summary>
        /// (optional) GPS lng and lat coordinates of the camera location
        /// </summary>
        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public Location Location;

        /// <summary>
        /// (optional) Whether the camera is publicly findable
        /// </summary>
        [JsonProperty("discoverable", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsDiscoverable { get; set; }

        /// <summary>
        /// (optional) Camera external URLs
        /// </summary>
        [JsonProperty("external", NullValueHandling = NullValueHandling.Ignore)]
        public URL External { get; set; }

        /// <summary>
        /// (optional) Camera internal URLs
        /// </summary>
        [JsonProperty("internal", NullValueHandling = NullValueHandling.Ignore)]
        public URL Internal { get; set; }

        /// <summary>
        /// (optional) Camera dyndns URLs
        /// </summary>
        [JsonProperty("dyndns", NullValueHandling = NullValueHandling.Ignore)]
        public URL Dyndns { get; set; }

        /// <summary>
        /// (optional) Camera short URLs
        /// </summary>
        [JsonProperty("short", NullValueHandling = NullValueHandling.Ignore)]
        public URL Short { get; set; }

        /// <summary>
        /// (optional) True if the user owns the camera, false otherwise
        /// </summary>
        [JsonProperty("owned", NullValueHandling = NullValueHandling.Ignore)]
        public bool Owned { get; set; }

        /// <summary>
        /// (optional) A comma separated list of the users rights on the camera
        /// </summary>
        [JsonProperty("rights", NullValueHandling = NullValueHandling.Ignore)]
        public string Rights { get; set; }

        public CameraInfo GetInfo()
        {
            return new CameraInfo() 
            {
                ID = this.ID,
                Name = this.Name,
                Username = this.CameraUsername,
                Password = this.CameraUsername,
                IsPublic = this.IsPublic,
                ExternalHost = this.ExternalHost,
                InternalHost = this.InternalHost,
                ExternalHttpPort = this.ExternalHttpPort,
                InternalHttpPort = this.InternalHttpPort,
                JpegUrl = this.JpegUrl
            };
        }
    }

    public class URL
    {
        [JsonProperty("jpg_url")]
        public string JpgUrl { get; set; }
    }

    public class CameraInfo
    {
        /// <summary>
        /// Unique Evercam identifier for the camera
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Human readable or friendly name for the camera
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Whether or not this camera is publically available
        /// </summary>
        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        /// <summary>
        /// (optional) External host of the camera
        /// </summary>
        [JsonProperty("external_host", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalHost { get; set; }

        /// <summary>
        /// (optional) Internal host of the camera
        /// </summary>
        [JsonProperty("internal_host", NullValueHandling = NullValueHandling.Ignore)]
        public string InternalHost { get; set; }

        /// <summary>
        /// (optional) External http port of the camera
        /// </summary>
        [JsonProperty("external_http_port", NullValueHandling = NullValueHandling.Ignore)]
        public int ExternalHttpPort { get; set; }

        /// <summary>
        /// (optional) Internal http port of the camera
        /// </summary>
        [JsonProperty("internal_http_port", NullValueHandling = NullValueHandling.Ignore)]
        public int InternalHttpPort { get; set; }

        /// <summary>
        /// (optional) Short snapshot url using evr.cm url shortener
        /// </summary>
        [JsonProperty("jpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string JpegUrl { get; set; }

        /// <summary>
        /// (optional) Camera username
        /// </summary>
        [JsonProperty("cam_username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        /// <summary>
        /// (optional) Camera password
        /// </summary>
        [JsonProperty("cam_password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }
    }

    public class CameraTestInfo
    {
        /// <summary>
        /// External camera url
        /// </summary>
        [JsonProperty("external_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalUrl { get; set; }

        /// <summary>
        /// Snapshot url
        /// </summary>
        [JsonProperty("jpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string JpgUrl { get; set; }

        /// <summary>
        /// (optional) Camera username
        /// </summary>
        [JsonProperty("cam_username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        /// <summary>
        /// (optional) Camera password
        /// </summary>
        [JsonProperty("cam_password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }
    }

    public class LiveImage
    {
        [JsonProperty("camera", NullValueHandling = NullValueHandling.Ignore)]
        public string Camera { get; set; }

        /// <summary>
        /// Name of the IANA/tz timezone where this camera is located
        /// </summary>
        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }

        /// <summary>
        /// Unix timestamp at creation
        /// </summary>
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public long CreatedAt { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }
    }
    
    public class Location
    {
        [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        public double Latitude;

        [JsonProperty("lng", NullValueHandling = NullValueHandling.Ignore)]
        public double Longitude;
    }
}