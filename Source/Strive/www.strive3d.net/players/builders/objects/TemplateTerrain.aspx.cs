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
using thisterminal.Web;

namespace www.strive3d.net.players.builders.objects
{
	/// <summary>
	/// Summary description for TemplateTerrain.
	/// </summary>
	public class TemplateTerrain : GenericSpecialiser
	{
		protected System.Web.UI.WebControls.TextBox TemplateObjectName;
		protected System.Web.UI.WebControls.DropDownList ResourceID;
		protected System.Web.UI.HtmlControls.HtmlInputHidden Height;
		protected System.Web.UI.WebControls.DropDownList EnumTerrainTypeID;
#if FOO
		protected System.Web.UI.WebControls.Button Save;
		protected System.Web.UI.WebControls.Button Cancel;
#endif
		protected System.Web.UI.HtmlControls.HtmlInputHidden AreaID;

		public TemplateTerrain() : base( "Terrain" ) {
		}
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			// setup dropdowns
			if(!IsPostBack)
			{
				CommandFactory cmd = new CommandFactory();
				try {
					DataTable Resources = new DataTable();
					SqlDataAdapter ResourceFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM Resource WHERE EnumResourceTypeID = 1 ORDER BY ResourceName ASC"));
					ResourceFiller.Fill(Resources);
					ResourceID.DataSource = Resources;
					ResourceID.DataBind();
					ResourceID.Items.Insert(0, new ListItem("(select)", ""));

					DataTable EnumTerrainTypes = new DataTable();
					SqlDataAdapter EnumTerrainTypeFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumTerrainType ORDER BY EnumTerrainTypeName ASC"));
					EnumTerrainTypeFiller.Fill(EnumTerrainTypes);
					EnumTerrainTypeID.DataSource = EnumTerrainTypes;
					EnumTerrainTypeID.DataBind();
					EnumTerrainTypeID.Items.Insert(0, new ListItem("(select)", ""));
				} catch(Exception ex) {
					throw ex;
				}
				finally {
					cmd.Close();
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
