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
	/// Summary description for WorldMap.
	/// </summary>
	public class WorldMap : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{

			byte[] image = (byte[])Cache["WorldMap"];
			if(image == null)
			{

				CommandFactory cmd = new CommandFactory();
				try
				{

					//SqlCommand squaresLoader = cmd.TerrainSquareDetails(int.Parse(TextBox2.Text), int.Parse(TextBox1.Text));
					//SqlDataAdapter squaresFiller = new SqlDataAdapter(squaresLoader);
					//squares = new DataTable("Squares");
					//squaresFiller.Fill(squares);

					SqlDataReader worldStats = cmd.GetSqlCommand("SELECT MAX(X) - MIN(X) AS Width, MAX(Z) - MIN(Z) AS Height, MIN(X) As MinX, MIN(Z) AS MinZ FROM ObjectInstance INNER JOIN TemplateTerrain ON ObjectInstance.TemplateObjectID = TemplateTerrain.TemplateObjectID").ExecuteReader();

					float Width;
					float Height;
					float MinX;
					float MinZ;
					if(worldStats.Read())
					{
						Width = float.Parse(worldStats["Width"].ToString()) / Strive.Common.Constants.terrainPieceSize;
						Height = float.Parse(worldStats["Height"].ToString())  / Strive.Common.Constants.terrainPieceSize ;
						MinX = float.Parse(worldStats["MinX"].ToString());
						MinZ = float.Parse(worldStats["MinZ"].ToString());
						worldStats.Close();
						

					}
					else
					{
						worldStats.Close();
						throw new Exception("Could not collect world statistics");
					}



					System.Drawing.Bitmap theMap = new System.Drawing.Bitmap((int)Width+2, (int)Height+2);
			
					// build bitmap:
					SqlCommand worldMapLoader = cmd.GetSqlCommand("SELECT ObjectInstance.*, EnumTerrainType.* FROM ObjectInstance INNER JOIN TemplateTerrain ON ObjectInstance.TemplateObjectID = TemplateTerrain.TemplateObjectID INNER JOIN EnumTerrainType ON TemplateTerrain.EnumTerrainTypeID = EnumTerrainType.EnumTerrainTypeID ORDER BY X, Z");

					Hashtable colourConverter = new Hashtable();
					colourConverter.Add(1, System.Drawing.Color.LightGreen);
					colourConverter.Add(2, System.Drawing.Color.Brown);
					colourConverter.Add(3, System.Drawing.Color.White);
					colourConverter.Add(4, System.Drawing.Color.DarkOliveGreen);
					colourConverter.Add(5, System.Drawing.Color.Black);
					colourConverter.Add(6, System.Drawing.Color.Red);
					colourConverter.Add(7, System.Drawing.Color.Silver);
					colourConverter.Add(8, System.Drawing.Color.Blue);
					colourConverter.Add(9, System.Drawing.Color.DarkGreen);
					colourConverter.Add(10, System.Drawing.Color.Silver);
				
					SqlDataReader worldReader = worldMapLoader.ExecuteReader();
					while(worldReader.Read())
					{
						int pixelX = (int)(Math.Abs((MinX + float.Parse(worldReader["X"].ToString()))) / 10) ;
						int pixelZ = (int)(Math.Abs((MinZ + float.Parse(worldReader["Z"].ToString()))) / 10) ;
						int EnumTerrainTypeID = int.Parse(worldReader["EnumTerrainTypeID"].ToString());
						try
						{
							theMap.SetPixel(pixelX, pixelZ, (System.Drawing.Color)colourConverter[EnumTerrainTypeID]);
						}
						catch(Exception ex)
						{
							throw new Exception("Failed setting X[" + pixelX+ "], Y[" + pixelZ+ "], EnumTerrainTypeID[" + EnumTerrainTypeID + "]", ex);
						}
				
					}
					worldReader.Close();
					System.IO.MemoryStream newImage = new System.IO.MemoryStream();
					theMap.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipX);
					theMap.Save(newImage, System.Drawing.Imaging.ImageFormat.Png);
					image = newImage.GetBuffer();
					Cache.Add("WorldMap", image, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);
				}
				catch(Exception c)
				{
					throw new Exception("Generating world map failed", c);
				}
				finally
				{
					cmd.Close();
				}
			}

			Response.Clear();
			Response.ContentType = "image/png";
			Response.AddHeader("Content-Disposition", "filename=\"WorldMap.png\"");
			Response.BinaryWrite(image);
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
