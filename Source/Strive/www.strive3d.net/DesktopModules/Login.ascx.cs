namespace www.strive3d.net
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
using www.strive3d.net.Game;
	/// <summary>
	///		Summary description for Login.
	/// </summary>
	public abstract class Login : System.Web.UI.UserControl
	{

		bool _loginFailed = false;

		public bool LoginFailed
		{
			get
			{
				return _loginFailed;
			}
		}

		public string UserName
		{
			get
			{
				return thisterminal.Web.Authentication.Basic.LoggedOnUserID;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(this.Page.Request.QueryString["LoginRequested"] == "true" &&
				!PlayerAuthenticator.CurrentPlayerLoggedIn 	)
			{
				bool LoggedIn = thisterminal.Web.Authentication.Basic.LogonCurrentUser(new PlayerAuthenticator(), "www.strive3d.net");
				if(!LoggedIn)
				{
					_loginFailed = true;
				}
				else
				{


					if(this.Request.QueryString["Referer"]  != null &&
						this.Request.QueryString["Referer"].ToString() != String.Empty)
					{
						Response.Redirect(this.Request.QueryString["Referer"].ToString().Replace("LoginRequested=true", ""));  
					}
					else
					if(this.Request.ServerVariables["HTTP_REFERER"]  != null)
					{
						Response.Redirect(this.Request.ServerVariables["HTTP_REFERER"].ToString().Replace("LoginRequested=true", ""));  
					}
				}
			}
			else
			{
				if(this.Page.Request["LogoutRequested"] == "true")
				{
					PlayerAuthenticator.LogoutCurrentPlayer();
				}
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
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
