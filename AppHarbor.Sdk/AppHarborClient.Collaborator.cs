using System;
using System.Collections.Generic;
using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborClient
	{
		public Collaborator GetCollaborator(string applicationSlug, string id)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.Resource = "applications/{applicationSlug}/collaborators/{id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("id", id, ParameterType.UrlSegment);

			return ExecuteGetKeyed<Collaborator>(request);
		}

		public IEnumerable<Collaborator> GetCollaborators(string applicationSlug)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest();
			request.Resource = "applications/{applicationSlug}/collaborators";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);

			return ExecuteGetListKeyed<Collaborator>(request);
		}

		public CreateResult CreateCollaborator(string applicationSlug, string email, CollaboratorType collaboratorType)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);
			CheckArgumentNull("email", email);

			if (collaboratorType == CollaboratorType.None)
			{
				throw new ArgumentException("collaboratorType needs to be set.");
			}

			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationSlug}/collaborators";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddBody(new
			{
				collaboratorEmail = email,
				role = GetCollaboratorType(collaboratorType),
			});
			return ExecuteCreate(request);
		}

		public bool EditCollaborator(string applicationSlug, Collaborator collaborator)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);
			CheckArgumentNull("collaborator", collaborator);
			CheckArgumentNull("collaborator.Role", collaborator.Role);

			if (collaborator.Role == CollaboratorType.None)
			{
				throw new ArgumentException("collaborator.Role has to be set.");
			}

			var request = new RestRequest(Method.PUT);
			request.RequestFormat = DataFormat.Json;
			request.Resource = "applications/{applicationSlug}/collaborators/{id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
			request.AddParameter("id", collaborator.Id, ParameterType.UrlSegment);
			request.AddBody(new
			{
				role = GetCollaboratorType(collaborator.Role),
			});
			return ExecuteEdit(request);
		}

		public bool DeleteCollaborator(string applicationSlug, string id)
		{
			CheckArgumentNull("applicationSlug", applicationSlug);

			var request = new RestRequest(Method.DELETE);
			request.Resource = "applications/{applicationSlug}/collaborators/{id}";
			request.AddParameter("applicationSlug", applicationSlug, ParameterType.UrlSegment);
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
