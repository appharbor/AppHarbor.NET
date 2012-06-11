using System;

namespace AppHarbor.Model
{
	public class Error : IKeyed, IUrl
	{
		public string Id
		{
			get;
			set;
		}

		public string Commit_Id
		{
			get;
			set;
		}

		public DateTime Date
		{
			get;
			set;
		}

		public string Request_Path
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public Exception Exception
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
