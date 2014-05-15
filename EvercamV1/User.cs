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

namespace EvercamV1
{
    public class User
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("forename", NullValueHandling = NullValueHandling.Ignore)]
        public string ForeName { get; set; }
        [JsonProperty("lastname", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public long CreatedAt { get; set; }
        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public long UpdatedAt { get; set; }
        [JsonProperty("confirmed_at", NullValueHandling = NullValueHandling.Ignore)]
        public long ConfirmedAt { get; set; }

        public UserInfo GetInfo()
        {
            return new UserInfo()
            {
                ID = this.ID,
                Email = this.Email,
                UserName = this.UserName,
                ForeName = this.ForeName,
                LastName = this.LastName,
                Country = this.Country
            };
        }
    }

    public class UserInfo
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("forename", NullValueHandling = NullValueHandling.Ignore)]
        public string ForeName { get; set; }
        [JsonProperty("lastname", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
    }
}
