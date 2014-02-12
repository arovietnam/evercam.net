using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace EvercamV1
{
    public sealed class API
    {
        // Live Server
        public const string LIVE_URL = "https://api.evercam.io/v1/";

        // Mock Server
        // Server Script: https://github.com/evercam/tools/blob/master/mockserver/mockserver.js
        public const string MOCK_URL = "http://proxy.evercam.io:3000/v1/";

        // API Endpoints
        public const string VENDORS = "vendors.json";
        public const string VENDORS_MAC = "vendors/{0}.json";
        public const string MODELS = "models.json";
        public const string MODELS_VENDOR = "models/{0}.json";
        public const string MODELS_VENDOR_MODEL = "models/{0}/{1}.json";
        public const string USERS = "users.json";
        public const string USERS_ID = "users/{0}.json";
        public const string USERS_CAMERA = "users/{0}/cameras.json";
        public const string USERS_RIGHT = "users/{0}/rights.json";
        public const string CAMERAS = "cameras.json";
        public const string CAMERAS_ID = "cameras/{0}.json";
        public const string CAMERAS_SNAPSHOT = "cameras/{0}/snapshots.json";
        public const string CAMERAS_TIMESTAMP = "cameras/{0}/snapshots/{1}.json";

        // Declares static instance field with custom default value
        public static ThreadLocal<RestClient> Client = new ThreadLocal<RestClient>(() => new RestClient(LIVE_URL));

        /// <summary>
        /// Sets RestClient's authentication with provided auth details
        /// </summary>
        public static void SetClientAuth(Auth auth)
        {
            if (!Client.IsValueCreated)
                Client = new ThreadLocal<RestClient>(() => new RestClient(LIVE_URL));

            if (auth != null && auth.OAuth2 != null && !string.IsNullOrEmpty(auth.OAuth2.AccessToken))
                Client.Value.Authenticator = new HttpOAuth2Authenticator(auth.OAuth2.AccessToken, auth.OAuth2.TokenType);
            else if (auth != null && auth.Basic != null && !string.IsNullOrEmpty(auth.Basic.UserName))
                Client.Value.Authenticator = new HttpBasicAuthenticator(auth.Basic.UserName, auth.Basic.Password);
        }

        /// <summary>
        /// Set SANDBOX = True in order to test with Mock Server [http://proxy.evercam.io:3000/v1/]. 
        /// By default SANDBOX = False.
        /// </summary>
        public static bool SANDBOX
        {
            get { return _sandbox; }
            set { _sandbox = value; Client = new ThreadLocal<RestClient>(() => new RestClient(_sandbox ? MOCK_URL : LIVE_URL)); }
        }

        [ThreadStatic]
        private static bool _sandbox;
    }
}
