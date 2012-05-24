using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborApi
	{
		public Hostname GetHostname(string applicationID, long ID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}/hostnames/{ID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddParameter("ID", ID, ParameterType.UrlSegment);

			return ExecuteGetKeyed<Hostname>(request);
		}

		public IList<Hostname> GetHostnames(string applicationID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}/hostnames";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<Hostname>(request);
		}

		public CreateResult<long> CreateHostname(string applicationID, string hostName)
		{
			CheckArgumentNull("applicationID", applicationID);
			CheckArgumentNull("hostName", hostName);

			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationID}/hostnames";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddBody(new
			{
				value = hostName,
			});
			return ExecuteCreate(request);
		}

		public bool DeleteHostname(string applicationID, long ID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationID}/hostnames/{ID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddParameter("ID", ID, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}
	}
}
