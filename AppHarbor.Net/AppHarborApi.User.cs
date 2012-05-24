using AppHarbor.Model;
using RestSharp;

namespace AppHarbor
{
	public partial class AppHarborApi
	{
		public User GetUser()
		{
			var request = new RestRequest();
			request.Resource = "user";

			return ExecuteGet<User>(request);
		}
	}
}
