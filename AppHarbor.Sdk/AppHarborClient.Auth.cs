using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
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

		/// <summary>
		/// This will open a browser on the user's machine and asks for authorization of the client, if the user
		/// accepts then he is redirected back to localhost with the authorization code.
		/// </summary>
		/// <remarks>
		/// This command will start a webserver on localhost to listen for the authorization code. The webserver is
		/// listining on the following url: <code>http://+:80/Temporary_Listen_Addresses/</code>
		/// </remarks>
		/// <param name="clientId">The client ID that can be obtained created at http://appharbor.com/clients. The callback URL
		/// for the client must be set to <code>http://localhost:80/Temporary_Listen_Addresses/</code>. May not be null.</param>
		/// <param name="clientSecret">The client secret, which is obtained in the same way as the client ID. May not be null.</param>
		/// <param name="timeout">Timeout, if the user has not authorized your application within the given timeout then a
		/// <see cref="TimeoutException"/> is thrown and the operation is cancelled.</param>
		/// <returns>If the operation is successful a valid <see cref="AuthInfo"/> is returned.</returns>
		public static AuthInfo AskForAuthorization(string clientId, string clientSecret, TimeSpan timeout)
		{
			CheckArgumentNull("clientId", clientId);
			CheckArgumentNull("clientSecret", clientSecret);

			string code = GetCodeFromLocalHost(clientId, timeout);
			return GetAuthInfo(clientId, clientSecret, code);
		}

		private static string GetAppHarborAuthorizationUrl(string clientId, string redirectUrl)
		{
			return string.Format("{0}/user/authorizations/new?client_id={1}&redirect_uri={2}", BaseUrl, clientId, redirectUrl);
		}

		private static string GetCodeFromLocalHost(string clientId, TimeSpan timeout)
		{
			const string httpTemporaryListenAddresses = "http://+:80/Temporary_Listen_Addresses/";
			const string redirectUrl = "http://localhost:80/Temporary_Listen_Addresses/";

			string code;
			using (var listener = new HttpListener())
			{
				string localHostUrl = string.Format(httpTemporaryListenAddresses);

				listener.Prefixes.Add(localHostUrl);
				listener.Start();

				using (Process.Start(GetAppHarborAuthorizationUrl(clientId, redirectUrl)))
				{
					while (true)
					{
						var start = DateTime.Now;
						var context = listener.GetContext(timeout);
						var usedTime = DateTime.Now.Subtract(start);
						timeout = timeout.Subtract(usedTime);

						if (context.Request.Url.AbsolutePath == "/Temporary_Listen_Addresses/")
						{
							code = context.Request.QueryString["code"];
							if (code == null)
							{
								throw new AuthenticationException("Access denied, no return code was returned");
							}

							var writer = new StreamWriter(context.Response.OutputStream);
							writer.WriteLine(CloseWindowResponse);
							writer.Flush();

							context.Response.Close();
							break;
						}

						context.Response.StatusCode = 404;
						context.Response.Close();
					}
				}
			}
			return code;
		}

		private const string CloseWindowResponse = "<!DOCTYPE html><html><head></head><body onload=\"closeThis();\"><h1>Authorization Successfull</h1><p>You can now close this window</p><script type=\"text/javascript\">function closeMe() { window.close(); } function closeThis() { window.close(); }</script></body></html>";
	}
}
