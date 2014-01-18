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

namespace Evercam.V1
{
    public class Firmware
    {
        [JsonProperty("auth")]
        public Auth Auth { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
