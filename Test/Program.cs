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
            // Get list of all camera vendors
            var all = Vendor.GetAll();

            // Get vendor by mac address
            var byMac = Vendor.GetAllByMac("00:73:57");

            // Get vendor by mac address (for testing...)
            var noMac = Vendor.GetAllByMac("nonExistentMac");

            // Get vendor object by mac
            var v = new Vendor("Ubiquiti Networks");
            var un = v.GetFirmware("*").Auth.Basic.UserName;

            // Get list of all camera vendors
            var allVendors = Model.GetAllVendors();

            // Get list of all camera vendors by id
            var allVendorsbyId = Model.GetAllVendorsById("testid");

            // Get list of all camera vendors by id
            var noVendorsId = Model.GetAllVendorsById("nonExistentId");

            // Get camera model by vendor and model id
            var model = Model.Get("testid", "YCW005");
        }
    }
}
