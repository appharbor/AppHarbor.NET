namespace AppHarbor.Model
{
	public class Hostname : IUrl, IKeyed
	{
		public string ID
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		public bool Canonical
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}
	}
}
