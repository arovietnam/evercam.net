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
		private readonly string _authorizationValue;

        public HttpOAuth2Authenticator(string accessToken): this(accessToken, "OAuth")
		{
            _authorizationValue = "bearer " + accessToken;
		}

        public HttpOAuth2Authenticator(string accessToken, string tokenType) : base(accessToken)
		{
			_authorizationValue = tokenType + " " + accessToken;
		}

		public override void Authenticate(IRestClient client, IRestRequest request)
		{
			if (!request.Parameters.Any(p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase)))
			{
				request.AddParameter("Authorization", _authorizationValue, ParameterType.HttpHeader);
			}
		}
    }
}
