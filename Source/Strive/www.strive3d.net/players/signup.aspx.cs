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

namespace www.strive3d.net.players
{
	/// <summary>
	/// Summary description for signup.
	/// </summary>
	public class signup : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox PlayerEmail;
		protected System.Web.UI.WebControls.TextBox PlayerPassword;
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator1;
		protected System.Web.UI.WebControls.Panel signupform;
		protected System.Web.UI.WebControls.Panel signupsuccess;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			signupform.Visible = true;
			signupsuccess.Visible = false;
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
			this.Button1.Click += new System.EventHandler(this.Button1_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Button1_Click(object sender, System.EventArgs e)
		{
			if(this.Page.IsValid)
			{
			Player.Create(PlayerEmail.Text, PlayerPassword.Text);
			signupform.Visible = false;
			signupsuccess.Visible = true;
	}

		}
	}
}
