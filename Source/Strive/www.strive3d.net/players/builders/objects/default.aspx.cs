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

namespace www.strive3d.net.players.builders.objects
{
	/// <summary>
	/// Summary description for _default.
	/// </summary>
	public class _default : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Repeater TemplateMobileList;
		protected System.Web.UI.WebControls.Repeater TemplateItemJunkList;
		protected System.Web.UI.WebControls.Repeater TemplateItemQuaffableList;
protected System.Web.UI.WebControls.Repeater TemplateItemEquipableList;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			CommandFactory cmd = new CommandFactory();
			try
			{
				SqlDataAdapter TemplateMobileFiller = new SqlDataAdapter(
					cmd.GetSqlCommand(
						"SELECT TemplateMobile.*, " +
						"TemplateObject.TemplateObjectName " +
						"FROM TemplateMobile " +
						"INNER JOIN TemplateObject " +
						"ON TemplateMobile.TemplateObjectID = TemplateObject.TemplateObjectID " +
						"ORDER BY TemplateObjectName "));

				DataTable TemplateMobiles = new DataTable();
				
				TemplateMobileFiller.Fill(TemplateMobiles);
				TemplateMobileList.DataSource = TemplateMobiles;
				TemplateMobileList.DataBind();

				SqlDataAdapter TemplateItemEquipableFiller = new SqlDataAdapter(
					cmd.GetSqlCommand(
					"SELECT TemplateItemEquipable.*, " +
					"TemplateObject.TemplateObjectName, " +
					"TemplateItem.Value, " +
					"TemplateItem.Weight, " +
					"TemplateItem.EnumItemDurabilityID " +
					"FROM TemplateItemEquipable " +
					"INNER JOIN TemplateItem " +
					"ON TemplateItemEquipable.TemplateObjectID = TemplateItem.TemplateObjectID " +
					"INNER JOIN TemplateObject " +
					"ON TemplateItem.TemplateObjectID = TemplateObject.TemplateObjectID " +
					"ORDER BY TemplateObjectName "));

				DataTable TemplateItemEquipables = new DataTable();
				
				TemplateItemEquipableFiller.Fill(TemplateItemEquipables);
				TemplateItemEquipableList.DataSource = TemplateItemEquipables;
				TemplateItemEquipableList.DataBind();

				SqlDataAdapter TemplateItemJunkFiller = new SqlDataAdapter(
					cmd.GetSqlCommand(
					"SELECT TemplateItemJunk.*, " +
					"TemplateObject.TemplateObjectName, " +
					"TemplateItem.Value, " +
					"TemplateItem.Weight, " +
					"TemplateItem.EnumItemDurabilityID " +
					"FROM TemplateItemJunk " +
					"INNER JOIN TemplateItem " +
					"ON TemplateItemJunk.TemplateObjectID = TemplateItem.TemplateObjectID " +
					"INNER JOIN TemplateObject " +
					"ON TemplateItem.TemplateObjectID = TemplateObject.TemplateObjectID " +
					"ORDER BY TemplateObjectName "));

				DataTable TemplateItemJunks = new DataTable();
				
				TemplateItemJunkFiller.Fill(TemplateItemJunks);
				TemplateItemJunkList.DataSource = TemplateItemJunks;
				TemplateItemJunkList.DataBind();

				SqlDataAdapter TemplateItemQuaffableFiller = new SqlDataAdapter(
					cmd.GetSqlCommand(
					"SELECT TemplateItemQuaffable.*, " +
					"TemplateObject.TemplateObjectName, " +
					"TemplateItem.Value, " +
					"TemplateItem.Weight, " +
					"TemplateItem.EnumItemDurabilityID " +
					"FROM TemplateItemQuaffable " +
					"INNER JOIN TemplateItem " +
					"ON TemplateItemQuaffable.TemplateObjectID = TemplateItem.TemplateObjectID " +
					"INNER JOIN TemplateObject " +
					"ON TemplateItem.TemplateObjectID = TemplateObject.TemplateObjectID " +
					"ORDER BY TemplateObjectName "));

				DataTable TemplateItemQuaffables = new DataTable();
				
				TemplateItemQuaffableFiller.Fill(TemplateItemQuaffables);
				TemplateItemQuaffableList.DataSource = TemplateItemQuaffables;
				TemplateItemQuaffableList.DataBind();
					
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
