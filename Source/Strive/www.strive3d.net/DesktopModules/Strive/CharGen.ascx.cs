namespace www.strive3d.net
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using www.strive3d.net.Game;
	using System.Data.SqlClient;
	/// <summary>
	///		Summary description for CharGen.
	/// </summary>
	public abstract class CharGen : www.strive3d.net.PortalModuleControl
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
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
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

				StrengthModifier.Text = formatModifier(raceRow["StrengthModifier"].ToString());
				ConstitutionModifier.Text = formatModifier(raceRow["ConstitutionModifier"].ToString());
				DexterityModifier.Text = formatModifier(raceRow["DexterityModifier"].ToString());
				CognitionModifier.Text = formatModifier(raceRow["CognitionModifier"].ToString());
				WillpowerModifier.Text = formatModifier(raceRow["WillpowerModifier"].ToString());
				AirModifier.Text = formatModifier(raceRow["AirModifier"].ToString());
				FireModifier.Text  = formatModifier(raceRow["FireModifier"].ToString());
				WaterModifier.Text = formatModifier(raceRow["WaterModifier"].ToString());
				SpiritModifier.Text = formatModifier(raceRow["SpiritModifier"].ToString());
				EarthModifier.Text = formatModifier(raceRow["EarthModifier"].ToString());
				
			}
			else
			{
				showrace.Visible = false;
			}
		}

		string formatModifier(string input)
		{
			if(int.Parse(input) > 0)
			{
				return "+" + input;
			}
			else
			{
				return input;
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
		}

	}
}
