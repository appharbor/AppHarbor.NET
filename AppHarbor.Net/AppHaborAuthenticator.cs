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
using System.Text;

namespace AppHarbor
{
    [Obsolete("Instead use HeaderAppHaborAuthenticator")]       
    public class QueryStringAppHaborAuthenticator : RestSharp.IAuthenticator
    {
        private readonly string _AuthorizationQueryString;

        public QueryStringAppHaborAuthenticator(AuthInfo authInfo)
        {
            if (authInfo == null)
                throw new ArgumentNullException("authInfo");

            if (authInfo.AccessToken == null)
                throw new ArgumentNullException("authInfo.AccessToken");
            _AuthorizationQueryString = string.Format("?access_token={0}", authInfo.AccessToken);
        }

        public void Authenticate(RestSharp.IRestClient client, RestSharp.IRestRequest request)
        {
            request.Resource += _AuthorizationQueryString;
        }
    }

    public class HeaderAppHaborAuthenticator : RestSharp.IAuthenticator
    {
        private readonly string _AuthorizationHeaderValue;

        public HeaderAppHaborAuthenticator(AuthInfo authInfo)
        {
            if (authInfo == null)
                throw new ArgumentNullException("authInfo");

            if (authInfo.AccessToken == null)
                throw new ArgumentNullException("authInfo.AccessToken");

            _AuthorizationHeaderValue = string.Format("BEARER {0}", authInfo.AccessToken);
        }

        public void Authenticate(RestSharp.IRestClient client, RestSharp.IRestRequest request)
        {
            request.AddHeader("Authorization", _AuthorizationHeaderValue);
        }
    }
}
