using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public Test GetTest(string applicationSlug, string buildId, string Id)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);
			CheckArgumentNull("Id", Id);

			var request = new RestRequest();
			request.Resource = "applications/{applicationSlug}/builds/{buildId}/tests/{Id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("buildId", buildId, ParameterType.UrlSegment);
			request.AddParameter("Id", Id, ParameterType.UrlSegment);

			return ExecuteGet<Test>(request);
		}

		public IEnumerable<Test> GetTests(string applicationSlug, string buildId)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.Resource = "applications/{applicationSlug}/builds/{buildId}/tests";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("buildId", buildId, ParameterType.UrlSegment);

			return ExecuteGet<List<Test>>(request);
		}
	}
}
