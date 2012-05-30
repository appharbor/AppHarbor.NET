namespace AppHarbor.Model
{
	public class ServiceHook : IUrl, IKeyed
	{
		public string Id
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
