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
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborApi
	{
		const string BaseUrl = "https://appharbor.com/";

		private RestClient _Client = null;
		private Uri _BaseUri = null;

		public AppHarborApi(AuthInfo authInfo)
			: this(authInfo, new RestClient(BaseUrl))
		{
		}

		/// <summary>
		/// Internal to hide RestClient dependency
		/// </summary>
		/// <param name="restClient">Rest client instance that is to be used.</param>
		internal AppHarborApi(AuthInfo authInfo, RestClient restClient)
		{
			if (authInfo == null)
				throw new ArgumentNullException("authInfo");

			if (restClient == null)
				throw new ArgumentNullException("restClient");

			_Client = restClient;
			_Client.Authenticator = new HeaderAppHaborAuthenticator(authInfo);

			_BaseUri = new Uri(BaseUrl);
		}

		private static long ExtractLongID(string url)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			return Convert.ToInt64(ExtractID(url));
		}

		private static string ExtractID(string url)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			return url.Split('/').Last();
		}

		private T ExecuteGet<T>(RestRequest request)
				where T : new()
		{
			var response = _Client.Execute<T>(request);

			if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				return default(T);
			}

			return response.Data;
		}

		private T ExecuteGetKeyed<T>(RestRequest request)
				where T : class, IKeyed, new()
		{
			var response = _Client.Execute<T>(request);

			var data = response.Data;
			if (data == null)
				return null;

			data.ID = ExtractID(response.ResponseUri.LocalPath);

			if (data is IUrl)
			{
				((IUrl)data).Url = new Uri(_BaseUri, response.ResponseUri.LocalPath).OriginalString;
			}

			return data;
		}

		private List<T> ExecuteGetListKeyed<T>(RestRequest request)
				where T : IKeyed, IUrl
		{
			var response = _Client.Execute<List<T>>(request);

			var data = response.Data;
			if (data == null)
				return null;

			foreach (var item in data)
			{
				item.ID = ExtractID(item.Url);
			}

			return data;
		}

		private CreateResult<long> ExecuteCreate(RestRequest request)
		{
			return ExecuteCreate<long>(request, ExtractLongID);
		}


		private CreateResult<string> ExecuteCreateApplication(RestRequest request)
		{
			return ExecuteCreate<string>(request, ExtractID);
		}

		private CreateResult<T> ExecuteCreate<T>(RestRequest request, Func<string, T> extractID)
		{
			var response = _Client.Execute(request);

			if (response == null)
				throw new ArgumentException("Response cannot be null.");

			if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
				return new CreateResult<T>()
				{
					Status = CreateStatus.AlreadyExists
				};

			if (response.StatusCode != System.Net.HttpStatusCode.Created)
				return new CreateResult<T>()
				{
					Status = CreateStatus.Undefined
				};

			var locationHeader = response.Headers
					.SingleOrDefault(p => string.Equals(p.Name, "Location", StringComparison.OrdinalIgnoreCase));

			if (locationHeader == null)
				throw new ArgumentException("Location header was not set.");

			var location = (string)locationHeader.Value;
			var id = extractID(location);

			return new CreateResult<T>()
			{
				Status = Model.CreateStatus.Created,
				ID = id,
				Location = location,
			};
		}

		private bool ExecuteEdit(RestRequest request)
		{
			var response = _Client.Execute(request);
			if (response == null)
				return false;

			return (response.StatusCode == System.Net.HttpStatusCode.OK);
		}

		private bool ExecuteDelete(RestRequest request)
		{
			var response = _Client.Execute(request);
			if (response == null)
				return false;

			// System.Net.HttpStatusCode.NotFound is returned if there is nothing to delete

			return (response.StatusCode == System.Net.HttpStatusCode.NoContent);
		}

		private static void CheckArgumentNull(string argumentName, object value)
		{
			if (value == null)
				throw new ArgumentNullException(argumentName);
		}
	}
}
