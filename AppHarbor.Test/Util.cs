using System;
using System.IO;

namespace AppHarbor.Test
{
	public static class Util
	{
		public static string GetCurrentBasePath()
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}

		public static string GetDataPath()
		{
			var currentBasePath = GetCurrentBasePath();

			return Path.Combine(currentBasePath, "Data");
		}
	}
}
