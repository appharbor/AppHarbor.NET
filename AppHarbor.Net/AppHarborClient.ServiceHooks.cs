using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public ServiceHook GetServicehook(string applicationId, string Id)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}/servicehooks/{Id}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("Id", Id, ParameterType.UrlSegment);

			return ExecuteGetKeyed<ServiceHook>(request);
		}

		public IEnumerable<ServiceHook> GetServicehooks(string applicationId)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}/servicehooks";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<ServiceHook>(request);
		}

		public CreateResult CreateServicehook(string applicationId, string url)
		{
			CheckArgumentNull("applicationId", applicationId);
			CheckArgumentNull("url", url);

			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationId}/servicehooks";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddBody(new
			{
				url = url,
			});
			return ExecuteCreate(request);
		}

		public bool DeleteServicehook(string applicationId, string Id)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationId}/servicehooks/{Id}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("Id", Id, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}
	}
}
