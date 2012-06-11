namespace AppHarbor.Model
{
	public class Exception
	{
		public string Stack_Trace
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public Exception Inner_Exception
		{
			get;
			set;
		}
	}
}
