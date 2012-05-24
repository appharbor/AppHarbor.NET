using System.Configuration;

namespace AppHarbor
{
	public class Config
	{
		public static string ClientID
		{
			get
			{
				return ConfigurationManager.AppSettings["ClientID"];
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
