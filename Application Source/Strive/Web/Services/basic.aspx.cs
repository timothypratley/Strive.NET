using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Strive.Web.Services
{
	/// <summary>
	/// Summary description for basic.
	/// </summary>
	public class basic : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			
			if(Request.Headers["Authorization"] != null &&
				Request.Headers["Authorization"].StartsWith("Basic"))
			{
				// Wow: who writes shameful lines of code like this? I DO!
				// The HTTP authorization header will look like this:
				// "Basic base64encodedcolonseperatedusernameandpassword"
				byte[] authenticationBytes = Convert.FromBase64String(Request.Headers["Authorization"].ToString().Replace("Basic", "").Trim());
				string authenticationInfo = System.Text.ASCIIEncoding.ASCII.GetString(authenticationBytes);
				string AUTH_USER = System.Web.HttpUtility.UrlDecode(authenticationInfo.Substring(0, authenticationInfo.IndexOf(":")));
				string AUTH_PASSWORD = System.Web.HttpUtility.UrlDecode(authenticationInfo.Substring(authenticationInfo.IndexOf(":") + 1));

				Response.Write(AUTH_USER + "<hr />" + AUTH_PASSWORD);

			}
			else
			{
				Response.StatusCode = 401;
				Response.AddHeader("WWW-Authenticate", "Basic");
				Response.End();
				return;
			}

			
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
