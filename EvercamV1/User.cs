using Newtonsoft.Json;

namespace EvercamV1
{
    public class User
    {
        /// <summary>
        /// Unique Evercam username
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Unix timestamp at creation
        /// </summary>
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public long CreatedAt { get; set; }

        /// <summary>
        /// Unix timestamp at last update
        /// </summary>
        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public long UpdatedAt { get; set; }

        /// <summary>
        /// Unix timestamp at account confirmation
        /// </summary>
        [JsonProperty("confirmed_at", NullValueHandling = NullValueHandling.Ignore)]
        public long ConfirmedAt { get; set; }

        /// <summary>
        /// Users forename
        /// </summary>
        [JsonProperty("forename", NullValueHandling = NullValueHandling.Ignore)]
        public string ForeName { get; set; }

        /// <summary>
        /// Users lastname
        /// </summary>
        [JsonProperty("lastname", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        
        /// <summary>
        /// Unique Evercam username
        /// </summary>
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }

        /// <summary>
        /// Users email address
        /// </summary>
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>
        /// Two letter ISO country code
        /// </summary>
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        
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
        /// <summary>
        /// Unique Evercam username
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Users forename
        /// </summary>
        [JsonProperty("forename", NullValueHandling = NullValueHandling.Ignore)]
        public string ForeName { get; set; }

        /// <summary>
        /// Users lastname
        /// </summary>
        [JsonProperty("lastname", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>
        /// Unique Evercam username
        /// </summary>
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }

        /// <summary>
        /// Users email address
        /// </summary>
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>
        /// Two letter ISO country code
        /// </summary>
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
    }
}
