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
	public class editterrainpiece : System.Web.UI.Page
{	
		protected System.Web.UI.WebControls.DropDownList ResourceID;
		protected System.Web.UI.HtmlControls.HtmlImage textureshower;
		protected System.Web.UI.WebControls.DropDownList EnumTerrainID;
		protected System.Web.UI.WebControls.Button Save;
		protected System.Web.UI.WebControls.Button Cancel;
		protected System.Web.UI.WebControls.TextBox Altitude;
		protected System.Web.UI.HtmlControls.HtmlInputHidden referer;
		protected int ObjectInstanceID;
		protected DataTable textures;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			if(QueryString.ContainsVariable("ObjectInstanceID"))
			{
				ObjectInstanceID = QueryString.GetVariableInt32Value("ObjectInstanceID");
			}

			if(!this.IsPostBack)
			{
				Altitude.Text = "-10";
				referer.Value = Request.ServerVariables["HTTP_REFERER"].ToString();
				CommandFactory cmd = new CommandFactory();

				SqlDataAdapter texturefiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM Resource WHERE EnumResourceTypeID = 1 ORDER BY ResourceName"));
				SqlDataAdapter terraintypefiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumTerrainType ORDER BY EnumTerrainTypeName"));

				textures = new DataTable();
				// add to viewstate for handy stuff
				ViewState.Add("textures", textures);
				texturefiller.Fill(textures);

				DataTable terrains = new DataTable();
				terraintypefiller.Fill(terrains);

				ResourceID.DataSource = textures;
				EnumTerrainID.DataSource = terrains;
				ResourceID.DataBind();				
				EnumTerrainID.DataBind();
				if(QueryString.ContainsVariable("ObjectInstanceID"))
				{
					// set values for edits:
					SqlDataReader oDr = cmd.GetTerrain(ObjectInstanceID).ExecuteReader();
					if(!oDr.Read())
					{
						oDr.Close();
						throw new Exception("Could not find terrain piece '" + ObjectInstanceID + "'.");
					}
					else
					{
						ResourceID.SelectedIndex = ResourceID.Items.IndexOf(ResourceID.Items.FindByValue(oDr["ResourceID"].ToString()));
                        EnumTerrainID.SelectedIndex = EnumTerrainID.Items.IndexOf(EnumTerrainID.Items.FindByValue(oDr["EnumTerrainTypeID"].ToString()));
						Altitude.Text = oDr["Y"].ToString();
						oDr.Close();
					}
				}

				// find correct row in datasource
				DataRow selectedResourceRow = textures.Select("ResourceID = " + ResourceID.SelectedItem.Value)[0];
				textureshower.Src = Utils.ApplicationPath + "/DesktopModules/Strive/Thumbnailer.aspx?i=" + Utils.ApplicationPath + "/players/builders/" + System.Configuration.ConfigurationSettings.AppSettings["resourcepath"] + "/texture/" +selectedResourceRow["ResourceID"] + selectedResourceRow["ResourceFileExtension"] +"&amp;h=75&amp;w=75";
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
			this.ResourceID.SelectedIndexChanged += new System.EventHandler(this.ResourceID_SelectedIndexChanged);
			this.Save.Click += new System.EventHandler(this.Save_Click);
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ResourceID_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// find correct row in datasource
			DataRow selectedResourceRow = ((DataTable) ViewState["textures"]).Select("ResourceID = " + ResourceID.SelectedItem.Value)[0];
			textureshower.Src = Utils.ApplicationPath + "/DesktopModules/Strive/Thumbnailer.aspx?i=" + Utils.ApplicationPath + "/players/builders/" + System.Configuration.ConfigurationSettings.AppSettings["resourcepath"] + "/texture/" +selectedResourceRow["ResourceID"] + selectedResourceRow["ResourceFileExtension"] +"&amp;h=75&amp;w=75";
		}

		private void Save_Click(object sender, System.EventArgs e)
		{
			CommandFactory cmd = new CommandFactory();
			if(QueryString.ContainsVariable("ObjectInstanceID"))
			{
				cmd.UpdateTerrain(QueryString.GetVariableInt32Value("ObjectInstanceID"),
					1,
					EnumTerrainID.SelectedItem.Text + " - " + ResourceID.SelectedItem.Text,
					Int32.Parse(ResourceID.SelectedItem.Value),
					PlayerAuthenticator.CurrentLoggedInPlayerID,
					Int32.Parse(EnumTerrainID.SelectedItem.Value),
					QueryString.GetVariableInt32Value("X"),
					float.Parse(Altitude.Text),
					QueryString.GetVariableInt32Value("Z"),
					0, 
					0, 
					0).ExecuteNonQuery();
			}
			else
			{
				cmd.CreateTerrain(1,
					EnumTerrainID.SelectedItem.Text + " - " + ResourceID.SelectedItem.Text,
					Int32.Parse(ResourceID.SelectedItem.Value),
					PlayerAuthenticator.CurrentLoggedInPlayerID,
					Int32.Parse(EnumTerrainID.SelectedItem.Value),
					QueryString.GetVariableInt32Value("X"),
					Int32.Parse(Altitude.Text),
					QueryString.GetVariableInt32Value("Z"),
					0, 
					0, 
					0).ExecuteNonQuery();
			}
			Page.RegisterClientScriptBlock("Refresh", "<script type=\"text/javascript\">window.parent.frames['" + Request.QueryString["FrameID"].ToString() + "'].location.reload(true);</script>");
			Page.RegisterClientScriptBlock("Close", "<script type=\"text/javascript\">location.href='about:blank';</script>");
		}

		private void Cancel_Click(object sender, System.EventArgs e)
		{
			Page.RegisterClientScriptBlock("Close", "<script type=\"text/javascript\">location.href='about:blank';</script>");
		}
	}
}
