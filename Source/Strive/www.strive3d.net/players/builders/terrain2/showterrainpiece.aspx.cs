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
using www.strive3d.net.Game;
using thisterminal.Web;
using System.Data.SqlClient;

namespace www.strive3d.net.players.builders.terrain2
{
	/// <summary>
	/// Summary description for editterrainpiece.
	/// </summary>
	public class showterrainpiece : System.Web.UI.Page
{
		protected int ObjectInstanceID;
		protected string TextureSrc;
		protected decimal Altitude;
		protected decimal Rotation;
		protected bool Loaded;
		protected int X;
		protected System.Web.UI.WebControls.Button Higher;
				protected System.Web.UI.WebControls.Button Rotate;
		protected System.Web.UI.WebControls.Button Lower;
		protected int Z;
	
		private void Page_Load(object sender, System.EventArgs e)
		{

			if(QueryString.ContainsVariable("ObjectInstanceID"))
			{
				ObjectInstanceID = QueryString.GetVariableInt32Value("ObjectInstanceID");
			}

			X = QueryString.GetVariableInt32Value("X");
			Z = QueryString.GetVariableInt32Value("Z");
			CommandFactory cmd = new CommandFactory();
			try {
			// set values for edits:
			SqlDataReader oDr = cmd.GetTerrain(ObjectInstanceID).ExecuteReader();
		
			if(!oDr.Read())
			{
				oDr.Close();

				Loaded = false;
			}
			else
			{
				Altitude = decimal.Parse(oDr["Y"].ToString());
				Rotation = decimal.Parse(oDr["RotationY"].ToString());
				TextureSrc = Utils.ApplicationPath + "/DesktopModules/Strive/Thumbnailer.aspx?i=" + Utils.ApplicationPath + "/players/builders/" + System.Configuration.ConfigurationSettings.AppSettings["resourcepath"] + "/texture/" +oDr["ResourceID"] + oDr["ResourceFileExtension"] +"&amp;h=75&amp;w=75&amp;r=" + Rotation; ;
				oDr.Close();
				Loaded = true;
			}
			}
			catch(Exception c)
			{
				throw c;
			}
			finally
			{
				cmd.Close();
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
			this.Higher.Click += new System.EventHandler(this.Higher_Click);
			this.Lower.Click += new System.EventHandler(this.Lower_Click);
			this.Load += new System.EventHandler(this.Page_Load);
			this.Rotate.Click += new System.EventHandler(this.Rotate_Click);

		}
		#endregion

		private void Higher_Click(object sender, System.EventArgs e)
		{
			CommandFactory cmd = new CommandFactory();
			try 
			{
				cmd.RaiseTerrain(QueryString.GetVariableInt32Value("ObjectInstanceID")).ExecuteNonQuery();

			}
			catch(Exception c)
			{
				throw c;
			}
			finally
			{
				cmd.Close();
			}
			Response.Redirect(Request.Url.ToString());
		}

		private void Lower_Click(object sender, System.EventArgs e)
		{
			CommandFactory cmd = new CommandFactory();
			try {
			cmd.LowerTerrain(QueryString.GetVariableInt32Value("ObjectInstanceID")).ExecuteNonQuery();

			}
			catch(Exception c)
			{
				throw c;
			}
			finally
			{
				cmd.Close();
			}
			Response.Redirect(Request.Url.ToString());
		}

		private void Rotate_Click(object sender, System.EventArgs e)
		{
			CommandFactory cmd = new CommandFactory();
			try {
			cmd.RotateTerrain(QueryString.GetVariableInt32Value("ObjectInstanceID"), 90).ExecuteNonQuery();
			}
			catch(Exception c)
			{
				throw c;
			}
			finally
			{
				cmd.Close();
			}
			Response.Redirect(Request.Url.ToString());		
		}
	}
}
