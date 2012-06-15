using System.Collections.Generic;

namespace AppHarbor.Model
{
	public class User
	{
		public string Id
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		public List<string> Email_Addresses
		{
			get;
			set;
		}
	}
}
