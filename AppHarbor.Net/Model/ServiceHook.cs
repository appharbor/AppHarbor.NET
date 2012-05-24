namespace AppHarbor.Model
{
	public class ServiceHook : IUrl, IKeyed
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

		public string Url
		{
			get;
			set;
		}
	}
}
