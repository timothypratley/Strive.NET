using System;
using System.Collections;

using Strive.Rendering;
using Strive.Rendering.Models;

namespace Strive.Resources
{
	/// <summary>
	/// Summary description for TerrrainPieces.
	/// </summary>
	public class TerrainCollection
	{
		public Hashtable terrainPieces = new Hashtable();
		public Scene scene;

		public TerrainCollection( Scene scene ) {
			this.scene = scene;
		}

		public void Add( TerrainPiece tp ) {
			foreach ( TerrainPiece tmptp in terrainPieces.Values ) {
				int xdiff = (int)(tp.x - tmptp.x)/100;
				int zdiff = (int)(tp.z - tmptp.z)/100;
				if ( xdiff == 0 ) {
					if ( zdiff == 1 ) {
						tp.zminus = tmptp;
						tmptp.zplus = tp;
					} else if ( zdiff == -1 ) {
						tp.zplus = tmptp;
						tmptp.zminus = tp;
					}
				} else if ( zdiff == 0 ) {
					if ( xdiff == 1 ) {
						tp.xminus = tmptp;
						tmptp.xplus = tp;
					} else if ( xdiff == -1 ) {
						tp.xplus = tmptp;
						tmptp.xminus = tp;
					}
				}
			}
			terrainPieces.Add( tp.instance_id, tp );
			ReCreateTerrain();
		}

		public void Remove( int instance_id ) {
			TerrainPiece tp = (TerrainPiece)terrainPieces[instance_id];
			if ( tp == null ) return;

			// unlink it
			if ( tp.xminus != null ) { tp.xminus.xplus = null; }
			if ( tp.xplus != null ) { tp.xplus.xminus = null; }
			if ( tp.zminus != null ) { tp.zminus.zplus = null; }
			if ( tp.zplus != null ) { tp.zplus.zminus = null; }
			terrainPieces.Remove( instance_id );
		}

		public void ReCreateTerrain() {
			foreach ( TerrainPiece tp in terrainPieces.Values ) {
				scene.Models.Remove( tp.instance_id.ToString() );
				scene.Models.Add( tp.CreateModel() );
			}
		}
	}
}
