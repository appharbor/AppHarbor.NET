using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborApi
	{
		public Test GetTest(string applicationID, long buildID, string ID)
		{
			CheckArgumentNull("applicationID", applicationID);
			CheckArgumentNull("ID", ID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}/builds/{buildID}/tests/{ID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddParameter("buildID", buildID, ParameterType.UrlSegment);
			request.AddParameter("ID", ID, ParameterType.UrlSegment);

			return ExecuteGet<Test>(request);
		}

		public IList<Test> GetTests(string applicationID, long buildID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}/builds/{buildID}/tests";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddParameter("buildID", buildID, ParameterType.UrlSegment);

			return ExecuteGet<List<Test>>(request);
		}
	}
}
