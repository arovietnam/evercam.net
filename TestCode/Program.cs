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
                ID = "hikvisiontest",
                Name = "Test Hikvision",
                Owner = "shakeelanjum",
                IsPublic = true,
                Timezone = "Asia/Karachi",
                Vendor = "Hikvision",
                Endpoints = new List<string> { "http://192.168.0.1:80" },
                Snapshots = new Snapshots() { Jpg = "/jpg/image.jpg" },
                Auth = new Auth(new Basic("admin", "12345"))
            };

            Evercam evercam = new Evercam("shakeelanjum", "asdf1234");
            evercam.CreateCamera(c);
            c = evercam.GetCamera("hikvision");
            var data = c.GetLiveImage();
            evercam.DeleteCamera(c.ID);
            List<Camera> cameras = evercam.GetCameras("shakeelanjum");
        }
    }
}
