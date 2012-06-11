using System;
using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public Collaborator GetCollaborator(string applicationId, string id)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}/collaborators/{id}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("id", id, ParameterType.UrlSegment);

			return ExecuteGetKeyed<Collaborator>(request);
		}

		public IEnumerable<Collaborator> GetCollaborators(string applicationId)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest();
			request.Resource = "applications/{applicationId}/collaborators";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<Collaborator>(request);
		}

		public CreateResult CreateCollaborator(string applicationId, string email, CollaboratorType collaboratorType)
		{
			CheckArgumentNull("applicationId", applicationId);
			CheckArgumentNull("email", email);

			if (collaboratorType == CollaboratorType.None)
			{
				throw new ArgumentException("collaboratorType needs to be set.");
			}

			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationId}/collaborators";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddBody(new
			{
				collaboratorEmail = email,
				role = GetCollaboratorType(collaboratorType),
			});
			return ExecuteCreate(request);
		}

		public bool EditCollaborator(string applicationId, Collaborator collaborator)
		{
			CheckArgumentNull("applicationId", applicationId);
			CheckArgumentNull("collaborator", collaborator);
			CheckArgumentNull("collaborator.Role", collaborator.Role);

			if (collaborator.Role == CollaboratorType.None)
			{
				throw new ArgumentException("collaborator.Role has to be set.");
			}

			var request = new RestRequest(Method.PUT);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationId}/collaborators/{id}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("id", collaborator.Id, ParameterType.UrlSegment);
			request.AddBody(new
			{
				role = GetCollaboratorType(collaborator.Role),
			});
			return ExecuteEdit(request);
		}

		public bool DeleteCollaborator(string applicationId, string id)
		{
			CheckArgumentNull("applicationId", applicationId);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationId}/collaborators/{id}";
			request.AddParameter("applicationId", applicationId, ParameterType.UrlSegment);
			request.AddParameter("id", id, ParameterType.UrlSegment);

			return ExecuteDelete(request);
		}

		public static string GetCollaboratorType(CollaboratorType collaboratorType)
		{
			var name = Enum.GetName(typeof(CollaboratorType), collaboratorType)
				.ToLower();
			return name;
		}
	}
}
