using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evercam.V1
{
    public sealed class API
    {
        /// <summary>
        /// API URLs Evercam Live
        /// </summary>
        //public const string VENDORS_URL = "https://api.evercam.io/v1/vendors/";
        //public const string USERS_URL = "https://api.evercam.io/v1/users/";
        //public const string STREAMS_URL = "https://api.evercam.io/v1/streams/";
        //public const string MODELS_URL = "https://api.evercam.io/v1/models/";

        /// <summary>
        /// API URLs for Testing
        /// </summary>
        public const string VENDORS_URL = "http://proxy.evercam.io:3000/v1/vendors/";
        public const string USERS_URL = "http://proxy.evercam.io:3000/v1/users/";
        public const string STREAMS_URL = "http://proxy.evercam.io:3000/v1/streams/";
        public const string MODELS_URL = "http://proxy.evercam.io:3000/v1/models/";
    }
}
