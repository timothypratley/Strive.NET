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

namespace www.strive3d.net.players.builders.terrain
{
	/// <summary>
	/// Summary description for editterrainpiece.
	/// </summary>
	public class editterrainpiece : System.Web.UI.Page
{
		protected System.Web.UI.HtmlControls.HtmlImage textureshower;
		protected System.Web.UI.WebControls.TextBox Altitude;
		protected System.Web.UI.HtmlControls.HtmlInputHidden referer;
		protected int ObjectInstanceID;
		protected float TerrainX;
		protected float TerrainZ;
		protected System.Web.UI.WebControls.Repeater TemplateItemJunkList;
		protected System.Web.UI.WebControls.Button cancel;
		protected System.Web.UI.WebControls.Repeater TemplateItemWieldableList;
		protected System.Web.UI.WebControls.Repeater TemplateItemReadableList;
		protected System.Web.UI.WebControls.Repeater TemplateItemEquipableList;
		protected System.Web.UI.WebControls.Repeater TemplateItemQuaffableList;
		protected System.Web.UI.WebControls.Repeater TemplateMobileList;
		protected System.Web.UI.WebControls.DropDownList TemplateObject;
		
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			if(QueryString.ContainsVariable("ObjectInstanceID"))
			{
				ObjectInstanceID = QueryString.GetVariableInt32Value("ObjectInstanceID");
			}

