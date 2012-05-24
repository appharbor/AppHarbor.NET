using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborApi
	{
		public IList<Application> GetApplications()
		{
			var request = new RestRequest();
			request.Resource = "applications";

			return ExecuteGet<List<Application>>(request);
		}

		public Application GetApplication(string applicationID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			return ExecuteGet<Application>(request);
		}

		/// <summary>
		/// Creates a new AppHarbor application
		/// </summary>
		/// <param name="name">Name of the application.</param>
		/// <param name="regionIdentifier">Region the application will be created in. If null, it will default to "amazon-web-services::us-east-1".</param>
		/// <returns>Returns a create result. The Status of the CreateResult indicates whether the creation was successfull or not.</returns>
		public CreateResult<string> CreateApplication(string name, string regionIdentifier)
		{
			CheckArgumentNull("name", name);

			if (regionIdentifier == null)
				regionIdentifier = "amazon-web-services::us-east-1";

			CheckArgumentNull("regionIdentifier", regionIdentifier);

			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications";
			request.AddBody(new
			{
				name = name,
				region_identifier = regionIdentifier,
			});

			return ExecuteCreateApplication(request);
		}

		public bool EditApplication(string applicationID, Application application)
		{
			CheckArgumentNull("applicationID", applicationID);
			CheckArgumentNull("application", application);
			CheckArgumentNull("application.Name", application.Name);

			var request = new RestRequest(Method.PUT);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddBody(new
			{
				name = application.Name,
			});
			return ExecuteEdit(request);
		}

		public bool DeleteApplication(string applicationID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}
	}
}
