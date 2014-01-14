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
            // Basic Authentication
            var auth = Auth.GetBasic("shakeelanjum", "asdf1234");

            // Create new user
            // POST /v1/users
            User user = new User();
            user.ForeName = "shakeel";
            user.LastName = "anjum";
            user.Email = "shakeel.anjum@camba.tv";
            user.UserName = "shakeelanjum";
            user.Country = "pk";
            var newUser = User.Create(user);

            // Get list of cameras of a user (with given user id)
            // GET /v1/users/{id}/cameras
            var cameras = User.GetAllCameras("shakeelanjum");

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
