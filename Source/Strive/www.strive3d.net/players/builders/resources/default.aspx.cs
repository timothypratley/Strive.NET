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
namespace www.strive3d.net.players.builders.resources
{
	/// <summary>
	/// Summary description for _default.
	/// </summary>
	public class _default : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DataList DataList1;
		protected DataTable resourceTypes = new DataTable();
		private void Page_Load(object sender, System.EventArgs e)
		{
			CommandFactory cmd = new CommandFactory();

			string ResourceSql = "SELECT Resource.*, EnumResourceType.EnumResourceTypeName FROM Resource INNER JOIN EnumResourceType ON Resource.EnumResourceTypeID = EnumResourceType.EnumResourceTypeID " + (Request.QueryString["EnumResourceTypeID"] != null ? " WHERE EnumResourceType.EnumResourceTypeID = " + Request.QueryString["EnumResourceTypeID"] : "") + " ORDER BY EnumResourceTypeName, ResourcePak, ResourceName";
			
			SqlDataAdapter resourceFiller = new SqlDataAdapter(cmd.GetSqlCommand(ResourceSql));

			DataTable resource = new DataTable();

			resourceFiller.Fill(resource);

			DataList1.DataSource = resource;

			DataList1.DataBind();

			SqlDataAdapter resourceTypeFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM EnumResourceType ORDER BY EnumResourceTypeName"));

			resourceTypeFiller.Fill(resourceTypes);
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
