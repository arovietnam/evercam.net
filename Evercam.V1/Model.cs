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
    public class Model
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("vendor")]
        public string vendor { get; set; }

        [JsonProperty("known_models")]
        public List<string> KnownModels { get; set; }

        [JsonProperty("defaults")]
        public Defaults Defaults { get; set; }

        
        /// <summary>
        /// Get list of all camera vendors
        /// </summary>
        /// <returns>List<Vendor></returns>
        public static List<Vendor> GetAllVendors()
        {
            return Vendor.GetVendors(API.MODELS);
        }

        /// <summary>
        /// Get list of all camera vendors with given ID
        /// </summary>
        /// <param name="id">Vendor ID</param>
        /// <returns>List<Vendor></returns>
        public static List<Vendor> GetAllVendorsById(string vendorId)
        {
            return Vendor.GetVendors(API.MODELS + vendorId);
        }

        /// <summary>
        /// Get camera model with given vendor ID and model ID
        /// </summary>
        /// <param name="vendorId">Vendor ID</param>
        /// <param name="modelId">Model ID</param>
        /// <returns>Model</returns>
        public static Model Get(string vendorId, string modelId)
        {
            try
            {
                var request = new RestRequest(API.MODELS + vendorId + "/" + modelId, Method.GET);
                request.RequestFormat = DataFormat.Json;
                var response = API.Client.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new Exception(response.Content);
                }

                return JObject.Parse(response.Content)["models"].ToObject<List<Model>>().FirstOrDefault<Model>();
            }
            catch (Exception x) { throw new Exception("Error Occured: " + x.Message); }
        }

        ///// <summary>
        ///// PRIVATE: Get list of all vendors
        ///// </summary>
        ///// <param name="url">API URL</param>
        ///// <returns>List<Vendor></returns>
        //private static List<Vendor> GetVendors(string url)
        //{
        //    HttpResponse<string> response = Unirest.get(url)
        //        .header("accept", "application/json")
        //        .asString();

        //    switch (response.Code)
        //    {
        //        case (int)System.Net.HttpStatusCode.NotFound:
        //            return new List<Vendor>();
        //    }

        //    var serializer = new JavaScriptSerializer();
        //    serializer.MaxJsonLength = Common.MaxJsonLength;
        //    VendorsList list = serializer.Deserialize<VendorsList>(response.Body);
        //    return list.vendors;
        //}
    }

    class ModelsList
    {
        [JsonProperty("models")]
        public List<Model> models;
    }

    public class Defaults
    {
        [JsonProperty("auth")]
        public Auth Auth;

        [JsonProperty("snapshots")]
        public Snapshots Snapshots;
    }

    public class Snapshots
    {
        [JsonProperty("jpg")]
        public string Jpg;
    }
}
