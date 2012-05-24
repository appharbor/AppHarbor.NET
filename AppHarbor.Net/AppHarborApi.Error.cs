using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborApi
	{
		public Error GetError(string applicationID, long ID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}/errors/{ID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddParameter("ID", ID, ParameterType.UrlSegment);

			return ExecuteGetKeyed<Error>(request);
		}

		public IList<Error> GetErrors(string applicationID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}/errors";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<Error>(request);
		}
	}
}
