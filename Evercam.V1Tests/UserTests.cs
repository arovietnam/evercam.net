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
    public class UserTests
    {
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void CreateTest()
        {
            API.SANDBOX = false;
            User userInfo = new User()
            {
                UserName = "joeyb",
                ForeName = "Joey",
                LastName = "Bloggs",
                Email = "joe.bloggs@example.org",
                Country = "us"
            };
            User user = userInfo.Create();
            Assert.AreEqual("joeyb", user.ID);

            userInfo = new User()
            {
                UserName = "fail",
                ForeName = "Joey",
                LastName = "Bloggs",
                Email = "joe.bloggs@example.org",
                Country = "us"
            };
            user = userInfo.Create();
        }

        [TestMethod()]
        public void GetAllCamerasTest()
        {
            // Public Access: Get all public cameras owned by "joeyb"
            List<Camera> cameras = User.GetAllCameras("joeyb");
            Assert.AreEqual(1, cameras.Count);
            Assert.AreEqual("joeyb", cameras[0].Owner);
            Assert.AreEqual("my-camera-name", cameras[0].ID);

            // Protected Access: Get all public cameras owned by "joeyb" 
            // plus his cameras shared with "shakeelanjum"
            cameras = User.GetAllCameras("joeyb", new Auth("shakeelanjum", "asdf1234"));
            Assert.AreEqual(1, cameras.Count);
            Assert.AreEqual("joeyb", cameras[0].Owner);
            Assert.AreEqual("my-camera-name", cameras[0].ID);
        }
    }
}
