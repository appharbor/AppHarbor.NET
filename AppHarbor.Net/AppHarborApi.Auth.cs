using System;
using System.Linq;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborApi
	{
		public static AuthInfo GetAuthInfo(string clientID, string clientSecret, string code)
		{
			CheckArgumentNull("clientID", clientID);
			CheckArgumentNull("clientSecret", clientSecret);
			CheckArgumentNull("code", code);

			// make POST request to obtain the token
			var client = new RestClient(BaseUrl);
			var request = new RestRequest(Method.POST);
			request.Resource = "tokens";
			request.AddParameter("client_id", clientID);
			request.AddParameter("client_secret", clientSecret);
			request.AddParameter("code", code);

			var response = client.Execute(request);
			if (response.StatusCode != System.Net.HttpStatusCode.OK)
				return null;

			// parse the returned value
			var values = response.Content.Split('&')
					.Select(i => i.Split('='))
					.ToArray();

			// if format is: error=unauthorized_client
			if (values.Length == 1)
			{
				var errorMessage = values[0][1];
				throw new ApplicationException(string.Format("Error: {0}", values[0][1]));
			}

			// if format is: access_token=:accesstoken&token_type=:tokentype
			if (values.Length == 2 && values.All(i => i.Length == 2))
			{
				var dict = values.ToDictionary(i => i[0], i => i[1]);
				var accessToken = dict["access_token"];
				var tokenType = dict["token_type"];

				return new AuthInfo()
				{
					AccessToken = accessToken,
					TokenType = tokenType,
				};
			}

			return null;
		}
	}
}
