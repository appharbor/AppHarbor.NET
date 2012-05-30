using System.Configuration;

namespace AppHarbor
{
	public class Config
	{
		public static string ClientId
		{
			get
			{
				return ConfigurationManager.AppSettings["ClientId"];
			}
		}

		public static string ClientSecret
		{
			get
			{
				return ConfigurationManager.AppSettings["ClientSecret"];
			}
		}

		public static string ClientCallbackUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["ClientCallbackUrl"];
			}
		}

		public static string ClientUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["ClientUrl"];
			}
		}
	}
}
