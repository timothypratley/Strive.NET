using System;
using System.Collections;

namespace Strive.Resources
{
	/// <summary>
	/// Summary description for TerrrainPieces.
	/// </summary>
	public class TerrainCollection
	{
		public Hashtable terrainPieces = new Hashtable();

		public TerrainCollection() {}

		public void Add( TerrainPiece tp ) {
			foreach ( TerrainPiece tmptp in terrainPieces.Values ) {
				int xdiff = (int)(tp.x - tmptp.x)/100;
				int zdiff = (int)(tp.z - tmptp.z)/100;

				// neighbours
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
				} else if ( xdiff == 1 ) {
					if ( zdiff == 1 ) {
						tp.xminuszminus = tmptp;
						tmptp.xpluszplus = tp;
					} else if ( zdiff == -1 ) {
						tp.xminuszplus = tmptp;
						tmptp.xpluszminus = tp;
					}
				} else if ( xdiff == -1 ) {
					if ( zdiff == 1 ) {
						tp.xpluszminus = tmptp;
						tmptp.xminuszplus = tp;
					} else if ( zdiff == -1 ) {
						tp.xpluszplus = tmptp;
						tmptp.xminuszminus = tp;
					}
				}
			}
			terrainPieces.Add( tp.instance_id, tp );
			ReCreateTerrain();
		}

		public void Remove( int instance_id ) {
			// only remove if it exists
			TerrainPiece tp = (TerrainPiece)terrainPieces[instance_id];
			if ( tp == null ) return;

			// unlink it
			if ( tp.xminus != null ) { tp.xminus.xplus = null; }
			if ( tp.xplus != null ) { tp.xplus.xminus = null; }
			if ( tp.zminus != null ) { tp.zminus.zplus = null; }
			if ( tp.zplus != null ) { tp.zplus.zminus = null; }
			if ( tp.xminuszminus != null ) { tp.xminuszminus.xpluszplus = null; }
			if ( tp.xpluszminus != null ) { tp.xpluszminus.xminuszplus = null; }
			if ( tp.xminuszplus != null ) { tp.xminuszplus.xpluszminus = null; }
			if ( tp.xpluszplus != null ) { tp.xpluszplus.xminuszminus = null; }

			// remove it
			terrainPieces.Remove( instance_id );
		}

		public void ReCreateTerrain() {
			foreach ( TerrainPiece tp in terrainPieces.Values ) {
				tp.Display();
			}
		}
	}
}
