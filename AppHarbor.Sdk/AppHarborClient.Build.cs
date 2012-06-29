using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public Build GetBuild(string applicationSlug, string id)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.Resource = "applications/{applicationSlug}/builds/{id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("id", id, ParameterType.UrlSegment);

			return ExecuteGetKeyed<Build>(request);
		}

		public IEnumerable<Build> GetBuilds(string applicationSlug)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.Resource = "applications/{applicationSlug}/builds";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<Build>(request);
		}
	}
}
