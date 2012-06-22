using System;
using System.IO;
using System.Reflection;
using System.Security.Authentication;

namespace AppHarbor.ConsoleSample
{
	public class Program
	{
		public static void Main(string[] arguments)
		{
			CheckUsage(arguments);

			string clientId = arguments[0];
			string clientSecret = arguments[1];

			Console.WriteLine("Please authorize this application in the browser window that has just opened");
			var authInfo = GetAuthorization(clientId, clientSecret);

			Console.WriteLine("Authorization successful");
			var client = new AppHarborClient(authInfo);

			Console.WriteLine("Applications:");
			foreach (var application in client.GetApplications())
			{
				Console.WriteLine(" - {0}", application.Name);
			}
		}

		private static void CheckUsage(string[] arguments)
		{
			if (arguments.Length != 2)
			{
				Console.WriteLine("Usage: {0} [clientId] [clientSecret]", ExecutableName);
				Environment.Exit(1);
			}
		}

		private static AuthInfo GetAuthorization(string clientId, string clientSecret)
		{
			try
			{
				return AppHarborClient.AskForAuthorization(clientId, clientSecret, TimeSpan.FromMinutes(1));
			}
			catch (AuthenticationException)
			{
				Console.WriteLine("Failed to get authorization");
				Environment.Exit(-1);
				throw;
			}
			catch (TimeoutException)
			{
				Console.WriteLine("Timeout, you have to be faster than that");
				Environment.Exit(-1);
				throw;
			}
		}

		private static string ExecutableName
		{
			get
			{
				return Path.GetFileName(Assembly.GetEntryAssembly().Location);
			}
		}
	}
}
