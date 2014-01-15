using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evercam.V1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evercam.V1.Tests
{
    /// <summary>
    /// Testing using mockserver: https://github.com/evercam/tools/blob/master/mockserver/mockserver.js
    /// </summary>
    [TestClass()]
    public class VendorTests
    {
        [TestMethod()]
        public void VendorTest1()
        {
            Vendor vendor = new Vendor("TP-Link Technologies");
            Assert.AreEqual("tplink", vendor.ID);

            vendor = new Vendor("Axis");
            Assert.IsNull(vendor.ID);
        }

        [TestMethod()]
        public void GetFirmwareTest()
        {
            List<Vendor> vendors = Vendor.GetAllByMac("00:73:57");
            Assert.AreEqual(1, vendors.Count);
            Assert.AreEqual(1, vendors[0].Firmwares.Count);
            Assert.IsNotNull(vendors[0].GetFirmware("*"));
            Assert.AreEqual("*", vendors[0].GetFirmware("*").Name);
            Assert.AreEqual("root", vendors[0].GetFirmware("*").Auth.Basic.UserName);
            Assert.AreEqual("pass", vendors[0].GetFirmware("*").Auth.Basic.Password);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            List<Vendor> vendors = Vendor.GetAll();
            Assert.AreEqual(2, vendors.Count);
        }

        [TestMethod()]
        public void GetAllByNameTest()
        {
            Vendor vendor = new Vendor("TP-Link Technologies");
            Assert.AreEqual("TP-Link Technologies", vendor.Name);
            Assert.IsNotNull(vendor.ID);

            vendor = new Vendor("nonExistentVendorName");
            Assert.IsNull(vendor.ID);
        }

        [TestMethod()]
        public void GetAllByMacTest()
        {
            List<Vendor> vendors = Vendor.GetAllByMac("00:73:57");
            Assert.AreEqual(1, vendors.Count);
            Assert.IsTrue(vendors[0].Known_Macs.Contains("00:73:57"));

            vendors = Vendor.GetAllByMac("nonExistentMac");
            Assert.AreEqual(0, vendors.Count);
        }
    }
}
