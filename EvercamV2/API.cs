using System;
using System.Threading;
using RestSharp;

namespace EvercamV2
{
    public sealed class API
    {
        // Live Server
        public const string LIVE_URL = "https://api.evercam.io/v1/";
        // Proxy Server
        public const string PROXY_URL = "http://evr.cm/";
        // Test Server
        public const string TEST_URL = "http://proxy.evr.cm:9292/v1/";
        // Local Server
        //public const string TEST_URL = "http://localhost:9292/v1/";

        internal const string CAMERA_PROTOCOL = "http://";

        // API Endpoints
        public const string AUTH_TEST = "auth/test";
        public const string VENDORS = "vendors";
        public const string VENDORS_ID = "vendors/{0}";
        public const string MODELS = "models";
        public const string MODELS_ID = "models/{0}";
        public const string USERS = "users";
        public const string USERS_ID = "users/{0}";
        public const string USERS_CREDENTIALS = "users/{0}/credentials";
        public const string PUBLIC_CAMERAS = "public/cameras";
        public const string PUBLIC_CAMERAS_NEAREST = "public/cameras/nearest";
        public const string PUBLIC_CAMERAS_NEAREST_SNAPSHOT = "public/cameras/nearest/snapshot";
        public const string CAMERAS_SHARES = "cameras/{0}/shares";
        public const string CAMERAS_SHARES_REQUESTS = "cameras/{0}/shares/requests";
        public const string CAMERAS_WEBHOOKS = "cameras/{0}/webhooks";
        public const string CAMERAS_WEBHOOKS_ID = "cameras/{0}/webhooks/{1}";
        public const string CAMERAS = "cameras";
        public const string CAMERAS_TEST = "cameras/test";
        public const string CAMERAS_ID = "cameras/{0}";
        public const string CAMERAS_LIVE = "cameras/{0}/live/snapshot";
        public const string CAMERAS_LOGS = "cameras/{0}/logs";
        public const string CAMERAS_SNAPSHOT = "cameras/{0}/recordings/snapshots";
        public const string CAMERAS_SNAPSHOT_LATEST = "cameras/{0}/recordings/snapshots/latest";
        public const string CAMERAS_SNAPSHOT_DAYS = "cameras/{0}/recordings/snapshots/{1}/{2}/days";
        public const string CAMERAS_SNAPSHOT_HOURS = "cameras/{0}/recordings/snapshots/{1}/{2}/{3}/hours";
        public const string CAMERAS_SNAPSHOT_TIMESTAMP = "cameras/{0}/recordings/snapshots/{1}";

        // Declares static instance field with custom default value
        internal static ThreadLocal<RestClient> Client = new ThreadLocal<RestClient>(() => { return new RestClient(LIVE_URL); });

        /// <summary>
        /// Sets RestClient's authentication with provided auth details
        /// </summary>
        internal static void SetClientAuth(Auth auth)
        {
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
            set { _sandbox = value; Client = new ThreadLocal<RestClient>(() => new RestClient(_sandbox ? TEST_URL : LIVE_URL)); }
        }

        [ThreadStatic]
        private static bool _sandbox;
    }
}
