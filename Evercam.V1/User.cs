﻿using System;
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
    public class User
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "forename")]
        public string ForeName { get; set; }
        
        [DataMember(Name = "lastname")]
        public string LastName { get; set; }

        [DataMember(Name = "username")]
        public string UserName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "created_at")]
        public long Created_At { get; set; }

        [DataMember(Name = "updated_at")]
        public long Updated_At { get; set; }

        [DataMember(Name = "confirmed_at")]
        public long Confirmed_At { get; set; }

        public static List<User> Create(User user)
        {
            HttpResponse<string> response = Unirest.Post(API.USERS_URL)
                .header("accept", "application/json")
                .body<User>(user)
                .asJson<string>();

            switch (response.Code)
            {
                case (int)System.Net.HttpStatusCode.BadRequest:
                    throw new Exception(response.Body);
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Common.MaxJsonLength;
            List<User> list = serializer.Deserialize<List<User>>(response.Body);
            
            return list;
        }

        public static List<Camera> GetAllCameras(string userId)
        {
            return GetCameras(API.USERS_URL + userId + "/cameras/");
        }

        private static List<Camera> GetCameras(string url)
        {
            HttpResponse<string> response = Unirest.get(url)
                .header("accept", "application/json")
                .header("Authorization", "c2hha2VlbGFuanVtOmFzZGYxMjM0")
                .asString();

            switch (response.Code)
            {
                case (int)System.Net.HttpStatusCode.NotFound:
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Common.MaxJsonLength;
            CamerasList list = serializer.Deserialize<CamerasList>(response.Body);
            return list.cameras;
        }
    }

    [DataContract]
    class CamerasList
    {
        [DataMember(Name = "cameras")]
        public List<Camera> cameras;
    }
}
