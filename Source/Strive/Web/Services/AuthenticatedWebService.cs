using System;

namespace Strive.Web.Services
{
	/// <summary>
	/// Summary description for AuthenticatedWebService.
	/// </summary>
	public class AuthenticatedWebService : System.Web.Services.WebService
	{
		protected string AUTH_USER;
		protected string AUTH_PASSWORD;

		public bool AuthenticateRequest()
		{
			if(this.Context.Request.Headers["Authorization"] != null &&
				this.Context.Request.Headers["Authorization"].StartsWith("Basic"))
			{
				// Wow: who writes shameful lines of code like this? I DO!
				// The HTTP authorization header will look like this:
				// "Basic base64encodedcolonseperatedusernameandpassword"
				byte[] authenticationBytes = Convert.FromBase64String(this.Context.Request.Headers["Authorization"].ToString().Replace("Basic", "").Trim());
				string authenticationInfo = System.Text.ASCIIEncoding.ASCII.GetString(authenticationBytes);
				AUTH_USER = System.Web.HttpUtility.UrlDecode(authenticationInfo.Substring(0, authenticationInfo.IndexOf(":")));
				AUTH_PASSWORD = System.Web.HttpUtility.UrlDecode(authenticationInfo.Substring(authenticationInfo.IndexOf(":") + 1));
				return true;
				

			}
			else
			{
				this.Context.Response.StatusCode = 401;
				this.Context.Response.AddHeader("WWW-Authenticate", "Basic");
				this.Context.Response.End();
				return false;
			}
		}



	}
}
