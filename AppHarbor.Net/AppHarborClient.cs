#region License
//   Copyright 2012 Nikolas Tziolis
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		private const string BaseUrl = "https://appharbor.com/";

		private readonly RestClient _client;
		private readonly Uri _baseUri;

		public AppHarborClient(AuthInfo authInfo)
			: this(authInfo, new RestClient(BaseUrl))
		{
		}

		protected AppHarborClient(AuthInfo authInfo, RestClient restClient)
		{
			CheckArgumentNull("authInfo", authInfo);
			CheckArgumentNull("restClient", restClient);

			_client = restClient;
			_client.Authenticator = new AppHarborHeaderAuthenticator(authInfo);

			_baseUri = new Uri(BaseUrl);
		}

		private static string ExtractId(Uri url)
		{
			CheckArgumentNull("url", url);

			return url.LocalPath.Split('/').Last();
		}

		private T ExecuteGet<T>(RestRequest request)
			where T : new()
		{
			var response = _client.Execute<T>(request);

			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return default(T);
			}

			return response.Data;
		}

		private T ExecuteGetKeyed<T>(RestRequest request)
			where T : class, IKeyed, new()
		{
			var response = _client.Execute<T>(request);

			var data = response.Data;
			if (data == null)
			{
				return null;
			}

			data.Id = ExtractId(response.ResponseUri);

			if (data is IUrl)
			{
				((IUrl)data).Url = new Uri(_baseUri, response.ResponseUri.LocalPath);
			}

			return data;
		}

		private List<T> ExecuteGetListKeyed<T>(RestRequest request)
			where T : IKeyed, IUrl
		{
			var response = _client.Execute<List<T>>(request);

			var data = response.Data;
			if (data == null)
			{
				return null;
			}

			foreach (var item in data)
			{
				item.Id = ExtractId(item.Url);
			}

			return data;
		}

		private CreateResult ExecuteCreateApplication(RestRequest request)
		{
			return ExecuteCreate(request);
		}

		private CreateResult ExecuteCreate(RestRequest request)
		{
			var response = _client.Execute(request);

			if (response == null)
			{
				throw new ArgumentException("Response cannot be null.");
			}

			if (response.StatusCode == HttpStatusCode.Conflict)
			{
				return new CreateResult
				{
					Status = CreateStatus.AlreadyExists
				};
			}

			if (response.StatusCode != HttpStatusCode.Created)
			{
				return new CreateResult
				{
					Status = CreateStatus.Undefined
				};
			}

			var locationHeader = response.Headers
				.SingleOrDefault(p => string.Equals(p.Name, "Location", StringComparison.OrdinalIgnoreCase));

			if (locationHeader == null)
			{
				throw new ArgumentException("Location header was not set.");
			}

			var location = new Uri((string)locationHeader.Value);
			var id = ExtractId(location);

			return new CreateResult
			{
				Status = CreateStatus.Created,
				Id = id,
				Location = location,
			};
		}

		private bool ExecuteEdit(RestRequest request)
		{
			var response = _client.Execute(request);
			if (response == null)
			{
				return false;
			}

			return (response.StatusCode == HttpStatusCode.OK);
		}

		private bool ExecuteDelete(RestRequest request)
		{
			var response = _client.Execute(request);
			if (response == null)
			{
				return false;
			}

			return (response.StatusCode == HttpStatusCode.NoContent);
		}

		private static void CheckArgumentNull(string argumentName, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(argumentName);
			}
		}
	}
}
