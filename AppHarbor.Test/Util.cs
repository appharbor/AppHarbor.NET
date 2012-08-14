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
			return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Data");
		}
	}
}
