using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public Test GetTest(string applicationId, string buildId, string Id)
		{
			CheckArgumentNull("applicationId", applicationId);
			CheckArgumentNull("Id", Id);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}/builds/{buildId}/tests/{Id}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("buildId", buildId, ParameterType.UrlSegment);
			request.AddParameter("Id", Id, ParameterType.UrlSegment);

			return ExecuteGet<Test>(request);
		}

		public IEnumerable<Test> GetTests(string applicationId, string buildId)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}/builds/{buildId}/tests";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("buildId", buildId, ParameterType.UrlSegment);

			return ExecuteGet<List<Test>>(request);
		}
	}
}
