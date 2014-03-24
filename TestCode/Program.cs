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
            Evercam evercam = new Evercam(new EvercamClient("c298268b", "600736690c93646b89e3230480978629", "http://1button.info/"));
            evercam = new Evercam("sajjadkhan", "asdf");
            CameraInfo c = new CameraInfo()
            {
                ID = "harbourtest",
                Name = "Sajjad Office",
                IsPublic = true,
                ExternalUrl = "http://pp9.no-ip.org:8001",
                JpgUrl = "/Streaming/channels/1/picture",
                Username = "user",
                Password = "user"
            };
            evercam.UpdateCamera(c);
        }
    }
}
