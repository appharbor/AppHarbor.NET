using System;

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

		public Uri Url
		{
			get;
			set;
		}
	}
}
