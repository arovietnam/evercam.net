using System;
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
    public class Firmware
    {
        [DataMember(Name = "auth")]
        public Auth Auth { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
