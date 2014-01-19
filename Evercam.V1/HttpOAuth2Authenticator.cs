using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;

namespace Evercam.V1
{
    public class HttpOAuth2Authenticator : OAuth2Authenticator
    {
        public HttpOAuth2Authenticator(string token): base(token)
        {
            // define body
        }

        public override void Authenticate(IRestClient client, IRestRequest request) 
        {
            // define body
        }
    }
}
