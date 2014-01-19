using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;

namespace Evercam.V1
{
    public sealed class API
    {
        // Live Server
        public const string LIVE_URL = "https://api.evercam.io/v1/";
        // Mock Server
        public const string MOCK_URL = "http://proxy.evercam.io:3000/v1/";
        
        public const string VENDORS = "vendors/";
        public const string USERS = "users/";
        public const string CAMERAS = "cameras/";
        public const string MODELS = "models/";

        public static RestClient Client = new RestClient(MOCK_URL);

        public static bool SANDBOX
        {
            get { return _sandbox; }
            set { _sandbox = value; Client = new RestClient(_sandbox ? MOCK_URL : LIVE_URL); }
        }

        private static bool _sandbox = false;
    }
}
