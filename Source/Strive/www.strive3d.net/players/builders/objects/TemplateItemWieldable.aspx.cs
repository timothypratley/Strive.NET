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
	public class TemplateItemWieldable : GenericSpecialiser
	{
		protected System.Web.UI.WebControls.TextBox TemplateObjectName;
		protected System.Web.UI.WebControls.DropDownList ResourceID;
		protected System.Web.UI.WebControls.TextBox Height;
		protected System.Web.UI.WebControls.TextBox Value;
		protected System.Web.UI.WebControls.TextBox Weight;
		protected System.Web.UI.WebControls.TextBox ArmourClass;
		protected System.Web.UI.WebControls.DropDownList EnumDamageTypeID;
		protected System.Web.UI.WebControls.DropDownList EnumWeaponSizeID;
		protected System.Web.UI.WebControls.TextBox Damage;
		protected System.Web.UI.WebControls.TextBox Hitroll;




		protected System.Web.UI.WebControls.DropDownList EnumItemDurabilityID;

	
		public TemplateItemWieldable() : base("ItemWieldable")
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
					DataTable EnumDamageTypes = new DataTable();
					SqlDataAdapter EnumDamageTypeFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumDamageType ORDER BY EnumDamageTypeName "));
					EnumDamageTypeFiller.Fill(EnumDamageTypes);
					EnumDamageTypeID.DataSource = EnumDamageTypes;
					EnumDamageTypeID.DataBind();
					EnumDamageTypeID.Items.Insert(0, new ListItem("(select)", ""));

					DataTable EnumItemDurabilitys = new DataTable();
					SqlDataAdapter EnumItemDurabilityFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumItemDurability ORDER BY BaseHitpointsEnergy "));
					EnumItemDurabilityFiller.Fill(EnumItemDurabilitys);
					EnumItemDurabilityID.DataSource = EnumItemDurabilitys;
					EnumItemDurabilityID.DataBind();
					EnumItemDurabilityID.Items.Insert(0, new ListItem("(select)", ""));

					DataTable EnumWeaponSizes = new DataTable();
					SqlDataAdapter EnumWeaponSizeFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumWeaponSize ORDER BY EnumWeaponSizeName "));
					EnumWeaponSizeFiller.Fill(EnumWeaponSizes);
					EnumWeaponSizeID.DataSource = EnumWeaponSizes;
					EnumWeaponSizeID.DataBind();
					EnumWeaponSizeID.Items.Insert(0, new ListItem("(select)", ""));

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
