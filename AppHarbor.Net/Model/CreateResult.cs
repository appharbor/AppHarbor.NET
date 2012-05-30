namespace AppHarbor.Model
{
	public class CreateResult<T>
	{
		public CreateStatus Status
		{
			get;
			set;
		}

		public T Id
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
