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
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
    public class AppHarborApi
    {
        #region Public Static Methods

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

        #endregion

        #region Base

        const string BaseUrl = "https://appharbor.com/";

        private RestClient _Client = null;
        private Uri _BaseUri = null;
        private readonly CustomJsonDeserializer _JsonDeserializer = null;

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
            _JsonDeserializer = new CustomJsonDeserializer();

            // use the customized JsonDeseriilizer that can handle TimeSpan etc.
            _Client.AddHandler("application/json", _JsonDeserializer);
            _Client.AddHandler("text/json", _JsonDeserializer);
            _Client.AddHandler("text/x-json", _JsonDeserializer);
            _Client.AddHandler("text/javascript", _JsonDeserializer);
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

            data.ID = ExtractLongID(response.ResponseUri.LocalPath);

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
                item.ID = ExtractLongID(item.Url);
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

        private CreateResult<T> ExecuteCreate<T>(RestRequest request,Func<string, T> extractID)
        {
            var response = _Client.Execute(request);

            if (response == null)
                throw new ArgumentException("Response cannot be null.");

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                return new CreateResult<T>() { Status = CreateStatus.AlreadyExists };

            if (response.StatusCode != System.Net.HttpStatusCode.Created)
                return new CreateResult<T>() { Status = CreateStatus.Undefined };

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

        #endregion

        #region Application

        public IList<Application> GetApplications()
        {
            var request = new RestRequest();
            request.Resource = "applications";

            return ExecuteGet<List<Application>>(request);
        }

        public Application GetApplication(string applicationID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            return ExecuteGet<Application>(request);
        }

        /// <summary>
        /// Creates a new AppHarbor application
        /// </summary>
        /// <param name="name">Name of the application.</param>
        /// <param name="regionIdentifier">Region the application will be created in. If null, it will default to "amazon-web-services::us-east-1".</param>
        /// <returns>Returns a create result. The Status of the CreateResult indicates whether the creation was successfull or not.</returns>
        public CreateResult<string> CreateApplication(string name, string regionIdentifier)
        {
            CheckArgumentNull("name", name);

            if (regionIdentifier == null)
                regionIdentifier = "amazon-web-services::us-east-1";

            CheckArgumentNull("regionIdentifier", regionIdentifier);

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.Resource = "applications";
            request.AddBody(new
            {
                name = name,
                region_identifier = regionIdentifier,
            });

            return ExecuteCreateApplication(request);
        }

        public bool EditApplication(string applicationID, Application application)
        {
            CheckArgumentNull("applicationID", applicationID);
            CheckArgumentNull("application", application);
            CheckArgumentNull("application.Name", application.Name);

            var request = new RestRequest(Method.PUT);
            request.RequestFormat = DataFormat.Json;
            request.Resource = "applications/{applicationID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddBody(new
            {
                name = application.Name,
            });
            return ExecuteEdit(request);
        }

        public bool DeleteApplication(string applicationID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest(Method.DELETE);
            request.Resource = "applications/{applicationID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

            return ExecuteDelete(request);
        }

        #endregion

        #region Collaborator

        public Collaborator GetCollaborator(string applicationID, long ID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/collaborators/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("ID", ID, ParameterType.UrlSegment);

            return ExecuteGetKeyed<Collaborator>(request);
        }

        public IList<Collaborator> GetCollaborators(string applicationID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/collaborators";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

            return ExecuteGetListKeyed<Collaborator>(request);
        }

        public CreateResult<long> CreateCollaborator(string applicationID, string email, CollaboratorType collaboratorType)
        {
            CheckArgumentNull("applicationID", applicationID);
            CheckArgumentNull("email", email);

            if (collaboratorType == CollaboratorType.None)
                throw new ArgumentException("collaboratorType needs to be set.");

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.Resource = "applications/{applicationID}/collaborators";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddBody(new
            {
                collaboratorEmail = email,
                role = Util.GetCollaboratorType(collaboratorType),
            });
            return ExecuteCreate(request);
        }

        public bool EditCollaborator(string applicationID, Collaborator collaborator)
        {
            CheckArgumentNull("applicationID", applicationID);
            CheckArgumentNull("collaborator", collaborator);
            CheckArgumentNull("collaborator.Role", collaborator.Role);

            if (collaborator.Role == CollaboratorType.None)
                throw new ArgumentException("collaborator.Role has to be set.");

            var request = new RestRequest(Method.PUT);
            request.RequestFormat = DataFormat.Json;
            request.Resource = "applications/{applicationID}/collaborators/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("ID", collaborator.ID, ParameterType.UrlSegment);
            request.AddBody(new
            {
                role = Util.GetCollaboratorType(collaborator.Role),
            });
            return ExecuteEdit(request);
        }

        public bool DeleteCollaborator(string applicationID, long ID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest(Method.DELETE);
            request.Resource = "applications/{applicationID}/collaborators/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("ID", ID, ParameterType.UrlSegment);

            return ExecuteDelete(request);
        }

        #endregion

        #region ConfigurationVariable

        public ConfigurationVariable GetConfigurationVariable(string applicationID, long ID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/configurationvariables/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("ID", ID, ParameterType.UrlSegment);

            return ExecuteGetKeyed<ConfigurationVariable>(request);
        }

        public IList<ConfigurationVariable> GetConfigurationVariables(string applicationID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/configurationvariables";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

            return ExecuteGetListKeyed<ConfigurationVariable>(request);
        }

        public CreateResult<long> CreateConfigurationVariable(string applicationID, string key, string value)
        {
            CheckArgumentNull("applicationID", applicationID);
            CheckArgumentNull("key", key);
            CheckArgumentNull("value", value);

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.Resource = "applications/{applicationID}/configurationvariables";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddBody(new
            {
                key = key,
                value = value
            });
            return ExecuteCreate(request);
        }

        public bool EditConfigurationVariable(string applicationID, ConfigurationVariable configurationVariable)
        {
            CheckArgumentNull("applicationID", applicationID);
            CheckArgumentNull("configurationVariable", configurationVariable);
            CheckArgumentNull("configurationVariable.Key ", configurationVariable.Key);
            CheckArgumentNull("configurationVariable.Value", configurationVariable.Value);

            var request = new RestRequest(Method.PUT);
            request.RequestFormat = DataFormat.Json;
            request.Resource = "applications/{applicationID}/configurationvariables/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("ID", configurationVariable.ID, ParameterType.UrlSegment);
            request.AddBody(new
            {
                key = configurationVariable.Key,
                value = configurationVariable.Value
            });
            return ExecuteEdit(request);
        }

        public bool DeleteConfigurationVariable(string applicationID, long ID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest(Method.DELETE);
            request.Resource = "applications/{applicationID}/configurationvariables/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("ID", ID, ParameterType.UrlSegment);

            return ExecuteDelete(request);
        }

        #endregion

        #region Hostname

        public Hostname GetHostname(string applicationID, long ID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/hostnames/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("ID", ID, ParameterType.UrlSegment);

            return ExecuteGetKeyed<Hostname>(request);
        }

        public IList<Hostname> GetHostnames(string applicationID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/hostnames";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

            return ExecuteGetListKeyed<Hostname>(request);
        }

        public CreateResult<long> CreateHostname(string applicationID, string hostName)
        {
            CheckArgumentNull("applicationID", applicationID);
            CheckArgumentNull("hostName", hostName);

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.Resource = "applications/{applicationID}/hostnames";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddBody(new
            {
                value = hostName,
            });
            return ExecuteCreate(request);
        }

        public bool DeleteHostname(string applicationID, long ID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest(Method.DELETE);
            request.Resource = "applications/{applicationID}/hostnames/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("ID", ID, ParameterType.UrlSegment);

            return ExecuteDelete(request);
        }

        #endregion

        #region Servicehook

        public ServiceHook GetServicehook(string applicationID, long ID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/servicehooks/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("ID", ID, ParameterType.UrlSegment);

            return ExecuteGetKeyed<ServiceHook>(request);
        }

        public IList<ServiceHook> GetServicehooks(string applicationID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/servicehooks";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

            return ExecuteGetListKeyed<ServiceHook>(request);
        }

        public CreateResult<long> CreateServicehook(string applicationID, string url)
        {
            CheckArgumentNull("applicationID", applicationID);
            CheckArgumentNull("url", url);

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.Resource = "applications/{applicationID}/servicehooks";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddBody(new
            {
                url = url,
            });
            return ExecuteCreate(request);
        }

        public bool DeleteServicehook(string applicationID, long ID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest(Method.DELETE);
            request.Resource = "applications/{applicationID}/servicehooks/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("ID", ID, ParameterType.UrlSegment);

            return ExecuteDelete(request);
        }

        #endregion

        #region Build

        public Build GetBuild(string applicationID, long ID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/builds/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("ID", ID, ParameterType.UrlSegment);

            return ExecuteGetKeyed<Build>(request);
        }

        public IList<Build> GetBuilds(string applicationID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/builds";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

            return ExecuteGetListKeyed<Build>(request);
        }

        #endregion

        #region Test

        public Test GetTest(string applicationID, long buildID, string ID)
        {
            CheckArgumentNull("applicationID", applicationID);
            CheckArgumentNull("ID", ID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/builds/{buildID}/tests/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("buildID", buildID, ParameterType.UrlSegment);
            request.AddParameter("ID", ID, ParameterType.UrlSegment);

            return ExecuteGet<Test>(request);
        }

        public IList<Test> GetTests(string applicationID, long buildID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/builds/{buildID}/tests";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("buildID", buildID, ParameterType.UrlSegment);

            return ExecuteGet<List<Test>>(request);
        }

        #endregion

        #region Error

        public Error GetError(string applicationID, long ID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/errors/{ID}";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
            request.AddParameter("ID", ID, ParameterType.UrlSegment);

            return ExecuteGetKeyed<Error>(request);
        }

        public IList<Error> GetErrors(string applicationID)
        {
            CheckArgumentNull("applicationID", applicationID);

            var request = new RestRequest();
            request.Resource = "applications/{applicationID}/errors";
            request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

            return ExecuteGetListKeyed<Error>(request);
        }

        #endregion

        #region User

        public User GetUser()
        {
            var request = new RestRequest();
            request.Resource = "user";

            return ExecuteGet<User>(request);
        }

        #endregion
    }
}
