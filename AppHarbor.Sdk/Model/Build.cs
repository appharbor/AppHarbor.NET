using System;

namespace AppHarbor.Model
{
	public class Build : IUrl, IKeyed
	{
		public string Id
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}

		public DateTime Created
		{
			get;
			set;
		}

		public DateTime Deployed
		{
			get;
			set;
		}

		public Commit Commit
		{
			get;
			set;
		}

		public string DownloadUrl
		{
			get;
			set;
		}

		public string TestsUrl
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
