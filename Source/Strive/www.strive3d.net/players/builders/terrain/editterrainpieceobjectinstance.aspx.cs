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

namespace www.strive3d.net.players.builders.terrain
{
	/// <summary>
	/// Summary description for editterrainpieceobjectinstance.
	/// </summary>
	public class editterrainpieceobjectinstance : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DropDownList TemplateList;
		protected System.Web.UI.WebControls.TextBox X;
		protected System.Web.UI.WebControls.TextBox Z;
		protected System.Web.UI.WebControls.Button Save;
		int ObjectInstanceStartX;
		int ObjectInstanceStartZ;
		protected System.Web.UI.WebControls.Button Cancel;
		protected System.Web.UI.WebControls.Button Delete;
		int ObjectInstanceY;
		private void Page_Load(object sender, System.EventArgs e)
		{
			ObjectInstanceStartX = QueryString.GetVariableInt32Value("StartX");
			ObjectInstanceStartZ = QueryString.GetVariableInt32Value("StartZ");
			ObjectInstanceY = QueryString.GetVariableInt32Value("Y");
			if(!QueryString.ContainsVariable("ObjectInstanceID")) {
				Delete.Visible=false;
			}

			if(!IsPostBack)
			{
				CommandFactory cmd = new CommandFactory();
				try
				{
					string TemplateName = QueryString.GetVariableStringValue("TemplateName");

					// 2.0 Set up drop down:
					SqlDataAdapter TemplateItemFiller;
					if(TemplateName.StartsWith("Mobile")) {
						TemplateItemFiller = new SqlDataAdapter(
							cmd.GetSqlCommand(
							"SELECT TemplateMobile.*, " +
							"TemplateObject.TemplateObjectName " +
							"FROM TemplateMobile " +
							"INNER JOIN TemplateObject " +
							"ON TemplateMobile.TemplateObjectID = TemplateObject.TemplateObjectID " +
							"ORDER BY TemplateObjectName "));
					}
					else {
						TemplateItemFiller = new SqlDataAdapter(
							cmd.GetSqlCommand(
							"SELECT TemplateItem.*, " +
							"TemplateObject.TemplateObjectName, " +
							"TemplateItem.Value, " +
							"TemplateItem.Weight, " +
							"TemplateItem.EnumItemDurabilityID " +
							"FROM TemplateItem " +
							"INNER JOIN Template" + TemplateName + " " +
							"ON Template" + TemplateName + " " + ".TemplateObjectID = TemplateItem.TemplateObjectID " +
							"INNER JOIN TemplateObject " +
							"ON TemplateItem.TemplateObjectID = TemplateObject.TemplateObjectID " +
							"ORDER BY TemplateObjectName "));
					}

					DataTable TemplateItems = new DataTable();
				
					TemplateItemFiller.Fill(TemplateItems);
					TemplateList.DataSource = TemplateItems;
					TemplateList.DataBind();
					TemplateList.Items.Insert(0, new ListItem("(select)", ""));

					// 3.0 Load any values:
					if(QueryString.ContainsVariable("ObjectInstanceID"))
					{
						SqlDataReader oDr = cmd.GetSqlCommand("SELECT * FROM ObjectInstance WHERE ObjectInstanceID = " + QueryString.GetVariableInt32Value("ObjectInstanceID")).ExecuteReader();
						if(oDr.Read())
						{
							// Set TemplateItemList
							TemplateList.SelectedIndex = TemplateList.Items.IndexOf(TemplateList.Items.FindByValue(oDr["TemplateObjectID"].ToString()));
							// Set position:
							float ObjectInstanceX = float.Parse(oDr["X"].ToString());
							float ObjectInstanceZ = float.Parse(oDr["Z"].ToString());
							X.Text = (ObjectInstanceX- ObjectInstanceStartX).ToString();
							Z.Text = ( ObjectInstanceZ - ObjectInstanceStartZ ).ToString();
						}
						else
						{
							oDr.Close();
							throw new Exception("Could not load ObjectInstance [" + QueryString.GetVariableInt32Value("ObjectInstanceID") + "].");
							
						}
						oDr.Close();
					}

				}
				catch(Exception c)
				{
					throw new Exception("editterrainpieceobjectinstance.Page_Load", c);
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
			this.Save.Click += new System.EventHandler(this.Save_Click);
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			this.Delete.Click += new System.EventHandler(this.Delete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Save_Click(object sender, System.EventArgs e)
		{
			CommandFactory cmd = new CommandFactory();
			try
			{
				float NewX = 0;
				float NewY = QueryString.GetVariableInt32Value("Y");
				float NewZ = 0;

				NewX = float.Parse(X.Text) + ObjectInstanceStartX ;
				NewZ = float.Parse(Z.Text) + ObjectInstanceStartZ;
			

				if(QueryString.ContainsVariable("ObjectInstanceID"))
				{
					// update
					cmd.GetSqlCommand(
						"UPDATE ObjectInstance " +
						"SET X = " + NewX + ", " +
						"Y = " + NewY + ", " +
						"Z = " + NewZ + ", " +
						"TemplateObjectID = " + TemplateList.SelectedValue.ToString() + " " +
						"WHERE ObjectInstanceID = " + QueryString.GetVariableInt32Value("ObjectInstanceID")).ExecuteNonQuery();
				}
				else
				{
					// insert
					cmd.GetSqlCommand(
						"INSERT INTO ObjectInstance " + 
						"(TemplateObjectID, X, Y, Z, RotationX, RotationY, RotationZ, EnergyCurrent, HitpointsCurrent) " +
						"VALUES " + 
						"(" + TemplateList.SelectedValue.ToString() + "," + NewX + "," + NewY + "," + NewZ + ",0,0,0,0,0)").ExecuteNonQuery();

				}
				Page.RegisterClientScriptBlock("Refresh", "<script type=\"text/javascript\">window.parent.frames['Editor'].location.reload(true);</script>");
				Page.RegisterClientScriptBlock("Close", "<script type=\"text/javascript\">location.href='about:blank';</script>");
			}
			catch(Exception c)
			{
				throw new Exception("editterrainpieceobjectinsance.Save_Click", c);
			}
			finally
			{
				cmd.Close();
			}
		

		}

		private void Cancel_Click(object sender, System.EventArgs e) {
			Page.RegisterClientScriptBlock("Close", "<script type=\"text/javascript\">location.href='about:blank';</script>");		
		}

		private void Delete_Click(object sender, System.EventArgs e) {
			using(CommandFactory cmd = new CommandFactory()) {
                cmd.GetSqlCommand("DELETE FROM ObjectInstance WHERE ObjectInstanceID = " + QueryString.GetVariableInt32Value("ObjectInstanceID")).ExecuteNonQuery();
			}
			Page.RegisterClientScriptBlock("Refresh", "<script type=\"text/javascript\">window.parent.frames['Editor'].location.reload(true);</script>");
			Page.RegisterClientScriptBlock("Close", "<script type=\"text/javascript\">location.href='about:blank';</script>");		
		}
	}
}
