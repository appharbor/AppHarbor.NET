using System;
using System.Linq;
using System.Net;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public static AuthInfo GetAuthInfo(string clientId, string clientSecret, string code)
		{
			CheckArgumentNull("clientId", clientId);
			CheckArgumentNull("clientSecret", clientSecret);
			CheckArgumentNull("code", code);

			// make POST request to obtain the token
			var client = new RestClient(BaseUrl);
			var request = new RestRequest(Method.POST);
			request.Resource = "tokens";
			request.AddParameter("client_id", clientId);
			request.AddParameter("client_secret", clientSecret);
			request.AddParameter("code", code);

			var response = client.Execute(request);
			if (response.StatusCode != HttpStatusCode.OK)
			{
				return null;
			}

			// parse the returned value
			var values = response.Content.Split('&')
				.Select(x => x.Split(new[] { '=' }, count: 2))
				.ToArray();

			// if format is: error=unauthorized_client
			if (values.Length == 1)
			{
				var errorMessage = values[0][1];
				throw new ApplicationException(string.Format("Error: {0}", values[0][1]));
			}

			// if format is: access_token=:accesstoken&token_type=:tokentype
			if (values.Length == 2 && values.All(x => x.Length == 2))
			{
				var authInfoElements = values.ToDictionary(x => x[0], x => x[1]);

				return new AuthInfo(authInfoElements["access_token"], authInfoElements["token_type"]);
			}

			return null;
		}
	}
}
