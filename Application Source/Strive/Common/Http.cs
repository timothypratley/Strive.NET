using System;
using System.IO;
using System.Net;

namespace Strive.Common
{
	/// <summary>
	/// Summary description for Http.
	/// </summary>
	public class Http
	{
		private static System.Net.WebClient _webClient = new System.Net.WebClient();


		public static void SaveUrlTargetToDisk(Uri url, string path)
		{
			if(!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(path)))
			{
				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
			}
			_webClient.DownloadFile(url.ToString(), path);
		}

		public static bool UrlTargetExists(Uri url)
		{
			return true;
		}

	}
}
