using System;
using System.Data;
using System.Drawing;

// TODO: this class requires these 2 references, should it belong elsewhere?
using Strive.Common;

namespace Strive.Data {
	/// <summary>
	/// TerrainHeightMapHelper creates maps from the database,
	/// and can update the database from maps.
	/// </summary>
	public class TerrainHeightMapHelper	{

		public static DataSet CreateTerrainData( Bitmap heightmap, Bitmap texturemap, float start_x, float start_z ) {
			DataSet ds = new DataSet();
			int row, col;
			for ( row=0; row<heightmap.Height; row++ ) {
				for ( col=0; col<heightmap.Width; col++ ) {
					float x = col * Constants.terrainPieceSize;
					float y = heightmap.GetPixel(row, col).GetBrightness();
					float z = (heightmap.Height-row-1) * Constants.terrainPieceSize;
					int terrain_id = (int)texturemap.GetPixel(row, col).GetBrightness();
					ds.Insert( x, y, z, terrain_id );
				}
			}
		}


		public static Bitmap CreateTexturemapImageFromData( DataTable data, float start_x, float start_z, int width, int height ) {
			Bitmap image = new Bitmap( width, height );
			foreach ( DataRow dr in data.Rows ) {
				int col = (int)(((float)dr["X"]-start_x) / Constants.terrainPieceSize);
				int row = height - (int)(((float)dr["Z"]-start_z) / Constants.terrainPieceSize) - 1;
				Color color = new Color();
				int id = (int)dr["TemplateObjectID"];
				switch ( id ) {
					case 0: color = Color.Red; break;
					case 1: color = Color.Blue; break;
					default: throw new Exception( "Unexpected TemplateObjectID " + id );
				}
				image.SetPixel( col, row, color );
			}
			return image;
		}

		public static Bitmap CreateHeightmapImageFromData( DataTable data, float start_x, float start_z, int width, int height ) {
			Bitmap image = new Bitmap( width, height );
			foreach ( DataRow dr in data.Rows ) {
				int col = (int)(((float)dr["X"]-start_x) / Constants.terrainPieceSize);
				int row = height - (int)(((float)dr["Z"]-start_z) / Constants.terrainPieceSize) - 1;
				byte alt = (byte)dr["Y"];
				Color color = Color.FromArgb( alt, alt, alt );
				image.SetPixel( col, row, color );
			}
			return image;
		}
	}
}
