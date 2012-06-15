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
using RestSharp;

namespace AppHarbor
{
	public class AppHarborHeaderAuthenticator : IAuthenticator
	{
		private readonly string _authorizationHeaderValue;

		public AppHarborHeaderAuthenticator(AuthInfo authInfo)
		{
			if (authInfo == null)
			{
				throw new ArgumentNullException("authInfo");
			}

			_authorizationHeaderValue = string.Format("BEARER {0}", authInfo.AccessToken);
		}

		public void Authenticate(IRestClient client, IRestRequest request)
		{
			request.AddHeader("Authorization", _authorizationHeaderValue);
		}
	}
}
