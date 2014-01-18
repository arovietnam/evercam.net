using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Threading.Tasks;

using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Evercam.V1
{
    public class Vendor
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_supported")]
        public bool IsSupported { get; set; }

        [JsonProperty("known_macs")]
        public List<string> KnownMacs { get; set; }

        [JsonProperty("firmwares")]
        public List<Firmware> Firmwares { get; set; }

        [JsonProperty("models")]
        public List<string> Models { get; set; }


        public Vendor() { }

        /// <summary>
        /// Constructor: Initialize new vendor object with given name
        /// </summary>
        /// <param name="name">Vendor Name</param>
        public Vendor(string name)
        {
            Vendor vendor = GetAllByName(name).FirstOrDefault<Vendor>();
            if (vendor == null)
                throw new Exception("Unknown vendor");

            this.ID = vendor.ID;
            this.IsSupported = vendor.IsSupported;
            this.Name = vendor.Name;
            this.Models = vendor.Models;
            this.KnownMacs = vendor.KnownMacs;
            this.Firmwares = vendor.Firmwares;
        }

        /// <summary>
        /// Get vendor's firmware with given firmware name
        /// </summary>
        /// <param name="name">Firmware Name</param>
        /// <returns>Firmware</returns>
        public Firmware GetFirmware(string name)
        {
            Firmware firmware = null;

            try
            {
                List<Firmware> list = Firmwares;
                foreach (Firmware item in list)
                {
                    if (item.Name.ToLower().Equals(name.ToLower()))
                    {
                        firmware = item;
                        break;
                    }
                }
            }
            catch (Exception x) { throw new Exception("Error Occured: " + x.Message); }

            if (firmware == null)
                throw new Exception("Unknown firmware");

            return firmware;
        }

        /// <summary>
        /// Get a list of all camera vendors
        /// </summary>
        /// <returns>List<Vendor></returns>
        public static List<Vendor> GetAll()
        {
            return GetVendors(API.VENDORS);
        }

        /// <summary>
        /// Get a list of all camera vendors filtered by given vendor name
        /// </summary>
        /// <param name="name">Vendor Name</param>
        /// <returns>List<Vendor></returns>
        public static List<Vendor> GetAllByName(string name)
        {
            List<Vendor> allVendors = GetVendors(API.VENDORS);
            List<Vendor> list = new List<Vendor>();

            try
            {
                foreach (Vendor v in allVendors)
                {
                    if (v.Name.ToLower().Equals(name.ToLower()))
                        list.Add(v);
                }
            }
            catch (Exception x) { throw new Exception( "Error Occured: " + x.Message); }

            return list;
        }

        /// <summary>
        /// Get a list of all camera vendors filtered by given MAC address
        /// </summary>
        /// <param name="mac">MAC Address</param>
        /// <returns>List<Vendor></returns>
        public static List<Vendor> GetAllByMac(string mac)
        {
            return GetVendors(API.VENDORS + mac);
        }

        /// <summary>
        /// PRIVATE: Get list of all vendors
        /// </summary>
        /// <param name="url">API URL</param>
        /// <returns>List<Vendor></returns>
        internal static List<Vendor> GetVendors(string url)
        {
            try
            {
                var request = new RestRequest(url, Method.GET);
                request.RequestFormat = DataFormat.Json;
                var response = API.Client.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new Exception(response.Content);
                }

                return JObject.Parse(response.Content)["vendors"].ToObject<List<Vendor>>();
            }
            catch (Exception x) { throw new Exception("Error Occured: " + x.Message); }
        }
    }
}
