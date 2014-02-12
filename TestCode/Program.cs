using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using EvercamV1;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Camera c = new Camera()
            {
                ID = "test",
                Name = "Test Camera",
                Owner = "joeyb",
                IsPublic = true,
                Timezone = "Asia/Karachi",
                Vendor = "TP-Link",
                Endpoints = new List<string> { "http://123.123.123.123:8080" },
                Snapshots = new Snapshots() { Jpg = "/jpg/image.jpg" },
                Auth = new Auth(new Basic("admin", "admin"))
            };

            Evercam evercam = new Evercam("joeyb_access_token");
            evercam.CreateCamera(c);
            c = evercam.GetCamera("test");
            var data = c.GetLiveImage();
            evercam.DeleteCamera(c.ID);
            List<Camera> cameras = evercam.GetCameras("joeyb");
        }
    }
}
