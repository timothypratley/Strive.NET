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


namespace www.strive3d.net.players.builders.terrain
{
	/// <summary>
	/// Summary description for _default.
	/// </summary>
	public class _default : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(IsPostBack)
			{
				CommandFactory cmd = new CommandFactory();
				float Width;
				float Height;
				float MinX;
				float MinZ;
				float MaxZ;
				try
				{

					//SqlCommand squaresLoader = cmd.TerrainSquareDetails(int.Parse(TextBox2.Text), int.Parse(TextBox1.Text));
					//SqlDataAdapter squaresFiller = new SqlDataAdapter(squaresLoader);
					//squares = new DataTable("Squares");
					//squaresFiller.Fill(squares);

					SqlDataReader worldStats = cmd.GetSqlCommand("SELECT MAX(X) - MIN(X) AS Width, MAX(Z) AS MaxZ, MAX(Z) - MIN(Z) AS Height, MIN(X) As MinX, MIN(Z) AS MinZ FROM ObjectInstance INNER JOIN TemplateTerrain ON ObjectInstance.TemplateObjectID = TemplateTerrain.TemplateObjectID").ExecuteReader();


					if(worldStats.Read())
					{
						Width = float.Parse(worldStats["Width"].ToString());
						Height = float.Parse(worldStats["Height"].ToString());
						MinX = float.Parse(worldStats["MinX"].ToString());
						MinZ = float.Parse(worldStats["MinZ"].ToString());
						MaxZ = float.Parse(worldStats["MaxZ"].ToString());
						worldStats.Close();
						

					}
					else
					{
						worldStats.Close();
						throw new Exception("Could not collect world statistics");
					}
				}
				catch(Exception c)
				{
					throw new Exception("Generating world map failed", c);
				}
				finally
				{
					cmd.Close();
				}
				int startX = (int.Parse(Request.Form["x"].ToString()) * Strive.Common.Constants.terrainPieceSize) + (int)MinX;
				int startZ = (int)MaxZ - (int.Parse(Request.Form["y"].ToString()) * Strive.Common.Constants.terrainPieceSize);
				// reverse y
				//startZ = (int)Height - startZ;

				int endX = startX + 99;
				int endZ = startZ + 99;
				
				Response.Redirect("./editsquare.aspx?GroupXStart=" + startX+ "&GroupXEnd=" + endX+ "&GroupZStart=" +startZ + "&GroupZEnd=" + endZ+ Utils.TabHref);
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

		private void Redraw_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
