using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Threading.Tasks;

using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EvercamV1
{
    public class Camera
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("owner", NullValueHandling = NullValueHandling.Ignore)]
        public string Owner { get; set; }

        [JsonProperty("vendor", NullValueHandling = NullValueHandling.Ignore)]
        public string Vendor { get; set; }

        [JsonProperty("model", NullValueHandling = NullValueHandling.Ignore)]
        public string Model { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public long CreatedAt { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public long UpdatedAt { get; set; }

        [JsonProperty("last_polled_at", NullValueHandling = NullValueHandling.Ignore)]
        public long LastPolledAt { get; set; }

        [JsonProperty("last_online_at", NullValueHandling = NullValueHandling.Ignore)]
        public long LastOnlineAt { get; set; }

        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string Timezone { get; set; }

        [JsonProperty("is_online", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsOnline { get; set; }
        
        [JsonProperty("is_public", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPublic { get; set; }

        [JsonProperty("external_host", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalHost { get; set; }

        [JsonProperty("internal_host", NullValueHandling = NullValueHandling.Ignore)]
        public string InternalHost { get; set; }

        [JsonProperty("external_http_port", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalHttpPort { get; set; }

        [JsonProperty("internal_http_port", NullValueHandling = NullValueHandling.Ignore)]
        public string InternalHttpPort { get; set; }

        [JsonProperty("external_rtsp_port", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalRtspPort { get; set; }

        [JsonProperty("internal_rtsp_port", NullValueHandling = NullValueHandling.Ignore)]
        public string InternalRtspPort { get; set; }

        [JsonProperty("jpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string JpegUrl { get; set; }

        [JsonProperty("cam_username", NullValueHandling = NullValueHandling.Ignore)]
        public string CameraUsername { get; set; }

        [JsonProperty("cam_password", NullValueHandling = NullValueHandling.Ignore)]
        public string CameraPassword { get; set; }

        [JsonProperty("mac_address", NullValueHandling = NullValueHandling.Ignore)]
        public string MacAddress { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public Location Location;

        public byte[] GetLiveImage()
        {
            byte[] data = new byte[] { };
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API.CAMERA_PROTOCOL + ExternalHost + ":" + ExternalHttpPort + JpegUrl);
            request.KeepAlive = false;
            request.Credentials = new NetworkCredential(CameraUsername, CameraPassword);

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (MemoryStream ms = new MemoryStream(60000))
                    {
                        if (response.ContentType.Contains("image") && stream != null)
                        {
                            stream.CopyTo(ms);
                            data = ms.ToArray();
                        }
                    }
                }
            }
            return data;
        }
    }

    public class CameraInfo
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty("external_host", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalHost { get; set; }

        [JsonProperty("internal_host", NullValueHandling = NullValueHandling.Ignore)]
        public string InternalHost { get; set; }

        [JsonProperty("external_http_port", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalHttpPort { get; set; }

        [JsonProperty("internal_http_port", NullValueHandling = NullValueHandling.Ignore)]
        public string InternalHttpPort { get; set; }

        [JsonProperty("jpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string JpegUrl { get; set; }

        [JsonProperty("cam_username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("cam_password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }
    }

    public class CameraTestInfo
    {
        [JsonProperty("external_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalUrl { get; set; }

        [JsonProperty("jpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string JpgUrl { get; set; }

        [JsonProperty("cam_username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("cam_password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }
    }

    public class CameraShare
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("camera_id")]
        public string CameraID { get; set; }

        [JsonProperty("external_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalUrl { get; set; }

        [JsonProperty("jpg_url", NullValueHandling = NullValueHandling.Ignore)]
        public string JpgUrl { get; set; }

        [JsonProperty("cam_username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("cam_password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }
    }
    
    public class Location
    {
        [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        public double Latitude;

        [JsonProperty("lng", NullValueHandling = NullValueHandling.Ignore)]
        public double Longitude;
    }
}
