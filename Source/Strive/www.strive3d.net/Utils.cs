using System;

namespace www.strive3d.net
{
	/// <summary>
	/// Summary description for Utils.
	/// </summary>
	public class Utils
	{
		public Utils()
		{

		}
		public static string ApplicationPath
		{
			get
			{
				return System.Web.HttpContext.Current.Request.ApplicationPath.EndsWith("/") ? System.Web.HttpContext.Current.Request.ApplicationPath.Substring(0, System.Web.HttpContext.Current.Request.ApplicationPath.Length -1) : System.Web.HttpContext.Current.Request.ApplicationPath;
			}
		}
	}
}
