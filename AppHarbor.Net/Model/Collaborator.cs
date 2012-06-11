using System;

namespace AppHarbor.Model
{
	public class Collaborator : IUrl, IKeyed
	{
		public string Id
		{
			get;
			set;
		}

		public CollaboratorUser User
		{
			get;
			set;
		}

		public CollaboratorType Role
		{
			get;
			set;
		}

		public Uri Url
		{
			get;
			set;
		}
	}
}
