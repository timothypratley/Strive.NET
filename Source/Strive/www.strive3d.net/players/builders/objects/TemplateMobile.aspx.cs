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
	/// Summary description for TemplateMobile.
	/// </summary>
	public class TemplateMobile : GenericSpecialiser
	{
		protected System.Web.UI.WebControls.DropDownList ResourceID;
		protected System.Web.UI.WebControls.DropDownList EnumRaceID;
		protected System.Web.UI.WebControls.DropDownList EnumMobileSizeID;
		protected System.Web.UI.WebControls.TextBox TemplateObjectName;
		protected System.Web.UI.WebControls.TextBox Height;
		protected System.Web.UI.WebControls.TextBox Strength;
		protected System.Web.UI.WebControls.TextBox Constitution;
		protected System.Web.UI.WebControls.TextBox Cognition;
		protected System.Web.UI.WebControls.TextBox Willpower;
		protected System.Web.UI.WebControls.TextBox Dexterity;
		protected System.Web.UI.WebControls.DropDownList EnumSexID;
		protected System.Web.UI.WebControls.TextBox Level;

		protected System.Web.UI.WebControls.DropDownList EnumMobileStateID;
		public TemplateMobile() : base("Mobile")
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
					DataTable EnumRaces = new DataTable();
					SqlDataAdapter EnumRaceFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumRace ORDER BY SelectableByPlayers DESC, EnumRaceName ASC"));
					EnumRaceFiller.Fill(EnumRaces);
					EnumRaceID.DataSource = EnumRaces;
					EnumRaceID.DataBind();
					EnumRaceID.Items.Insert(0, new ListItem("(select)", ""));

					DataTable EnumMobileStates = new DataTable();
					SqlDataAdapter EnumMobileStateFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumMobileState"));
					EnumMobileStateFiller.Fill(EnumMobileStates);
					EnumMobileStateID.DataSource = EnumMobileStates;
					EnumMobileStateID.DataBind();
					EnumMobileStateID.Items.Insert(0, new ListItem("(select)", ""));

					DataTable EnumMobileSizes = new DataTable();
					SqlDataAdapter EnumMobileSizeFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumMobileSize"));
					EnumMobileSizeFiller.Fill(EnumMobileSizes);
					EnumMobileSizeID.DataSource = EnumMobileSizes;
					EnumMobileSizeID.DataBind();
					EnumMobileSizeID.Items.Insert(0, new ListItem("(select)", ""));

					DataTable EnumSexs = new DataTable();
					SqlDataAdapter EnumSexFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumSex"));
					EnumSexFiller.Fill(EnumSexs);
					EnumSexID.DataSource = EnumSexs;
					EnumSexID.DataBind();
					EnumSexID.Items.Insert(0, new ListItem("(select)", ""));

					DataTable ResourceIDs = new DataTable();
					SqlDataAdapter ResourceIDFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM Resource WHERE EnumResourceTypeID = 2"));
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
			this.EnumRaceID.SelectedIndexChanged += new System.EventHandler(this.EnumRaceID_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void EnumRaceID_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// setup correct stats
			if(EnumRaceID.SelectedValue != "")
			{
				// lookup the race and set the stats.
				using(CommandFactory cmd = new CommandFactory())
				{
					SqlDataReader EnumRaceRow = (SqlDataReader)cmd.GetSqlCommand("SELECT * FROM EnumRace WHERE EnumRaceID = " + EnumRaceID.SelectedValue).ExecuteReader();
					if(EnumRaceRow.Read())
					{
						Strength.Text = (15 + (int)EnumRaceRow["StrengthModifier"]).ToString();
						Constitution.Text = (15 + (int)EnumRaceRow["ConstitutionModifier"]).ToString();
						Dexterity.Text =  (15 + (int)EnumRaceRow["DexterityModifier"]).ToString();
						Cognition.Text = (15 + (int)EnumRaceRow["CognitionModifier"]).ToString();
						Willpower.Text = (15 + (int)EnumRaceRow["WillpowerModifier"]).ToString();
						EnumMobileSizeID.SelectedValue = EnumRaceRow["EnumMobileSizeID"].ToString();
					}
					EnumRaceRow.Close();
				}
			}

		}
	}
}
