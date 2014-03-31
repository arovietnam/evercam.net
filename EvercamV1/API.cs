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

        internal const string ACCESS_GRANT_TYPE = "authorization_code";
        internal const string REFRESH_TOKEN_TYPE = "refresh_token";

        // API Endpoints
        public const string AUTH = "oauth/authorize";
        public const string TOKEN = "oauth/token";
        public const string REVOKE = "oauth/revoke";
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
        public const string CAMERAS_TEST = "cameras/test.json";
        public const string CAMERAS_ID = "cameras/{0}.json";
        public const string CAMERAS_SHARE = "cameras/{0}/share.json";
        public const string CAMERAS_SNAPSHOT = "cameras/{0}/snapshots.json";
        public const string CAMERAS_SNAPSHOT_LATEST = "cameras/{0}/snapshots/latest.json";
        public const string CAMERAS_SNAPSHOT_RANGE = "cameras/{0}/snapshots/range.json";
        public const string CAMERAS_SNAPSHOT_DAYS = "cameras/{0}/snapshots/{1}/{2}/days.json";
        public const string CAMERAS_SNAPSHOT_HOURS = "cameras/{0}/snapshots/{1}/{2}/{3}/hours.json";
        public const string CAMERAS_SNAPSHOT_TIMESTAMP = "cameras/{0}/snapshots/{1}.json";
        public const string CAMERAS_SNAPSHOT_JPG = "cameras/{0}/snapshot.jpg";

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
            set { _sandbox = value; Client = new ThreadLocal<RestClient>(() => new RestClient(_sandbox ? MOCK_URL : LIVE_URL)); }
        }

        [ThreadStatic]
        private static bool _sandbox;
    }
}
