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
            //Evercam evercam = new Evercam(new EvercamClient("c298268b", "600736690c93646b89e3230480978629", "http://1button.info/"));            
            Evercam evercam = new Evercam(new EvercamClient("c4203c3e", "55e2df6518ec146e0d968d640064017d", "http://astimegoes.by/"));
            //Evercam evercam = new Evercam();
            //evercam.Auth = new Auth(new OAuth2("d65788e0bf4b9446ba9777d9d1e5c5f4"));
            var cams = evercam.GetCameras("azharmalik3");
            var cam = evercam.GetCamera("azharmalik321827");
            var i = cam.GetLiveImage();

            //evercam = new Evercam("azharmalik3", "4240");
            CameraInfo c = new CameraInfo()
            {
                ID = "hikvisiontest",
                Name = "Head Office",
                IsPublic = true,
                ExternalHost = "http://89.101.225.158",
                ExternalHttpPort = "8105",
                JpegUrl = "/Streaming/channels/1/picture",
                Username = "admin",
                Password = "mehcam"
            };
            evercam.UpdateCamera(c);
            
            //var cams = evercam.GetAllVendors();
            //Console.WriteLine(cams.Count);
        }
    }
}
