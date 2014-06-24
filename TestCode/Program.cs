using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Net.Http;
using System.IO;
using EvercamV1;
using System.Net;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Evercam evercam = new Evercam("c20efa43", "02dc67d564403903e1e6da1264e1ece8");
            //Evercam evercam = new Evercam("e8835f04c4203c3ec298268b", "34315a76d9e328a0ba36215f0f577dc955e2df6518ec146e0d968d640064017d600736690c93646b89e3230480978629");
            //Evercam evercam = new Evercam("ac45487a", "ef811c60ebba8b62ce11fec9c50bc51b");
            //Evercam evercam = new Evercam("906ed3ffcf6c845814e298ef0073ef42");

            //var u = evercam.GetUser("shakeelanjum");
            //CameraInfo info = new CameraInfo()
            //{
            //    ID = "fbadoffice",
            //    Name = "F'bad Office",
            //    Username = "mhlabs",
            //    Password = "123456",
            //    IsPublic = false,
            //    ExternalHost = "mhlabs.viewnetcam.com",
            //    InternalHost = "192.168.1.100",
            //    ExternalHttpPort = 8080,
            //    JpegUrl = "/SnapshotJPEG?Resolution=640x480"
            //};
            //Camera camera = evercam.CreateCamera(info);
            LiveImage image = evercam.GetLiveImage("window");
            System.IO.File.WriteAllBytes("e://testing.jpg", Utility.ToBytes(image.Data));
            LogMessages msgs = evercam.GetLogMessages("window", 0, 0, 10, 2, "");
            LogObjects objs = evercam.GetLogObjects("window", 0, 0, 10, 2, "");
            var cam = evercam.GetCamera("window");
            cam.ExternalHttpPort = 9000;
            cam.InternalHttpPort = 8000;
            cam = evercam.UpdateCamera(cam.GetInfo());
            
            //ShareInfo info = new ShareInfo()
            //{
            //    CameraID = "fsdtestcamera",
            //    Email = "azhar.malik@mhlabs.net",
            //    Rights = "view"
            //};
            //Share share = evercam.CreateCameraShare(info);
            //Share share = evercam.UpdateCameraShare(947, "view,list");
            //evercam.DeleteCameraShare("fsdtestcamera", 944);
            //List<Share> shares = evercam.GetCameraShares("fsdtestcamera");
            //List<Share> ushares = evercam.GetUserShares("azharmalik3");


            ShareRequest info = new ShareRequest()
            {
                CameraID = "reykjavikharbour",
                Email = "shakeel.anjum@camba.tv",
                Rights = "view,edit,delete,list",
                UserID = "azharmalik3"
            };
            //ShareRequest request = evercam.CreateCameraShareRequest(info);
            ShareRequest request = evercam.UpdateCameraShareRequest("testcam", "view,list");
            evercam.DeleteCameraShareRequest("testcam", "alicek@email.com");
            List<ShareRequest> requests = evercam.GetCameraShareRequests("testcam", "");
        }
    }
}
