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

namespace www.strive3d.net.players.builders.terrain2
{
	/// <summary>
	/// Summary description for editsquare.
	/// </summary>
	public class editsquare : System.Web.UI.Page
	{
		protected DataTable terrain = new DataTable("Terrain");
		protected DropDownList HeightMapDefaultTexture;
		protected TextBox BaseAltitude;
		protected Button ApplyHeightMap;
		protected System.Web.UI.HtmlControls.HtmlInputFile HeightMap;
		protected int startX;
		protected int startZ;
		protected int endX;
		protected int endZ;

		private void Page_Load(object sender, System.EventArgs e)
		{
			startX = QueryString.GetVariableInt32Value("GroupXStart");
			startZ = QueryString.GetVariableInt32Value("GroupZStart");
			endX = QueryString.GetVariableInt32Value("GroupXEnd");
			endZ = QueryString.GetVariableInt32Value("GroupZEnd");
				CommandFactory cmd = new CommandFactory();		
			try 
			{

				SqlDataAdapter terrainsFiller = new SqlDataAdapter(cmd.EnumTerrainInSquare(startX,
					startZ,
					endX,
					endZ));

				terrainsFiller.Fill(terrain);

				SqlDataAdapter texturefiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT *, 'Replace with ' + LTrim(IsNull(ResourcePak, '') + ' ' + ResourceName) AS ResourceDisplayName FROM Resource WHERE EnumResourceTypeID = 1 ORDER BY ResourceDisplayName"));
				DataTable textures = new DataTable();
				texturefiller.Fill(textures);

				/*eightMapDefaultTexture.DataSource = textures;
				HeightMapDefaultTexture.DataBind();

				HeightMapDefaultTexture.Items.Insert(0, new ListItem("Leave existing textures", ""));*/

	
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
