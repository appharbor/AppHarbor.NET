using System;
using System.Collections.Generic;

namespace AppHarbor.Model
{
	public class Test
	{
		public string Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}

		public string Kind
		{
			get;
			set;
		}

		public TimeSpan Duration
		{
			get;
			set;
		}

		public List<Test> Tests
		{
			get;
			set;
		}
	}
}
