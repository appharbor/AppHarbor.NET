using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public IEnumerable<Application> GetApplications()
		{
			var request = new RestRequest();
			request.Resource = "applications";

			return ExecuteGet<List<Application>>(request);
		}

		public Application GetApplication(string applicationId)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			return ExecuteGet<Application>(request);
		}

		/// <summary>
		/// Creates a new AppHarbor application
		/// </summary>
		/// <param name="name">Name of the application.</param>
		/// <param name="regionIdentifier">Region the application will be created in. The default is "amazon-web-services::us-east-1". May not be null.</param>
		/// <returns>Returns a create result. The Status of the CreateResult indicates whether the creation was successfull or not.</returns>
		public CreateResult CreateApplication(string name, string regionIdentifier = "amazon-web-services::us-east-1")
		{
			CheckArgumentNull("name", name);
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

		public bool EditApplication(string applicationId, Application application)
		{
			CheckArgumentNull("applicationId", applicationId);
			CheckArgumentNull("application", application);
			CheckArgumentNull("application.Name", application.Name);

			var request = new RestRequest(Method.PUT);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationId}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddBody(new
			{
				name = application.Name,
			});
			return ExecuteEdit(request);
		}

		public bool DeleteApplication(string applicationId)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationId}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}
	}
}
