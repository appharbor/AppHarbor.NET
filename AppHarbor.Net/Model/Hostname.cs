using System;

namespace AppHarbor.Model
{
	public class Hostname : IUrl, IKeyed
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

		public bool Canonical
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
