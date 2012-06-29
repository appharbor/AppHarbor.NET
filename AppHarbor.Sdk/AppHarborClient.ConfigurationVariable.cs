using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public ConfigurationVariable GetConfigurationVariable(string applicationSlug, string id)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.Resource = "applications/{applicationSlug}/configurationvariables/{id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("id", id, ParameterType.UrlSegment);

			return ExecuteGetKeyed<ConfigurationVariable>(request);
		}

		public IEnumerable<ConfigurationVariable> GetConfigurationVariables(string applicationSlug)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.Resource = "applications/{applicationSlug}/configurationvariables";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<ConfigurationVariable>(request);
		}

		public CreateResult CreateConfigurationVariable(string applicationSlug, string key, string value)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);
			CheckArgumentNull("key", key);
			CheckArgumentNull("value", value);

			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationSlug}/configurationvariables";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddBody(new
			{
				key = key,
				value = value
			});
			return ExecuteCreate(request);
		}

		public bool EditConfigurationVariable(string applicationSlug, ConfigurationVariable configurationVariable)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);
			CheckArgumentNull("configurationVariable", configurationVariable);
			CheckArgumentNull("configurationVariable.Key ", configurationVariable.Key);
			CheckArgumentNull("configurationVariable.Value", configurationVariable.Value);

			var request = new RestRequest(Method.PUT);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationSlug}/configurationvariables/{id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("id", configurationVariable.Id, ParameterType.UrlSegment);
			request.AddBody(new
			{
				key = configurationVariable.Key,
				value = configurationVariable.Value
			});
			return ExecuteEdit(request);
		}

		public bool DeleteConfigurationVariable(string applicationSlug, string id)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationSlug}/configurationvariables/{id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("id", id, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}
	}
}
