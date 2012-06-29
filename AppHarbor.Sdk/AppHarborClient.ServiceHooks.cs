using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public ServiceHook GetServicehook(string applicationSlug, string Id)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.Resource = "applications/{applicationSlug}/servicehooks/{Id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("Id", Id, ParameterType.UrlSegment);

			return ExecuteGetKeyed<ServiceHook>(request);
		}

		public IEnumerable<ServiceHook> GetServicehooks(string applicationSlug)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.Resource = "applications/{applicationSlug}/servicehooks";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<ServiceHook>(request);
		}

		public CreateResult CreateServicehook(string applicationSlug, string url)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);
			CheckArgumentNull("url", url);

			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationSlug}/servicehooks";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddBody(new
			{
				url = url,
			});
			return ExecuteCreate(request);
		}

		public bool DeleteServicehook(string applicationSlug, string Id)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationSlug}/servicehooks/{Id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("Id", Id, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}
	}
}
