using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Evercam.V1;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Camera c = new Camera()
            {
                ID = "hik-test",
                Name = "Test Hikvision",
                //Owner = "shakeelanjum",
                IsPublic = true,
                //Timezone = "Asia/Karachi",
                //Vendor = "Hikvision",
                Endpoints = new List<string> { "http://192.168.0.1:80" },
                //Snapshots = new Snapshots() { Jpg = "/jpg/image.jpg" },
                //Auth = new Auth(new Basic("admin", "12345"))
            };
            //Camera camera = c.Update(new Auth(new OAuth2("48a2a289271689277fcfad44a0b1c86f")), AuthMode.OAuth2);
            Camera camera = c.Update(new Auth(new Basic("shakeelanjum", "asdf1234")), AuthMode.Basic);

            // Get list of cameras of a user (with given user id)
            // GET /v1/users/{id}/cameras
            // var cameras = User.GetAllCameras("joeyb");
            var cameras = User.GetAllCameras("shakeelanjum", new Auth(new Basic("shakeelanjum", "asdf1234")), AuthMode.Basic);

            // Get details of public 'testcamera'
            var cam = Camera.Get("testcamera");
            // Get live image data of 'testcamera' using its endpoint, jpg url and auth info (if exists)
            var data = cam.GetLiveImage();
            //var data = cam.GetLiveImage(cam.Endpoints[2] + cam.Snapshots.Jpg);

            // Get list of all camera vendors
            // GET /v1/vendors
            var all = Vendor.GetAll();

            // Get vendor by mac address
            // GET /v1/vendors/{mac}
            var byMac = Vendor.GetAllByMac("00:73:57");

            // Get vendor by mac address (for testing...)
            // GET /v1/vendors/{mac}
            var noMac = Vendor.GetAllByMac("nonExistentMac");

            // Get vendor object by name
            // NO direct API method found to GET vendor by name
            var vendor = new Vendor("Ubiquiti Networks");
            var u = vendor.GetFirmware("*").Auth.Basic.UserName;
            var p = vendor.GetFirmware("*").Auth.Basic.Password;

            // Get list of all camera vendors
            var allVendors = Model.GetAllVendors();

            // Get list of all camera vendors by id
            var allVendorsbyId = Model.GetAllVendorsById("testid");

            // Get list of all camera vendors by id
            var noVendorsId = Model.GetAllVendorsById("nonExistentId");

            // Get camera model by vendor and model id
            var model = Model.Get("testid", "YCW005");
            string username = model.Defaults.Auth.Basic.UserName;
            string password = model.Defaults.Auth.Basic.Password;
            string jpg = model.Defaults.Snapshots.Jpg;
        }
    }
}
