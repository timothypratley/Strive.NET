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
	/// Summary description for TemplateItemEquippable.
	/// </summary>
	public class TemplateItemEquippable : GenericSpecialiser
	{
		protected System.Web.UI.WebControls.TextBox TemplateObjectName;
		protected System.Web.UI.WebControls.DropDownList ResourceID;
		protected System.Web.UI.WebControls.TextBox Height;
		protected System.Web.UI.WebControls.TextBox Value;
		protected System.Web.UI.WebControls.TextBox Weight;
		protected System.Web.UI.WebControls.DropDownList EnumWearLocationID;
		protected System.Web.UI.WebControls.TextBox ArmourClass;
		protected System.Web.UI.WebControls.Button Save;
		protected System.Web.UI.WebControls.Button Cancel;
#if FOO
		protected System.Web.UI.WebControls.Button Save;
		protected System.Web.UI.WebControls.Button Cancel;
#endif



		protected System.Web.UI.WebControls.DropDownList EnumItemDurabilityID;

	
		public TemplateItemEquippable() : base("ItemEquipable")
		{
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			// setup dropdowns
			if(!IsPostBack)
			{
				CommandFactory cmd = new CommandFactory();
				try
				{
					DataTable EnumItemDurabilitys = new DataTable();
					SqlDataAdapter EnumItemDurabilityFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumItemDurability ORDER BY BaseHitpointsEnergy "));
					EnumItemDurabilityFiller.Fill(EnumItemDurabilitys);
					EnumItemDurabilityID.DataSource = EnumItemDurabilitys;
					EnumItemDurabilityID.DataBind();
					EnumItemDurabilityID.Items.Insert(0, new ListItem("(select)", ""));

					DataTable EnumWearLocations = new DataTable();
					SqlDataAdapter EnumWearLocationFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumWearLocation ORDER BY EnumWearLocationName "));
					EnumWearLocationFiller.Fill(EnumWearLocations);
					EnumWearLocationID.DataSource = EnumWearLocations;
					EnumWearLocationID.DataBind();
					EnumWearLocationID.Items.Insert(0, new ListItem("(select)", ""));

					DataTable ResourceIDs = new DataTable();
					SqlDataAdapter ResourceIDFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM Resource WHERE EnumResourceTypeID = 3"));
					ResourceIDFiller.Fill(ResourceIDs);

					ResourceID.DataSource = ResourceIDs;
					ResourceID.DataBind();

					ResourceID.Items.Insert(0, new ListItem("(select)", ""));
					
				} 
				catch(Exception ex)
				{
					throw ex;
				}
				finally
				{
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
