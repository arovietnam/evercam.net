using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evercam.V1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Evercam.V1.Tests
{
    [TestClass()]
    public class CameraTests
    {
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void CreateTest()
        {
            API.SANDBOX = true;
            Camera c = new Camera()
            {
                ID = "testcamera",
                Owner = "joeyb",
                IsPublic = true,
                Endpoints = new List<string> { "http://127.0.0.1:8080" },
                Snapshots = new Snapshots() { Jpg = "/onvif/snapshot" },
                Auth = new Auth(new Basic("admin", "12345"))
            };
            Camera camera = c.Create(new Auth(new Basic("shakeelanjum", "asdf1234")), AuthMode.Basic);
            Assert.IsNotNull(camera.ID);
            Assert.AreEqual("testcamera", camera.ID);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void GetTest()
        {
            API.SANDBOX = true;
            Camera camera = Camera.Get("testcamera");
            Assert.IsNotNull(camera);
            Assert.AreEqual("testcamera", camera.ID);
            Assert.AreEqual("joeyb", camera.Owner);
            Assert.AreEqual(3, camera.Endpoints.Count);
            Assert.IsTrue(camera.IsPublic);

            camera = Camera.Get("notestcamera", new Auth(new Basic("shakeelanjum", "asdf1234")), AuthMode.Basic);
            Assert.IsNull(camera);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void GetLiveImageTest()
        {
            API.SANDBOX = true;
            Camera camera = Camera.Get("testcamera", new Auth(new Basic("shakeelanjum", "asdf1234")), AuthMode.Basic);

            // get public image with default endpoint [0]
            byte[] data = camera.GetLiveImage();
            Assert.AreEqual(105708, data.Length);

            // get image from invalid at endpoint [1]
            data = camera.GetLiveImage(camera.Endpoints[1] + camera.Snapshots.Jpg);
            Assert.AreEqual(0, data.Length);

            // get protected image from endpoint [2]
            data = camera.GetLiveImage(camera.Endpoints[2] + camera.Snapshots.Jpg);
            Assert.AreEqual(105708, data.Length);
        }
    }
}
