using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public Error GetError(string applicationSlug, string id)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.DateFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'";
			request.Resource = "applications/{applicationSlug}/errors/{Id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("Id", id, ParameterType.UrlSegment);

			return ExecuteGetKeyed<Error>(request);
		}

		public IEnumerable<Error> GetErrors(string applicationSlug)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.DateFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'";
			request.Resource = "applications/{applicationSlug}/errors";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<Error>(request);
		}
	}
}
