using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Threading.Tasks;

using unirest_net;
using unirest_net.http;
using unirest_net.request;

using System.Runtime.Serialization;
using System.Web.Script.Serialization;

using Evercam;

namespace Evercam.V1
{
    [DataContract]
    public class Vendor
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "is_supported")]
        public bool Is_Supported { get; set; }

        [DataMember(Name = "known_macs")]
        public List<string> Known_Macs { get; set; }

        [DataMember(Name = "firmwares")]
        public List<Firmware> Firmwares { get; set; }

        [DataMember(Name = "models")]
        public List<string> Models { get; set; }


        public Vendor() { }

        public Vendor(string name)
        {
            Vendor vendor = GetAllByName(name).FirstOrDefault<Vendor>();
            this.ID = vendor.ID;
            this.Is_Supported = vendor.Is_Supported;
            this.Name = vendor.Name;
            this.Models = vendor.Models;
            this.Known_Macs = vendor.Known_Macs;
            this.Firmwares = vendor.Firmwares;
        }
        
        /// <summary>
        /// Get vendor's firmware with given firmware name
        /// </summary>
        /// <param name="name">Firmware Name</param>
        /// <returns>Firmware</returns>
        public Firmware GetFirmware(string name)
        {
            if (Firmwares.Count == 0)
                throw new Exception("Unknown firmware name");

            foreach (Firmware fw in Firmwares)
            {
                if (fw.Name.ToLower().Equals(name.ToLower()))
                    return fw;
            }

            throw new Exception("Unknown firmware name");
        }

        /// <summary>
        /// Get a list of all camera vendors
        /// </summary>
        /// <returns>List<Vendor></returns>
        public static List<Vendor> GetAll()
        {
            return GetVendors(API.VENDORS_URL);
        }

        /// <summary>
        /// Get a list of all camera vendors filtered by given vendor name
        /// </summary>
        /// <param name="name">Vendor Name</param>
        /// <returns>List<Vendor></returns>
        public static List<Vendor> GetAllByName(string name)
        {
            List<Vendor> allVendors = GetVendors(API.VENDORS_URL);
            List<Vendor> list = new List<Vendor>();
            
            foreach (Vendor v in allVendors)
            {
                if (v.Name.ToLower().Equals(name.ToLower()))
                    list.Add(v);
            }

            // If there are no matches the server will return a 404 NOT FOUND status
            if (list.Count == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return list;
        }

        /// <summary>
        /// Get a list of all camera vendors filtered by given MAC address
        /// </summary>
        /// <param name="mac">MAC Address</param>
        /// <returns>List<Vendor></returns>
        public static List<Vendor> GetAllByMac(string mac)
        {
            return GetVendors(API.VENDORS_URL + mac);
        }

        /// <summary>
        /// PRIVATE: Get list of all vendors
        /// </summary>
        /// <param name="url">API URL</param>
        /// <returns>List<Vendor></returns>
        private static List<Vendor> GetVendors(string url)
        {
            HttpResponse<string> response = Unirest.get(url)
                .header("accept", "application/json")
                .asString();

            switch (response.Code)
            {
                case (int)System.Net.HttpStatusCode.NotFound:
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Common.MaxJsonLength;
            VendorsList list = serializer.Deserialize<VendorsList>(response.Body);
            return list.vendors;
        }
    }

    [DataContract]
    class VendorsList
    {
        [DataMember(Name = "vendors")]
        public List<Vendor> vendors;
    }
}
