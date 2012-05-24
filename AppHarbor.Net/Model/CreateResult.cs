namespace AppHarbor.Model
{
	public class CreateResult<T>
	{
		public CreateStatus Status
		{
			get;
			set;
		}

		public T ID
		{
			get;
			set;
		}

		public string Location
		{
			get;
			set;
		}
	}
}
