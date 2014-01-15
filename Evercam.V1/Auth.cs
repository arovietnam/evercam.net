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
    public class Auth
    {
        [DataMember(Name = "basic")]
        public Basic Basic { get; set; }

        public Auth()
        {
        }

        public Auth(Basic basic)
        {
            Basic = basic;
        }
    }

    [DataContract]
    public class Basic
    {
        string _username;
        string _password;
        string _encoded;

        [DataMember(Name = "username")]
        public string UserName 
        { 
            get { return _username; }
            set { _username = value; Encoded = Common.Base64Encode(value + ":" + _password); } 
        }

        [DataMember(Name = "password")]
        public string Password 
        {
            get { return _password; }
            set { _password = value; Encoded = Common.Base64Encode(_username + ":" + value); } 
        }

        [DataMember(Name = "encoded")]
        public string Encoded 
        {
            get { return _encoded; }
            set { _encoded = value; } 
        }

        public Basic()
        { 
        }

        public Basic(string username, string password)
        {
            _username = username;
            _password = password;
            _encoded = Common.Base64Encode(username + ":" + password);
        }
    }
}
