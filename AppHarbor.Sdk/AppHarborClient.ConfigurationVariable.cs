using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public ConfigurationVariable GetConfigurationVariable(string applicationId, string id)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}/configurationvariables/{id}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("id", id, ParameterType.UrlSegment);

			return ExecuteGetKeyed<ConfigurationVariable>(request);
		}

		public IEnumerable<ConfigurationVariable> GetConfigurationVariables(string applicationId)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}/configurationvariables";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<ConfigurationVariable>(request);
		}

		public CreateResult CreateConfigurationVariable(string applicationId, string key, string value)
		{
			CheckArgumentNull("applicationId", applicationId);
			CheckArgumentNull("key", key);
			CheckArgumentNull("value", value);

			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationId}/configurationvariables";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddBody(new
			{
				key = key,
				value = value
			});
			return ExecuteCreate(request);
		}

		public bool EditConfigurationVariable(string applicationId, ConfigurationVariable configurationVariable)
		{
			CheckArgumentNull("applicationId", applicationId);
			CheckArgumentNull("configurationVariable", configurationVariable);
			CheckArgumentNull("configurationVariable.Key ", configurationVariable.Key);
			CheckArgumentNull("configurationVariable.Value", configurationVariable.Value);

			var request = new RestRequest(Method.PUT);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationId}/configurationvariables/{id}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("id", configurationVariable.Id, ParameterType.UrlSegment);
			request.AddBody(new
			{
				key = configurationVariable.Key,
				value = configurationVariable.Value
			});
			return ExecuteEdit(request);
		}

		public bool DeleteConfigurationVariable(string applicationId, string id)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationId}/configurationvariables/{id}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("id", id, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}
	}
}
