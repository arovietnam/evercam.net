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
        [JsonProperty("vendor_id", NullValueHandling = NullValueHandling.Ignore)]
        public string VendorID { get; set; }

        /// <summary>
        /// (optional) Unique identifier for the camera vendor
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
        /// (optional) External host of the camera
        /// </summary>
        [JsonProperty("external", NullValueHandling = NullValueHandling.Ignore)]
        public URL External { get; set; }

        /// <summary>
        /// (optional) Internal host of the camera
        /// </summary>
        [JsonProperty("internal", NullValueHandling = NullValueHandling.Ignore)]
        public URL Internal { get; set; }

        /// <summary>
        /// (optional) DynDns of the camera
        /// </summary>
        [JsonProperty("dyndns", NullValueHandling = NullValueHandling.Ignore)]
        public URL DynDns { get; set; }

        /// <summary>
        /// (optional) Proxy Url of the camera
        /// </summary>
        [JsonProperty("proxy_url", NullValueHandling = NullValueHandling.Ignore)]
        public URL Proxy { get; set; }

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
                External = this.External,
                Internal = this.Internal,
                Proxy = this.Proxy
            };
        }
    }

    public class URL
    {
        [JsonProperty("host", NullValueHandling = NullValueHandling.Ignore)]
        public string Host { get; set; }

        [JsonProperty("http", NullValueHandling = NullValueHandling.Ignore)]
        public Http Http { get; set; }

        [JsonProperty("rtsp", NullValueHandling = NullValueHandling.Ignore)]
        public Rtsp Rtsp { get; set; }
    }

    public class Http
    {
        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        public int Port { get; set; }

        [JsonProperty("camera", NullValueHandling = NullValueHandling.Ignore)]
        public string Camera { get; set; }

        [JsonProperty("jpg", NullValueHandling = NullValueHandling.Ignore)]
        public string Jpg { get; set; }

        [JsonProperty("mjpg", NullValueHandling = NullValueHandling.Ignore)]
        public string Mjpg { get; set; }
    }

    public class Rtsp
    {
        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        public int Port { get; set; }

        [JsonProperty("mpeg", NullValueHandling = NullValueHandling.Ignore)]
        public string Mpeg { get; set; }

        [JsonProperty("audio", NullValueHandling = NullValueHandling.Ignore)]
        public string Audio { get; set; }

        [JsonProperty("h264", NullValueHandling = NullValueHandling.Ignore)]
        public string H264 { get; set; }
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
        [JsonProperty("external", NullValueHandling = NullValueHandling.Ignore)]
        public URL External { get; set; }

        /// <summary>
        /// (optional) Internal host of the camera
        /// </summary>
        [JsonProperty("internal", NullValueHandling = NullValueHandling.Ignore)]
        public URL Internal { get; set; }

        /// <summary>
        /// (optional) DynDns host of the camera
        /// </summary>
        [JsonProperty("dyndns", NullValueHandling = NullValueHandling.Ignore)]
        public URL DynDns { get; set; }

        /// <summary>
        /// (optional) Proxy Url of the camera
        /// </summary>
        [JsonProperty("proxy_url", NullValueHandling = NullValueHandling.Ignore)]
        public URL Proxy { get; set; }

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

        /// <summary>
        /// Base64 encoded data string
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }

        /// <summary>
        /// Converts base64 data string to bytes array
        /// </summary>
        /// <returns>byte[]</returns>
        public byte[] ToBytes()
        {
            return Utility.ToBytes(this.Data);
        }
    }
    
    public class Location
    {
        [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        public double Latitude;

        [JsonProperty("lng", NullValueHandling = NullValueHandling.Ignore)]
        public double Longitude;
    }
}