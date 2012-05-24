using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborApi
	{
		public ServiceHook GetServicehook(string applicationID, long ID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}/servicehooks/{ID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddParameter("ID", ID, ParameterType.UrlSegment);

			return ExecuteGetKeyed<ServiceHook>(request);
		}

		public IList<ServiceHook> GetServicehooks(string applicationID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}/servicehooks";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<ServiceHook>(request);
		}

		public CreateResult<long> CreateServicehook(string applicationID, string url)
		{
			CheckArgumentNull("applicationID", applicationID);
			CheckArgumentNull("url", url);

			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationID}/servicehooks";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddBody(new
			{
				url = url,
			});
			return ExecuteCreate(request);
		}

		public bool DeleteServicehook(string applicationID, long ID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationID}/servicehooks/{ID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddParameter("ID", ID, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}
	}
}
