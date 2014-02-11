using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Evercam.V1;
using System.Web.Script.Serialization;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Camera c = new Camera()
            {
                ID = "hikvis-demo2",
                Name="hikvis 2",
                Owner = "azharmalik3",
                IsPublic = true,
                Endpoints = new List<string> { "http://azharmalik3.no-ip.biz:8084" },
                Snapshots = new Snapshots() { Jpg = "/snapshot.jpg" },
                Auth = new Auth(new Basic("mhlabs", "12345"))
            };
            Camera camera = c.Create(new Auth(new Basic("azharmalik3", "4240")),AuthMode.Basic);
            /*var str = @"{
  ""cameras"": [
    {
      ""id"": ""hikvis-demo"",
      ""name"": ""hikvis 2"",
      ""owner"": ""azharmalik3"",
      ""vendor"": null,
      ""model"": null,
      ""created_at"": 1392122046,
      ""updated_at"": 1392122046,
      ""last_polled_at"": null,
      ""last_online_at"": null,
      ""timezone"": ""Etc/UTC"",
      ""is_online"": null,
      ""is_public"": true,
      ""location"": null,
      ""endpoints"": [
        ""http://azharmalik3.no-ip.biz:8084""
      ],
      ""mac_address"": null,
      ""snapshots"": {
        ""jpg"": ""/snapshot.jpg""
      },
      ""auth"": {
        ""basic"": {
          ""username"": ""mhlabs"",
          ""password"": ""12345""
        }
      }
    }
  ]
}";*/
            //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            //var c = json_serializer.Deserialize<object>(str);
            //cam c = JavaScriptSerializer.Deserialize<cam>(str); //(cam)json_serializer.DeserializeObject(str);
            //var c = JObject.Parse(str)["cameras"].ToObject<cam>();//JsonConvert.DeserializeObject<object>(str);
            

            // Get list of cameras of a user (with given user id)
            // GET /v1/users/{id}/cameras
            // var cameras = User.GetAllCameras("joeyb");
            //var cameras = User.GetAllCameras("shakeelanjum", new Auth(new Basic("shakeelanjum", "asdf1234")), AuthMode.Basic);

            // Get details of public 'testcamera'
            //var cam = Camera.Get("testcamera");
            // Get live image data of 'testcamera' using its endpoint, jpg url and auth info (if exists)
            //var data = cam.GetLiveImage();
            //var data = cam.GetLiveImage(cam.Endpoints[2] + cam.Snapshots.Jpg);

            // Get list of all camera vendors
            // GET /v1/vendors
            //var all = Vendor.GetAll();
            Console.WriteLine(camera.ID);
            // Get vendor by mac address
            // GET /v1/vendors/{mac}
            //var byMac = Vendor.GetAllByMac("00:73:57");

            // Get vendor by mac address (for testing...)
            // GET /v1/vendors/{mac}
            //var noMac = Vendor.GetAllByMac("nonExistentMac");

            // Get vendor object by name
            // NO direct API method found to GET vendor by name
            //var vendor = new Vendor("Ubiquiti Networks");
            //var u = vendor.GetFirmware("*").Auth.Basic.UserName;
            //var p = vendor.GetFirmware("*").Auth.Basic.Password;

            // Get list of all camera vendors
            //var allVendors = Model.GetAllVendors();

            // Get list of all camera vendors by id
            //var allVendorsbyId = Model.GetAllVendorsById("testid");

            // Get list of all camera vendors by id
            //var noVendorsId = Model.GetAllVendorsById("nonExistentId");

            // Get camera model by vendor and model id
            //var model = Model.Get("testid", "YCW005");
            //string username = model.Defaults.Auth.Basic.UserName;
            //string password = model.Defaults.Auth.Basic.Password;
            //string jpg = model.Defaults.Snapshots.Jpg;
        }
    }

    class cam
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("vendor")]
        public string Vendor { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }

        [JsonProperty("last_polled_at")]
        public long LastPolledAt { get; set; }

        [JsonProperty("last_online_at")]
        public long LastOnlineAt { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("mac_address")]
        public string MacAddress { get; set; }

        [JsonProperty("endpoints")]
        public List<string> Endpoints { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty("is_online")]
        public bool IsOnline { get; set; }

        [JsonProperty("location")]
        public Location Location;

        [JsonProperty("auth")]
        public Auth Auth;

        [JsonProperty("snapshots")]
        public Snapshots Snapshots;
    }
}
