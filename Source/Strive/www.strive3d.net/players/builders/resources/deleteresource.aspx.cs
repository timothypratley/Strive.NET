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

namespace www.strive3d.net.players.builders.resources
{
	/// <summary>
	/// Summary description for deleteresource.
	/// </summary>
	public class deleteresource : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			CommandFactory cmd = new CommandFactory();
			SqlTransaction trans = cmd.Connection.BeginTransaction();
			try
			{
				string EnumResourceTypeName = "";
				string ResourceFileExtension = "";

				SqlCommand selectSingleResource = cmd.GetSqlCommand("SELECT Resource.*, EnumResourceType.EnumResourceTypeName FROM Resource INNER JOIN EnumResourceType ON Resource.EnumResourceTypeID = EnumResourceType.EnumResourceTypeID WHERE ResourceID = " + QueryString.GetVariableStringValue("ResourceID"));
				selectSingleResource.Transaction = trans;
				SqlDataReader enumResourceReader = selectSingleResource.ExecuteReader();
				try
				{
					while(enumResourceReader.Read())
					{
						EnumResourceTypeName = (string)enumResourceReader["EnumResourceTypeName"];
						ResourceFileExtension = (string)enumResourceReader["ResourceFileExtension"];
					}
				}
				catch(Exception rx)
				{
					throw new Exception("Command [" + selectSingleResource.CommandText + "] failed", rx);
				}
				finally
				{
					enumResourceReader.Close();
				}

				string resourcepath = Server.MapPath( "./" + EnumResourceTypeName + "/" + QueryString.GetVariableInt32Value("ResourceID").ToString() + ResourceFileExtension);

				SqlCommand resourceDelet0r = cmd.GetSqlCommand("DELETE FROM Resource WHERE ResourceID = " + QueryString.GetVariableInt32Value("ResourceID").ToString());
				resourceDelet0r.Transaction = trans;
				resourceDelet0r.ExecuteNonQuery();

				System.IO.File.Delete(resourcepath);
				trans.Commit();
			
			}
			catch(Exception ex)
			{
				trans.Rollback();
				throw new Exception("deleteresource.Page_Load", ex);
			}

			Response.Redirect("./");

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