			if(!this.IsPostBack) {
				Altitude.Text = "-10";
				referer.Value = Request.ServerVariables["HTTP_REFERER"].ToString();
				CommandFactory cmd = new CommandFactory();
				try {

					SqlDataAdapter texturefiller = new SqlDataAdapter(cmd.GetSqlCommand("select * from TemplateTerrain tt, TemplateObject t, Resource r where tt.TemplateObjectID = t.TemplateObjectID and t.ResourceID = r.ResourceID"));
					DataTable textures = new DataTable();
					// add to viewstate for handy stuff
					texturefiller.Fill(textures);
					ViewState.Add("textures", textures);

					TemplateObject.DataSource = textures;
					TemplateObject.DataBind();				
		
					if(QueryString.ContainsVariable("ObjectInstanceID")) {
						// set values for edits:
						SqlDataReader oDr = cmd.GetTerrain(ObjectInstanceID).ExecuteReader();
						if(!oDr.Read()) {
							oDr.Close();
							throw new Exception("Could not find terrain piece '" + ObjectInstanceID + "'.");
						}
						else {
							TemplateObject.SelectedIndex = TemplateObject.Items.IndexOf(TemplateObject.Items.FindByValue(oDr["TemplateObjectID"].ToString()));
							Altitude.Text = oDr["Y"].ToString();

							TerrainX = float.Parse(oDr["X"].ToString());
							TerrainZ = float.Parse(oDr["Z"].ToString());
							oDr.Close();
							SqlDataAdapter TemplateItemJunkFiller = new SqlDataAdapter(
								cmd.GetSqlCommand(
								"SELECT TemplateItemJunk.*, " +
								"TemplateObject.TemplateObjectName, " +
								"TemplateItem.Value, " +
								"TemplateItem.Weight, " +
								"ObjectInstance.ObjectInstanceID , " +
								"TemplateItem.EnumItemDurabilityID " +
								"FROM TemplateItemJunk " +
								"INNER JOIN TemplateItem " +
								"ON TemplateItemJunk.TemplateObjectID = TemplateItem.TemplateObjectID " +
								"INNER JOIN TemplateObject " +
								"ON TemplateItem.TemplateObjectID = TemplateObject.TemplateObjectID " +
								"INNER JOIN ObjectInstance  " +
								"ON ObjectInstance.TemplateObjectID = TemplateObject.TemplateObjectID " +
								"AND ObjectInstance.X >= " + TerrainX +
								" AND ObjectInstance.Z >= " + TerrainZ +
								" AND ObjectInstance.X < " + (TerrainX + Strive.Common.Constants.terrainPieceSize) +
								" AND ObjectInstance.Z < " + (TerrainZ + Strive.Common.Constants.terrainPieceSize) +
								" ORDER BY TemplateObjectName "));

							DataTable TemplateItemJunkInSquare = new DataTable();
							TemplateItemJunkFiller.Fill(TemplateItemJunkInSquare);
							TemplateItemJunkList.DataSource = TemplateItemJunkInSquare;
							TemplateItemJunkList.DataBind();

							SqlDataAdapter TemplateItemWieldableFiller = new SqlDataAdapter(
								cmd.GetSqlCommand(
								"SELECT TemplateItemWieldable.*, " +
								"TemplateObject.TemplateObjectName, " +
								"TemplateItem.Value, " +
								"TemplateItem.Weight, " +
								"ObjectInstance.ObjectInstanceID , " +
								"TemplateItem.EnumItemDurabilityID " +
								"FROM TemplateItemWieldable " +
								"INNER JOIN TemplateItem " +
								"ON TemplateItemWieldable.TemplateObjectID = TemplateItem.TemplateObjectID " +
								"INNER JOIN TemplateObject " +
								"ON TemplateItem.TemplateObjectID = TemplateObject.TemplateObjectID " +
								"INNER JOIN ObjectInstance  " +
								"ON ObjectInstance.TemplateObjectID = TemplateObject.TemplateObjectID " +
								"AND ObjectInstance.X >= " + TerrainX +
								" AND ObjectInstance.Z >= " + TerrainZ +
								" AND ObjectInstance.X < " + (TerrainX + Strive.Common.Constants.terrainPieceSize) +
								" AND ObjectInstance.Z < " + (TerrainZ + Strive.Common.Constants.terrainPieceSize) +
								" ORDER BY TemplateObjectName "));

							DataTable TemplateItemWieldableInSquare = new DataTable();
							TemplateItemWieldableFiller.Fill(TemplateItemWieldableInSquare);
							TemplateItemWieldableList.DataSource = TemplateItemWieldableInSquare;
							TemplateItemWieldableList.DataBind();

							SqlDataAdapter TemplateItemQuaffableFiller = new SqlDataAdapter(
								cmd.GetSqlCommand(
								"SELECT TemplateItemQuaffable.*, " +
								"TemplateObject.TemplateObjectName, " +
								"TemplateItem.Value, " +
								"TemplateItem.Weight, " +
								"ObjectInstance.ObjectInstanceID , " +
								"TemplateItem.EnumItemDurabilityID " +
								"FROM TemplateItemQuaffable " +
								"INNER JOIN TemplateItem " +
								"ON TemplateItemQuaffable.TemplateObjectID = TemplateItem.TemplateObjectID " +
								"INNER JOIN TemplateObject " +
								"ON TemplateItem.TemplateObjectID = TemplateObject.TemplateObjectID " +
								"INNER JOIN ObjectInstance  " +
								"ON ObjectInstance.TemplateObjectID = TemplateObject.TemplateObjectID " +
								"AND ObjectInstance.X >= " + TerrainX +
								" AND ObjectInstance.Z >= " + TerrainZ +
								" AND ObjectInstance.X < " + (TerrainX + Strive.Common.Constants.terrainPieceSize) +
								" AND ObjectInstance.Z < " + (TerrainZ + Strive.Common.Constants.terrainPieceSize) +
								" ORDER BY TemplateObjectName "));

							DataTable TemplateItemQuaffableInSquare = new DataTable();
							TemplateItemQuaffableFiller.Fill(TemplateItemQuaffableInSquare);
							TemplateItemQuaffableList.DataSource = TemplateItemQuaffableInSquare;
							TemplateItemQuaffableList.DataBind();

							SqlDataAdapter TemplateItemReadableFiller = new SqlDataAdapter(
								cmd.GetSqlCommand(
								"SELECT TemplateItemReadable.*, " +
								"TemplateObject.TemplateObjectName, " +
								"TemplateItem.Value, " +
								"TemplateItem.Weight, " +
								"ObjectInstance.ObjectInstanceID , " +
								"TemplateItem.EnumItemDurabilityID " +
								"FROM TemplateItemReadable " +
								"INNER JOIN TemplateItem " +
								"ON TemplateItemReadable.TemplateObjectID = TemplateItem.TemplateObjectID " +
								"INNER JOIN TemplateObject " +
								"ON TemplateItem.TemplateObjectID = TemplateObject.TemplateObjectID " +
								"INNER JOIN ObjectInstance  " +
								"ON ObjectInstance.TemplateObjectID = TemplateObject.TemplateObjectID " +
								"AND ObjectInstance.X >= " + TerrainX +
								" AND ObjectInstance.Z >= " + TerrainZ +
								" AND ObjectInstance.X < " + (TerrainX + Strive.Common.Constants.terrainPieceSize) +
								" AND ObjectInstance.Z < " + (TerrainZ + Strive.Common.Constants.terrainPieceSize) +
								" ORDER BY TemplateObjectName "));

							DataTable TemplateItemReadableInSquare = new DataTable();
							TemplateItemReadableFiller.Fill(TemplateItemReadableInSquare);
							TemplateItemReadableList.DataSource = TemplateItemReadableInSquare;
							TemplateItemReadableList.DataBind();

							SqlDataAdapter TemplateMobileFiller = new SqlDataAdapter(
								cmd.GetSqlCommand(
								"SELECT TemplateMobile.*, " +
								"TemplateObject.TemplateObjectName, " +
								"ObjectInstance.ObjectInstanceID  " +
								"FROM TemplateMobile " +
								"INNER JOIN TemplateObject " +
								"ON TemplateMobile.TemplateObjectID = TemplateObject.TemplateObjectID " +
								"INNER JOIN ObjectInstance  " +
								"ON ObjectInstance.TemplateObjectID = TemplateObject.TemplateObjectID " +
								"AND ObjectInstance.X >= " + TerrainX +
								" AND ObjectInstance.Z >= " + TerrainZ +
								" AND ObjectInstance.X < " + (TerrainX + Strive.Common.Constants.terrainPieceSize) +
								" AND ObjectInstance.Z < " + (TerrainZ + Strive.Common.Constants.terrainPieceSize) +
								" ORDER BY TemplateObjectName "));

							DataTable TemplateMobileInSquare = new DataTable();
							TemplateMobileFiller.Fill(TemplateMobileInSquare);
							TemplateMobileList.DataSource = TemplateMobileInSquare;
							TemplateMobileList.DataBind();

							SqlDataAdapter TemplateItemEquipableFiller = new SqlDataAdapter(
								cmd.GetSqlCommand(
								"SELECT TemplateItemEquipable.*, " +
								"TemplateObject.TemplateObjectName, " +
								"TemplateItem.Value, " +
								"TemplateItem.Weight, " +
								"ObjectInstance.ObjectInstanceID , " +
								"TemplateItem.EnumItemDurabilityID " +
								"FROM TemplateItemEquipable " +
								"INNER JOIN TemplateItem " +
								"ON TemplateItemEquipable.TemplateObjectID = TemplateItem.TemplateObjectID " +
								"INNER JOIN TemplateObject " +
								"ON TemplateItem.TemplateObjectID = TemplateObject.TemplateObjectID " +
								"INNER JOIN ObjectInstance  " +
								"ON ObjectInstance.TemplateObjectID = TemplateObject.TemplateObjectID " +
								"AND ObjectInstance.X >= " + TerrainX +
								" AND ObjectInstance.Z >= " + TerrainZ +
								" AND ObjectInstance.X < " + (TerrainX + Strive.Common.Constants.terrainPieceSize) +
								" AND ObjectInstance.Z < " + (TerrainZ + Strive.Common.Constants.terrainPieceSize) +
								" ORDER BY TemplateObjectName "));

							DataTable TemplateItemEquipableInSquare = new DataTable();
							TemplateItemEquipableFiller.Fill(TemplateItemEquipableInSquare);
							TemplateItemEquipableList.DataSource = TemplateItemEquipableInSquare;
							TemplateItemEquipableList.DataBind();





	
						}
					}

					// find correct row in datasource
					DataRow selectedResourceRow = textures.Select("TemplateObjectID = " + TemplateObject.SelectedItem.Value)[0];
					textureshower.Src = Utils.ApplicationPath + "/DesktopModules/Strive/Thumbnailer.aspx?i=" + Utils.ApplicationPath + "/players/builders/" + System.Configuration.ConfigurationSettings.AppSettings["resourcepath"] + "/texture/" +selectedResourceRow["ResourceID"] + selectedResourceRow["ResourceFileExtension"] +"&amp;h=75&amp;w=75";
				}
				catch(Exception c) {
					throw new Exception("EditTerrainPiece.Page_Load", c);
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
			this.TemplateObject.SelectedIndexChanged += new System.EventHandler(this.TemplateObject_SelectedIndexChanged);
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


		private void Save() {
			CommandFactory cmd = new CommandFactory();
			try{
			if(QueryString.ContainsVariable("ObjectInstanceID"))
			{
				cmd.UpdateTerrain(QueryString.GetVariableInt32Value("ObjectInstanceID"),
					int.Parse(TemplateObject.SelectedItem.Value),
					QueryString.GetVariableInt32Value("X"),
					float.Parse(Altitude.Text),
					QueryString.GetVariableInt32Value("Z"),
					0, 
					0, 
					0).ExecuteNonQuery();

			}
			else
			{
//				cmd.CreateTerrain(1,
//					EnumTerrainID.SelectedItem.Text + " - " + ResourceID.SelectedItem.Text,
//					Int32.Parse(ResourceID.SelectedItem.Value),
//					PlayerAuthenticator.CurrentLoggedInPlayerID,
//					Int32.Parse(EnumTerrainID.SelectedItem.Value),
//					QueryString.GetVariableInt32Value("X"),
//					Int32.Parse(Altitude.Text),
//					QueryString.GetVariableInt32Value("Z"),
//					0, 
//					0, 
//					0).ExecuteNonQuery();
			}
			Page.RegisterClientScriptBlock("Refresh", "<script type=\"text/javascript\">window.parent.frames['" + Request.QueryString["FrameID"].ToString() + "'].location.reload(true);</script>");
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

		private void TemplateObject_SelectedIndexChanged(object sender, System.EventArgs e) {
			DataTable textures = (DataTable)ViewState["textures"];
			DataRow selectedResourceRow = textures.Select("TemplateObjectID = " + TemplateObject.SelectedItem.Value)[0];
			textureshower.Src = Utils.ApplicationPath + "/DesktopModules/Strive/Thumbnailer.aspx?i=" + Utils.ApplicationPath + "/players/builders/" + System.Configuration.ConfigurationSettings.AppSettings["resourcepath"] + "/texture/" +selectedResourceRow["ResourceID"] + selectedResourceRow["ResourceFileExtension"] +"&amp;h=75&amp;w=75";
			Save();
		}

		private void cancel_Click(object sender, System.EventArgs e) {
			Page.RegisterClientScriptBlock("Close", "<script type=\"text/javascript\">location.href='about:blank';</script>");
		}
	}
}
