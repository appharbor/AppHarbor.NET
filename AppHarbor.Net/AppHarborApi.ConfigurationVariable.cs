using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborApi
	{
		public ConfigurationVariable GetConfigurationVariable(string applicationID, string ID)
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

		public CreateResult<string> CreateConfigurationVariable(string applicationID, string key, string value)
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

		public bool DeleteConfigurationVariable(string applicationID, string ID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationID}/configurationvariables/{ID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddParameter("ID", ID, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}
	}
}
