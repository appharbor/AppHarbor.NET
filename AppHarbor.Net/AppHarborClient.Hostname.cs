using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public Hostname GetHostname(string applicationId, string Id)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}/hostnames/{Id}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("Id", Id, ParameterType.UrlSegment);

			return ExecuteGetKeyed<Hostname>(request);
		}

		public IEnumerable<Hostname> GetHostnames(string applicationId)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}/hostnames";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<Hostname>(request);
		}

		public CreateResult CreateHostname(string applicationId, string hostName)
		{
			CheckArgumentNull("applicationId", applicationId);
			CheckArgumentNull("hostName", hostName);

			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationId}/hostnames";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddBody(new
			{
				value = hostName,
			});
			return ExecuteCreate(request);
		}

		public bool DeleteHostname(string applicationId, string Id)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationId}/hostnames/{Id}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("Id", Id, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}
	}
}
