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
using thisterminal.Web;
using System.Data.SqlClient;
using www.strive3d.net.Game;

namespace www.strive3d.net.players.builders.terrain
{
	/// <summary>
	/// Summary description for WorldMap.
	/// </summary>
	public class WorldMap : System.Web.UI.Page
	{
		public enum SupportedMap {
			HeightMap,
			TerrainMap
		}
		private void Page_Load(object sender, System.EventArgs e)
		{
			Server.ScriptTimeout = 9999;
			SupportedMap WorldMapType = SupportedMap.TerrainMap;

			if(QueryString.ContainsVariable("MapType")) {
				WorldMapType = (SupportedMap)Enum.Parse(typeof(SupportedMap), QueryString.GetVariableStringValue("MapType"), false);
			}

			byte[] image = (byte[])Cache["WorldMap" + WorldMapType.ToString()];
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
					SqlCommand worldMapLoader = cmd.GetSqlCommand("SELECT ObjectInstance.X, ObjectInstance.Y, ObjectInstance.Z, TemplateTerrain.EnumTerrainTypeID FROM ObjectInstance INNER JOIN TemplateTerrain ON ObjectInstance.TemplateObjectID = TemplateTerrain.TemplateObjectID ");

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
						int pixelX = (int)(Math.Abs((MinX + float.Parse(worldReader["X"].ToString()))) / Strive.Common.Constants.terrainPieceSize) ;
						float Altitude = float.Parse(worldReader["Y"].ToString());
						if(Altitude < 0 ) {
							 Altitude = 0;
						}
						int pixelZ = (int)(Math.Abs((MinZ + float.Parse(worldReader["Z"].ToString()))) / Strive.Common.Constants.terrainPieceSize) ;
						int EnumTerrainTypeID = int.Parse(worldReader["EnumTerrainTypeID"].ToString());
						try
						{
							switch(WorldMapType) {
								case SupportedMap.TerrainMap: {
									theMap.SetPixel(pixelX, pixelZ, (System.Drawing.Color)colourConverter[EnumTerrainTypeID]);
									break;
								}
								case SupportedMap.HeightMap: {
									theMap.SetPixel(pixelX, pixelZ, Color.FromArgb((int)Altitude, (int)Altitude, (int)Altitude));
									break;
								}

							}
						}
						catch(Exception ex)
						{
							throw new Exception("Failed setting X[" + pixelX+ "], Y[" + pixelZ+ "], EnumTerrainTypeID[" + EnumTerrainTypeID + "]", ex);
						}
				
					}
					worldReader.Close();
					for(int col = 0; col < theMap.Width; col ++) {
						for(int row = 0; row < theMap.Height; row ++) {
							if(theMap.GetPixel(col, row).R  < 1 && col > 0 && WorldMapType == SupportedMap.HeightMap) {
							//	throw new Exception("col[" + col + "]row[" + row + "][" + theMap.GetPixel(col-1, row).ToString() + "]");
								theMap.SetPixel(col, row, theMap.GetPixel(col-1, row));

								float WorldX = MinX + (col * Strive.Common.Constants.terrainPieceSize);
								float PrevWorldX = MinX + ((col - 1) * Strive.Common.Constants.terrainPieceSize);
								float WorldZ = MinZ + (row * Strive.Common.Constants.terrainPieceSize);

								cmd.GetSqlCommand("DELETE FROM ObjectInstance WHERE X = " + WorldX + " AND Z = " + WorldZ).ExecuteNonQuery();
								cmd.GetSqlCommand("INSERT INTO ObjectInstance " +
									"SELECT ObjectInstance.TemplateObjectID, " +
									WorldX + " AS X, " +
									"Y, " + 
									WorldZ + " AS Z, " +
									"0 As RotationX, " +
									"0 As RotationY, " +
									"0 AS RotationZ, " +
									"0 AS EnergyCurrent, " +
									"0 AS HitpointsCurrent " +
									"FROM ObjectInstance " + 
									"WHERE X = " + PrevWorldX + " " +
									"AND Z = " + WorldZ + " ").ExecuteNonQuery();

							}
						}
					}
					System.IO.MemoryStream newImage = new System.IO.MemoryStream();
					theMap.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipX);
					theMap.Save(newImage, System.Drawing.Imaging.ImageFormat.Png);
					image = newImage.GetBuffer();
					Cache.Add("WorldMap" + WorldMapType.ToString(), image, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);
				}
				catch(Exception c)
				{
					throw new Exception("Generating world map [" + WorldMapType.ToString() + "] failed", c);
				}
				finally
				{
					cmd.Close();
				}
			}

			Response.Clear();
			Response.ContentType = "image/png";
			Response.AddHeader("Content-Disposition", "filename=\"WorldMap" + WorldMapType.ToString() + ".png\"");
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
