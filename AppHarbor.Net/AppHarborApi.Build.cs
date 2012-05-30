using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborApi
	{
		public Build GetBuild(string applicationID, string ID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}/builds/{ID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddParameter("ID", ID, ParameterType.UrlSegment);

			return ExecuteGetKeyed<Build>(request);
		}

		public IList<Build> GetBuilds(string applicationID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}/builds";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<Build>(request);
		}
	}
}
