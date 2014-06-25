using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvercamV1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvercamV1.Tests
{
    [TestClass()]
    public class EvercamTests
    {
        Evercam evercam = new Evercam("shakeelanjum", "asdf1234");

        [TestMethod()]
        public void EvercamTest1()
        {
            Evercam ec = new Evercam("shakeelanjum", "asdf1234");
            Assert.AreEqual(ec.Auth.Basic.UserName, "shakeelanjum");
            Assert.AreEqual(ec.Auth.Basic.Password, "asdf1234");
        }

        [TestMethod()]
        public void EvercamTest2()
        {
            Evercam ec = new Evercam("48a2a289271689277fcfad44a0b1c86f");
            Assert.AreEqual(ec.Auth.OAuth2.AccessToken, "48a2a289271689277fcfad44a0b1c86f");
            Assert.AreEqual(ec.Auth.OAuth2.TokenType, "bearer");
        }

        [TestMethod()]
        public void GetVendorTest()
        {
            Vendor vendor = evercam.GetVendorsByName("TP-Link Technologies")[0];
            Assert.AreEqual("tplink", vendor.ID);
        }

        [TestMethod()]
        public void GetVendorsTest()
        {
            List<Vendor> vendors = evercam.GetAllVendors();
            Assert.AreEqual(323, vendors.Count);
        }

        [TestMethod()]
        public void GetVendorsByNameTest()
        {
            List<Vendor> vendors = evercam.GetVendorsByName("TP-Link Technologies");
            Assert.IsNotNull(vendors[0]);
            Assert.AreEqual("TP-Link Technologies", vendors[0].Name);
            
            vendors = evercam.GetVendorsByName("nonExistentVendorName");
            Assert.AreEqual(0, vendors.Count);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod()]
        public void GetVendorsByMacTest()
        {
            List<Vendor> vendors = evercam.GetVendorsByMac("54:E6:FC");
            Assert.AreEqual(1, vendors.Count);
            Assert.AreEqual("tplink", vendors[0].ID);
            Assert.IsTrue(vendors[0].KnownMacs.Contains("54:E6:FC"));

            vendors = evercam.GetVendorsByMac("nonExistentMac");
            Assert.AreEqual(0, vendors.Count);
        }

        [TestMethod()]
        public void GetAllVendorsTest()
        {
            List<Vendor> vendors = evercam.GetAllVendors();
            Assert.AreEqual(323, vendors.Count);
        }

        [TestMethod()]
        public void GetVendorsByIdTest()
        {
            List<Vendor> vendors = evercam.GetVendorsById("tplink");
            Assert.AreEqual(1, vendors.Count);
            Assert.AreEqual("tplink", vendors[0].ID);
        }

        [TestMethod()]
        public void GetModelTest()
        {
            Model model = evercam.GetModel("tplink", "*");
            Assert.AreEqual("*", model.Name);
        }

        [TestMethod()]
        public void GetCamerasTest()
        {
            List<Camera> cameras = evercam.GetCameras("shakeelanjum");
            Assert.AreEqual(14, cameras.Count);
        }

        [TestMethod()]
        public void GetUserTest()
        {
            Evercam ec = new Evercam("m.shakeel.anjum", "asdf1234");
            User user = ec.GetUser("m.shakeel.anjum");
            Assert.AreEqual("m.shakeel.anjum@hotmail.com", user.Email);
        }

        [TestMethod()]
        public void CreateUserTest()
        {
            User u = new User()
            {
                FirstName = "Shakeel",
                LastName = "Anjum",
                Email = "m.shakeel.anjum@hotmail.com",
                Country = "ie",
                UserName = "m.shakeel.anjum"
            };
            User result = evercam.CreateUser(u);
            Assert.IsNotNull(result);
            Assert.AreEqual("m.shakeel.anjum", result.ID);
            Assert.AreEqual("m.shakeel.anjum@hotmail.com", result.Email);
        }

        [TestMethod()]
        public void UpdateUserTest()
        {
            UserInfo u = new UserInfo()
            {
                ID = "m.shakeel.anjum",
                Country = "pk",
            };
            Evercam ec = new Evercam("m.shakeel.anjum", "asdf1234");
            User result = ec.UpdateUser(u);
            Assert.IsNotNull(result);
            Assert.AreEqual("m.shakeel.anjum", result.ID);
            Assert.AreEqual("pk", result.Country);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod()]
        public void DeleteUserTest()
        {
            string result = evercam.DeleteUser("m.shakeel.anjum");
            User user = evercam.GetUser("m.shakeel.anjum");
            Assert.IsNull(user);
        }

        [TestMethod()]
        public void GetUserRightsTest()
        {
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod()]
        public void GetCameraTest()
        {
            Camera camera = evercam.GetCamera("usertest");
            Assert.IsNotNull(camera);
            Assert.AreEqual("usertest", camera.ID);
            Assert.AreEqual("shakeelanjum", camera.Owner);
            Assert.IsTrue(camera.IsPublic);

            camera = evercam.GetCamera("noharbour");
            Assert.IsNull(camera);
        }

        [TestMethod()]
        public void CreateCameraTest()
        {
            CameraInfo c = new CameraInfo()
            {
                ID = "usertest",
                Name = "Camera Test",
                IsPublic = false,
                ExternalHost = "http://192.168.0.1:80",
                JpegUrl = "/jpg/image.jpg",
                Username = "admin",
                Password = "12345"
            };
            Camera result = evercam.CreateCamera(c);
            Assert.IsNotNull(result);
            Assert.AreEqual("usertest", result.ID);
            Assert.AreEqual("shakeelanjum", result.Owner);
        }

        [TestMethod()]
        public void UpdateCameraTest()
        {
            CameraInfo c = new CameraInfo()
            {
                ID = "usertest",
                Name = "User Test Camera",
                IsPublic = true
            };
            Camera result = evercam.UpdateCamera(c);
            Assert.IsNotNull(result);
            Assert.AreEqual("usertest", result.ID);
            Assert.AreEqual("User Test Camera", result.Name);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod()]
        public void DeleteCameraTest()
        {
            string result = evercam.DeleteCamera("usertest");
            Assert.AreEqual("", result);

            Camera camera = evercam.GetCamera("usertest");
            Assert.IsNull(camera);
        }

        [TestMethod()]
        public void GetSnapshotsTest()
        {
        }

        [TestMethod()]
        public void GetSnapshotsTest1()
        {
        }

        [TestMethod()]
        public void CreateSnapshotTest()
        {
        }

        [TestMethod()]
        public void UpdateSnapshotTest()
        {
        }

        [TestMethod()]
        public void DeleteSnapshotTest()
        {
        }
    }
}
