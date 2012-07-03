using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public Hostname GetHostname(string applicationSlug, string Id)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.Resource = "applications/{applicationSlug}/hostnames/{Id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("Id", Id, ParameterType.UrlSegment);

			return ExecuteGetKeyed<Hostname>(request);
		}

		public IEnumerable<Hostname> GetHostnames(string applicationSlug)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.Resource = "applications/{applicationSlug}/hostnames";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<Hostname>(request);
		}

		public CreateResult CreateHostname(string applicationSlug, string hostName)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);
			CheckArgumentNull("hostName", hostName);

			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationSlug}/hostnames";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddBody(new
			{
				value = hostName,
			});
			return ExecuteCreate(request);
		}

		public bool DeleteHostname(string applicationSlug, string Id)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationSlug}/hostnames/{Id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("Id", Id, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}
	}
}
