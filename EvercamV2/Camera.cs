using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace EvercamV2
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
        /// Unique identifier for the camera model
        /// </summary>
        [JsonProperty("model_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ModelID { get; set; }

        /// <summary>
        /// Name of the camera model
        /// </summary>
        [JsonProperty("model_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ModelName { get; set; }

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
        public Proxy ProxyUrl { get; set; }

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

        /// <summary>
        /// thumbnail preview of camera view
        /// </summary>
        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public string Thumbnail { get; set; }

        /// <summary>
        /// camera thumbnail image url
        /// </summary>
        [JsonProperty("thumbnail_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ThumbnailUrl { get; set; }

        public CameraInfo GetInfo()
        {
            return new CameraInfo()
            {
                ID = this.ID,
                Name = this.Name,
                Vendor = this.VendorID,
                Model = this.ModelName,
                Timezone = this.Timezone,
                IsPublic = this.IsPublic,
                IsOnline = this.IsOnline,
                IsDiscoverable = this.IsDiscoverable,
                CameraUsername = this.CameraUsername,
                CameraPassword = this.CameraUsername,
                MacAddress = this.MacAddress,
                LocationLatitude = this.Location.Latitude,
                LocationLongitude = this.Location.Longitude,
                ExternalHost = this.External.Host,
                InternalHost = this.Internal.Host,
                ExternalHttpPort = this.External.Http.Port,
                InternalHttpPort = this.Internal.Http.Port,
                ExternalRtspPort = this.External.Rtsp.Port,
                InternalRtspPort = this.Internal.Rtsp.Port,
                JpgUrl = this.External.Http.Jpg,
                MjpgUrl = this.External.Http.Mjpg,
                MpegUrl = this.External.Rtsp.Mpeg,
                AudioUrl = this.External.Rtsp.Audio,
                H264Url = this.External.Rtsp.H264,
                ThumbnailUrl = this.ThumbnailUrl
            };
        }
    }

    public class PublicCamera
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
        /// Unique identifier for the camera model
        /// </summary>
        [JsonProperty("model_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ModelID { get; set; }

        /// <summary>
        /// Name of the camera model
        /// </summary>
        [JsonProperty("model_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ModelName { get; set; }

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
        /// (optional) Proxy Url of the camera
        /// </summary>
        [JsonProperty("proxy_url", NullValueHandling = NullValueHandling.Ignore)]
        public Proxy ProxyUrl { get; set; }

        /// <summary>
        /// 150x150 preview of camera view
        /// </summary>
        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public string Thumbnail { get; set; }

        /// <summary>
        /// Camera thumbnail image url
        /// </summary>
        [JsonProperty("thumbnail_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ThumbnailUrl { get; set; }
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

    public class Proxy
    {
        [JsonProperty("jpg", NullValueHandling = NullValueHandling.Ignore)]
        public string Jpg { get; set; }

        [JsonProperty("rtmp", NullValueHandling = NullValueHandling.Ignore)]
        public string Rtmp { get; set; }
    }

    public class Location
    {
        [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        public double Latitude;

        [JsonProperty("lng", NullValueHandling = NullValueHandling.Ignore)]
        public double Longitude;
    }

    public class CameraInfo
    {
        public CameraInfo() { }

        /// <param name="id">Unique Evercam identifier for the camera</param>
        public CameraInfo(string id) { ID = id; }

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
        /// (optional) Camera vendor id
        /// </summary>
        [JsonProperty("vendor", NullValueHandling = NullValueHandling.Ignore)]
        public string Vendor { get; set; }

        /// <summary>
        /// Camera model name
        /// </summary>
        [JsonProperty("model", NullValueHandling = NullValueHandling.Ignore)]
        public string Model { get; set; }

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
        /// (optional) Camera GPS latitude location
        /// </summary>
        [JsonProperty("location_lat", NullValueHandling = NullValueHandling.Ignore)]
        public double LocationLatitude;

        /// <summary>
        /// (optional) Camera GPS longitude  location
        /// </summary>
        [JsonProperty("location_lng", NullValueHandling = NullValueHandling.Ignore)]
        public double LocationLongitude;

        /// <summary>
        /// (optional) Whether the camera is publicly findable
        /// </summary>
        [JsonProperty("discoverable", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsDiscoverable { get; set; }

        /// <summary>
        /// (optional) External camera host
        /// </summary>
        [JsonProperty("external_host", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalHost { get; set; }

        /// <summary>
        /// (optional) Internal camera host
        /// </summary>
        [JsonProperty("internal_host", NullValueHandling = NullValueHandling.Ignore)]
        public string InternalHost { get; set; }

        /// <summary>
        /// (optional) External camera http port
        /// </summary>
        [JsonProperty("external_http_port", NullValueHandling = NullValueHandling.Ignore)]
        public int ExternalHttpPort { get; set; }

        /// <summary>
        /// (optional) Internal camera http port
        /// </summary>
        [JsonProperty("internal_http_port", NullValueHandling = NullValueHandling.Ignore)]
        public int InternalHttpPort { get; set; }

        /// <summary>
        /// (optional) External camera rtsp port
        /// </summary>
        [JsonProperty("external_rtsp_port", NullValueHandling = NullValueHandling.Ignore)]
        public int ExternalRtspPort { get; set; }

        /// <summary>
        /// (optional) Internal camera rtsp port
        /// </summary>
        [JsonProperty("internal_rtsp_port", NullValueHandling = NullValueHandling.Ignore)]
        public int InternalRtspPort { get; set; }

        /// <summary>
        /// (optional) Snapshot url
        /// </summary>
        [JsonProperty("jpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string JpgUrl { get; set; }

        /// <summary>
        /// (optional) Mjpg url using evr.cm dynamic DNS
        /// </summary>
        [JsonProperty("mjpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string MjpgUrl { get; set; }

        /// <summary>
        /// (optional) Dynamis DNS mpeg url
        /// </summary>
        [JsonProperty("mpeg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string MpegUrl { get; set; }

        /// <summary>
        /// (optional) Dynamis DNS audio url
        /// </summary>
        [JsonProperty("audio_url", NullValueHandling = NullValueHandling.Ignore)]
        public string AudioUrl { get; set; }

        /// <summary>
        /// (optional) Dynamis DNS h264 url
        /// </summary>
        [JsonProperty("h264_url", NullValueHandling = NullValueHandling.Ignore)]
        public string H264Url { get; set; }

        /// <summary>
        /// Camera thumbnail image url
        /// </summary>
        [JsonProperty("thumbnail_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ThumbnailUrl { get; set; }
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
}