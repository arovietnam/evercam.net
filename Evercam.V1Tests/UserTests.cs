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
        public void CreateTest()
        {
            User user1 = new User()
            {
                UserName = "joeyb",
                ForeName = "Joe",
                LastName = "Bloggs",
                Email = "joe.bloggs@example.org",
                Country = "us"
            };

            User newUser = user1.Create();
            Assert.AreEqual("joeyb", newUser.ID);

            User user2 = new User()
            {
                UserName = "fail",
                ForeName = "Joe",
                LastName = "Bloggs",
                Email = "joe.bloggs@example.org",
                Country = "us"
            };

            newUser = user2.Create();
            Assert.IsNull(newUser.ID);
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
            cameras = User.GetAllCameras("joeyb", new Auth(new Basic("shakeelanjum", "asdf1234")));
            Assert.AreEqual(1, cameras.Count);
            Assert.AreEqual("joeyb", cameras[0].Owner);
            Assert.AreEqual("my-camera-name", cameras[0].ID);
        }
    }
}
