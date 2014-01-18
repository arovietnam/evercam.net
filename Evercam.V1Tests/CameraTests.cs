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
            Camera c = new Camera()
            {
                ID = "testcamera",
                Owner = "joeyb",
                IsPublic = true,
                Endpoints = new List<string> { "http://127.0.0.1:8080" },
                Snapshots = new Snapshots() { Jpg = "/onvif/snapshot" },
                Auth = new Auth("admin", "12345")
            };
            Camera camera = c.Create(new Auth("shakeelanjum", "asdf1234"));
            Assert.IsNotNull(camera.ID);
            Assert.AreEqual("testcamera", camera.ID);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void GetTest()
        {
            Camera camera = Camera.Get("testcamera", new Auth("shakeelanjum", "asdf1234"));
            Assert.IsNotNull(camera);
            Assert.AreEqual("testcamera", camera.ID);
            Assert.AreEqual("joeyb", camera.Owner);
            Assert.AreEqual(3, camera.Endpoints.Count);
            Assert.IsTrue(camera.IsPublic);

            // get public image NO Auth
            MemoryStream data = camera.GetLiveImage(camera.Endpoints[0] + camera.Snapshots.Jpg, false);
            Assert.AreEqual(105708, data.Length);   // valid image contents
            
            // get protected image WITHOUT Auth
            data = camera.GetLiveImage(camera.Endpoints[2] + camera.Snapshots.Jpg, false);
            Assert.AreEqual(0, data.Length);
            
            // get protected image WITH Auth
            data = camera.GetLiveImage(camera.Endpoints[2] + camera.Snapshots.Jpg, true);
            Assert.AreEqual(105708, data.Length);   // valid image contents

            camera = Camera.Get("notestcamera", new Auth("shakeelanjum", "asdf1234"));
            Assert.IsNull(camera);
        }
    }
}
