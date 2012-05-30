namespace AppHarbor.Model
{
	public class ConfigurationVariable : IUrl, IKeyed
	{
		public string Id
		{
			get;
			set;
		}

		public string Key
		{
			get;
			set;
		}

		public string Value
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
