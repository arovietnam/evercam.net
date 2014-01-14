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

namespace Evercam.V1
{
    [DataContract]
    public class Model
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "vendor")]
        public string vendor { get; set; }

        [DataMember(Name = "known_models")]
        public List<string> Known_Models { get; set; }

        [DataMember(Name = "defaults")]
        public Defaults Defaults { get; set; }

        
        /// <summary>
        /// Get list of all camera vendors
        /// </summary>
        /// <returns>List<Vendor></returns>
        public static List<Vendor> GetAllVendors()
        {
            return GetVendors(API.MODELS_URL);
        }

        /// <summary>
        /// Get list of all camera vendors with given ID
        /// </summary>
        /// <param name="id">Vendor ID</param>
        /// <returns>List<Vendor></returns>
        public static List<Vendor> GetAllVendorsById(string vendorId)
        {
            return GetVendors(API.MODELS_URL + vendorId);
        }

        /// <summary>
        /// Get camera model with given vendor ID and model ID
        /// </summary>
        /// <param name="vendorId">Vendor ID</param>
        /// <param name="modelId">Model ID</param>
        /// <returns>Model</returns>
        public static Model Get(string vendorId, string modelId)
        {
            HttpResponse<string> response = Unirest.get(API.MODELS_URL + vendorId + "/" + modelId)
                .header("accept", "application/json")
                .asString();

            switch (response.Code)
            {
                case (int)System.Net.HttpStatusCode.NotFound:
                    return new Model();
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Common.MaxJsonLength;
            ModelsList list = serializer.Deserialize<ModelsList>(response.Body);

            return list.models.FirstOrDefault<Model>();
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
                    return new List<Vendor>();
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Common.MaxJsonLength;
            VendorsList list = serializer.Deserialize<VendorsList>(response.Body);
            return list.vendors;
        }
    }

    [DataContract]
    class ModelsList
    {
        [DataMember(Name = "models")]
        public List<Model> models;
    }

    [DataContract]
    public class Defaults
    {
        [DataMember(Name = "auth")]
        public Auth Auth;

        [DataMember(Name = "snapshots")]
        public Snapshots Snapshots;
    }

    [DataContract]
    public class Snapshots
    {
        [DataMember(Name = "jpg")]
        public string Jpg;
    }
}
