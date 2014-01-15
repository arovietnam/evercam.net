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
    public class ModelTests
    {
        [TestMethod()]
        public void GetAllVendorsTest()
        {
            List<Vendor> vendors = Model.GetAllVendors();
            Assert.AreEqual(2, vendors.Count);
        }

        [TestMethod()]
        public void GetAllVendorsByIdTest()
        {
            List<Vendor> vendors = Model.GetAllVendorsById("testid");
            Assert.AreEqual(1, vendors.Count);
            Assert.AreEqual("testid", vendors[0].ID);

            vendors = Model.GetAllVendorsById("nonExistentId");
            Assert.AreEqual(0, vendors.Count);
        }

        [TestMethod()]
        public void GetTest()
        {
            Model model = Model.Get("testid", "YCW005");
            Assert.AreEqual("YCW005", model.Name);
        }
    }
}
