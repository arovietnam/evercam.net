using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvercamV1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvercamV1.Tests
{
    /// <summary>
    /// Testing using mockserver: https://github.com/evercam/tools/blob/master/mockserver/mockserver.js
    /// </summary>
    [TestClass()]
    public class ModelTests
    {
        [TestMethod()]
        public void GetAllVendorsTest()
        {
            API.SANDBOX = true;
            List<Vendor> vendors = Model.GetAllVendors();
            Assert.AreEqual(2, vendors.Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void GetAllVendorsByIdTest()
        {
            API.SANDBOX = true;
            List<Vendor> vendors = Model.GetAllVendorsById("testid");
            Assert.AreEqual(1, vendors.Count);
            Assert.AreEqual("testid", vendors[0].ID);

            vendors = Model.GetAllVendorsById("nonExistentId");
            Assert.AreEqual(0, vendors.Count);
        }

        [TestMethod()]
        public void GetTest()
        {
            API.SANDBOX = true;
            Model model = Model.Get("testid", "YCW005");
            Assert.AreEqual("YCW005", model.Name);
        }
    }
}
