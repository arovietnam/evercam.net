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
            Evercam evercam = new Evercam("api_id", "api_key");
            CameraInfo info = new CameraInfo() { 
                ID = "test",
                Name = "Test Camera",
                Username = "user",
                Password = "12345",
                IsPublic = false,
                ExternalHost = "100.100.100.100",
                InternalHost = "192.168.1.100",
                ExternalHttpPort = "8080",
                InternalHttpPort = "80",
                JpegUrl = "snapshots/image.jpg"
            };
            Camera camera = evercam.CreateCamera(info);

            var cams = evercam.GetCameras("joeyb");
        }
    }
}
