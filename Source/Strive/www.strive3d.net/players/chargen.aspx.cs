using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using www.strive3d.net.Game;

namespace www.strive3d.net.players
{
	/// <summary>
	/// Summary description for chargen.
	/// </summary>
	public class chargen : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox CharacterName;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.HtmlControls.HtmlInputHidden referer;
		protected System.Web.UI.WebControls.DropDownList EnumRaceID;
		protected System.Web.UI.WebControls.Panel showrace;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label StrengthModifier;
		protected System.Web.UI.WebControls.Label ConstitutionModifier;
		protected System.Web.UI.WebControls.Label DexterityModifier;
		protected System.Web.UI.WebControls.Label CognitionModifier;
		protected System.Web.UI.WebControls.Label WillpowerModifier;
		protected System.Web.UI.WebControls.Label AirModifier;
		protected System.Web.UI.WebControls.Label EarthModifier;
		protected System.Web.UI.WebControls.Label WaterModifier;
		protected System.Web.UI.WebControls.Label FireModifier;
		protected System.Web.UI.WebControls.Label SpiritModifier;
		protected System.Web.UI.WebControls.Label RaceDescription;
		protected DataTable races = new DataTable();	

		private void Page_Load(object sender, System.EventArgs e)
		{
			CommandFactory cmd = new CommandFactory();

			SqlDataAdapter raceFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumRace WHERE SelectableByPlayers = 1"));
			raceFiller.Fill(races);


			if(!this.IsPostBack)
			{
				referer.Value = Request.ServerVariables["HTTP_REFERER"];


				EnumRaceID.DataSource = races;
				EnumRaceID.DataBind();

				EnumRaceID.Items.Add(new ListItem("(select)", ""));

				EnumRaceID.SelectedIndex = EnumRaceID.Items.IndexOf(EnumRaceID.Items.FindByValue(""));

			}

			cmd.Close();
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
			this.Button1.Click += new System.EventHandler(this.Button1_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void EnumRaceID_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(EnumRaceID.SelectedItem.Value != "")
			{
				DataRow raceRow = races.Select("EnumRaceID = " + EnumRaceID.SelectedItem.Value)[0];

				showrace.Visible = true;

				RaceDescription.Text = raceRow["Description"].ToString();

				StrengthModifier.Text = raceRow["StrengthModifier"].ToString();
				ConstitutionModifier.Text = raceRow["ConstitutionModifier"].ToString();
				DexterityModifier.Text = raceRow["DexterityModifier"].ToString();
				CognitionModifier.Text = raceRow["CognitionModifier"].ToString();
				WillpowerModifier.Text = raceRow["WillpowerModifier"].ToString();
				AirModifier.Text = raceRow["AirModifier"].ToString();
				FireModifier.Text  = raceRow["FireModifier"].ToString();
				WaterModifier.Text = raceRow["WaterModifier"].ToString();
				SpiritModifier.Text = raceRow["SpiritModifier"].ToString();
				EarthModifier.Text = raceRow["EarthModifier"].ToString();
				
			}
			else
			{
				showrace.Visible = false;
			}
		}

		private void Button1_Click(object sender, System.EventArgs e)
		{
			CommandFactory cmd = new CommandFactory();

			cmd.CreateCharacter(CharacterName.Text,
				38,
				PlayerAuthenticator.CurrentLoggedInPlayerID,
				int.Parse(EnumRaceID.SelectedItem.Value)).ExecuteNonQuery();

			cmd.Close();

			Response.Redirect(referer.Value);
		}
	}
}
