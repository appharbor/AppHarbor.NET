namespace AppHarbor.Model
{
	public class Collaborator : IUrl, IKeyed
	{
		public string Id
		{
			get;
			set;
		}

		public UserClass User
		{
			get;
			set;
		}

		public CollaboratorType Role
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public class UserClass
		{
			public string Id
			{
				get;
				set;
			}

			public string Name
			{
				get;
				set;
			}
		}
	}
}
