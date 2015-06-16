using Newtonsoft.Json;

namespace EvercamV2
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
        [JsonProperty("firstname", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

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
        /// User password
        /// </summary>
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

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
                FirstName = this.FirstName,
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
        [JsonProperty("firstname", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

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

    public class UserCredentials
    {
        [JsonProperty("api_id", NullValueHandling = NullValueHandling.Ignore)]
        public string APIID;

        [JsonProperty("api_key", NullValueHandling = NullValueHandling.Ignore)]
        public string APIKEY;
    }
}
