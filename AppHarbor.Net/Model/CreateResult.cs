using System;

namespace AppHarbor.Model
{
	public class CreateResult
	{
		public CreateStatus Status
		{
			get;
			set;
		}

		public string Id
		{
			get;
			set;
		}

		public Uri Location
		{
			get;
			set;
		}
	}
}
