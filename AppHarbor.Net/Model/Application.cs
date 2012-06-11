using System;

namespace AppHarbor.Model
{
	public class Application : IUrl
	{
		public string Name
		{
			get;
			set;
		}

		public string Slug
		{
			get;
			set;
		}

		public string RegionIdentifier
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
