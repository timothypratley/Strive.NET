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
using System.Data.SqlClient;
using www.strive3d.net.Game;
namespace www.strive3d.net.players.builders.textures
{
	/// <summary>
	/// Summary description for addtexture.
	/// </summary>
	public class addtexture : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox ModelName;
		protected System.Web.UI.WebControls.Button Add;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
		protected System.Web.UI.WebControls.Label BitmapWarning;
		protected System.Web.UI.WebControls.TextBox ModelDescription;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
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
			this.Add.Click += new System.EventHandler(this.Add_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Add_Click(object sender, System.EventArgs e)
		{
			// Write file to file system
			if(Request.Files[0] == null ||
				!Request.Files[0].FileName.EndsWith(".bmp")
				)
			{
				BitmapWarning.Text = "You must select a bitmap.";
				return;
			}
			else
			{
				BitmapWarning.Text = "";
			}

			CommandFactory cmd = new CommandFactory();
			SqlCommand c = cmd.CreateModel(ModelName.Text,
				1,
				ModelDescription.Text);

			int ModelID = (int)c.ExecuteScalar();

			string modelsaveaspath = ".." + System.Configuration.ConfigurationSettings.AppSettings["resourcepath"] + "/textures/" + ModelID.ToString() + ".bmp";
			modelsaveaspath = Server.MapPath(modelsaveaspath);

			Request.Files[0].SaveAs(modelsaveaspath);

			cmd.Close();
			
			Response.Redirect("./");




		}
	}
}
