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
using thisterminal.Web;
using www.strive3d.net.Game;
namespace www.strive3d.net.players.builders.textures
{
	/// <summary>
	/// Summary description for deletetexture.
	/// </summary>
	public class deletetexture : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			CommandFactory cmd = new CommandFactory();
			cmd.GetSqlCommand("DELETE FROM Model WHERE ModelID = " + QueryString.GetVariableInt32Value("ModelID")).ExecuteNonQuery();
			string texturepath = ".." + System.Configuration.ConfigurationSettings.AppSettings["resourcepath"] + "/textures/";
			texturepath = Server.MapPath(texturepath);
			foreach(string modelName in System.IO.Directory.GetFiles(texturepath, QueryString.GetVariableInt32Value("ModelID").ToString() + "*"))
			{
				System.IO.File.Delete(modelName);
			}
			cmd.Close();
			Response.Redirect("./");
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
