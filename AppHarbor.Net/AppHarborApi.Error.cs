using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborApi
	{
		public Error GetError(string applicationId, string id)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}/errors/{Id}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("Id", id, ParameterType.UrlSegment);

			return ExecuteGetKeyed<Error>(request);
		}

		public IList<Error> GetErrors(string applicationId)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}/errors";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<Error>(request);
		}
	}
}
