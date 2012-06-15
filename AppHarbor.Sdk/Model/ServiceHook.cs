using System;

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

		public Uri Url
		{
			get;
			set;
		}
	}
}
