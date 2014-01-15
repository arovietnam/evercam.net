using System;
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
        public void CreateTest()
        {
            Camera c = new Camera()
            {
                ID = "testcamera",
                Owner = "joeyb",
                Is_Public = true,
                Endpoints = new List<string> { "http://127.0.0.1:8080" },
                Snapshots = new Snapshots() { Jpg = "/onvif/snapshot" },
                Auth = new Auth(new Basic("admin", "12345"))
            };
            Camera camera = c.Create(new Auth(new Basic("shakeelanjum", "asdf1234")));
            Assert.IsNotNull(camera.ID);
            Assert.AreEqual("testcamera", camera.ID);
        }

        [TestMethod()]
        public void GetTest()
        {
            Camera camera = Camera.Get("testcamera", new Auth(new Basic("shakeelanjum", "asdf1234")));
            Assert.IsNotNull(camera);
            Assert.AreEqual("testcamera", camera.ID);
            Assert.AreEqual("joeyb", camera.Owner);
            Assert.AreEqual(3, camera.Endpoints.Count);
            Assert.IsTrue(camera.Is_Public);

            camera = Camera.Get("notestcamera", new Auth(new Basic("shakeelanjum", "asdf1234")));
            Assert.IsNull(camera);
        }
    }
}
