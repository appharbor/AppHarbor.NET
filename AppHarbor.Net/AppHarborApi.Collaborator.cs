using System;
using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborApi
	{
		public Collaborator GetCollaborator(string applicationID, long ID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}/collaborators/{ID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddParameter("ID", ID, ParameterType.UrlSegment);

			return ExecuteGetKeyed<Collaborator>(request);
		}

		public IList<Collaborator> GetCollaborators(string applicationID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest();
			request.Resource = "applications/{applicationID}/collaborators";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<Collaborator>(request);
		}

		public CreateResult<long> CreateCollaborator(string applicationID, string email, CollaboratorType collaboratorType)
		{
			CheckArgumentNull("applicationID", applicationID);
			CheckArgumentNull("email", email);

			if (collaboratorType == CollaboratorType.None)
				throw new ArgumentException("collaboratorType needs to be set.");

			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationID}/collaborators";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddBody(new
			{
				collaboratorEmail = email,
				role = Util.GetCollaboratorType(collaboratorType),
			});
			return ExecuteCreate(request);
		}

		public bool EditCollaborator(string applicationID, Collaborator collaborator)
		{
			CheckArgumentNull("applicationID", applicationID);
			CheckArgumentNull("collaborator", collaborator);
			CheckArgumentNull("collaborator.Role", collaborator.Role);

			if (collaborator.Role == CollaboratorType.None)
				throw new ArgumentException("collaborator.Role has to be set.");

			var request = new RestRequest(Method.PUT);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationID}/collaborators/{ID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddParameter("ID", collaborator.ID, ParameterType.UrlSegment);
			request.AddBody(new
			{
				role = Util.GetCollaboratorType(collaborator.Role),
			});
			return ExecuteEdit(request);
		}

		public bool DeleteCollaborator(string applicationID, long ID)
		{
			CheckArgumentNull("applicationID", applicationID);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationID}/collaborators/{ID}";
			request.AddParameter("applicationID", applicationID, ParameterType.UrlSegment);
			request.AddParameter("ID", ID, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}
	}
}
