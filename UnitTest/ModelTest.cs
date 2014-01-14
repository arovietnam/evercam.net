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
    public class ModelTest
    {
        [TestMethod]
        public void TestGetAll()
        {
            List<Vendor> vendors = Model.GetAllVendors();
            Assert.AreEqual(2, vendors.Count);
        }

        [TestMethod]
        public void TestGetAllByID()
        {
            List<Vendor> vendors = Model.GetAllVendorsById("testid");
            Assert.AreEqual(1, vendors.Count);
            Assert.AreEqual("testid", vendors[0].ID);

            vendors = Model.GetAllVendorsById("nonExistentId");
            Assert.AreEqual(0, vendors.Count);
        }

        [TestMethod]
        public void TestGetModel()
        {
            Model model = Model.Get("testid", "YCW005");
            Assert.AreEqual("YCW005", model.Name);
        }
    }
}
