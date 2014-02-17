﻿using System;
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
    public class Model
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("vendor", NullValueHandling = NullValueHandling.Ignore)]
        public string Vendor { get; set; }

        [JsonProperty("known_models", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> KnownModels { get; set; }

        [JsonProperty("defaults", NullValueHandling = NullValueHandling.Ignore)]
        public Defaults Defaults { get; set; }
    }

    public class Defaults
    {
        [JsonProperty("auth", NullValueHandling = NullValueHandling.Ignore)]
        public Auth Auth;

        [JsonProperty("snapshots", NullValueHandling = NullValueHandling.Ignore)]
        public Snapshots Snapshots;
    }

    public class Snapshots
    {
        [JsonProperty("jpg", NullValueHandling = NullValueHandling.Ignore)]
        public string Jpg;
    }
}