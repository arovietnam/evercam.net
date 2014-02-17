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
            Evercam evercam = new Evercam("joeyb", "1234");
            Camera c = new Camera()
            {
                ID = "hikvision",
                Name = "Hikvision",
                Owner = "joeyb",
                IsPublic = false,
                Timezone = "Etc/UTC",
                Vendor = "Hikvision",
                Endpoints = new List<string> { "http://somedydns.com:8080" },
                Snapshots = new Snapshots() { Jpg = "/Streaming/picture" },
                Auth = new Auth(new Basic("admin", "admin"))
            };
            c = evercam.CreateCamera(c);
            c = evercam.GetCamera("hikvision");
            var data = c.GetLiveImage();
            evercam.DeleteCamera(c.ID);
            List<Camera> cameras = evercam.GetCameras("joeyb");
        }
    }
}
