using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Evercam.V1;

namespace UnitTest
{
    /// <summary>
    /// Testing using mockserver: https://github.com/evercam/tools/blob/master/mockserver/mockserver.js
    /// </summary>
    [TestClass]
    public class VendorTest
    {
        [TestMethod]
        public void TestGetAll()
        {
            List<Vendor> vendors = Vendor.GetAll();
            Assert.AreEqual(2, vendors.Count);
        }

        [TestMethod]
        public void TestGetAllByMac()
        {
            List<Vendor> vendors = Vendor.GetAllByMac("00:73:57");
            Assert.AreEqual(1, vendors.Count);
            Assert.IsTrue(vendors[0].Known_Macs.Contains("00:73:57"));

            vendors = Vendor.GetAllByMac("nonExistentMac");
            Assert.AreEqual(0, vendors.Count);
        }

        [TestMethod]
        public void TestGetAllByName()
        {
            Vendor vendor = new Vendor("TP-Link Technologies");
            Assert.AreEqual("TP-Link Technologies", vendor.Name);
            Assert.IsNotNull(vendor.ID);

            vendor = new Vendor("nonExistentVendorName");
            Assert.IsNull(vendor.ID);
        }

        [TestMethod]
        public void TestGetFirmware()
        {
            List<Vendor> vendors = Vendor.GetAllByMac("00:73:57");
            Assert.AreEqual(1, vendors.Count);
            Assert.AreEqual(1, vendors[0].Firmwares.Count);
            Assert.IsNotNull(vendors[0].GetFirmware("*"));
            Assert.AreEqual("*", vendors[0].GetFirmware("*").Name);
            Assert.AreEqual("root", vendors[0].GetFirmware("*").Auth.Basic.UserName);
            Assert.AreEqual("pass", vendors[0].GetFirmware("*").Auth.Basic.Password);
        }
    }
}
