using System;
using System.Collections;

using Strive.Rendering;
using Strive.Math3D;

namespace Strive.Resources
{
	/// <summary>
	/// Summary description for TerrrainPieces.
	/// </summary>
	public class TerrainCollection
	{
		public Hashtable terrainPieces = new Hashtable();
		IEngine _engine;

		public TerrainCollection( IEngine engine ) {
			_engine = engine;
		}

		public void Add( TerrainPiece tp ) {
			foreach ( TerrainPiece tmptp in terrainPieces.Values ) {
				int xdiff = (int)(tp.x - tmptp.x)/100;
				int zdiff = (int)(tp.z - tmptp.z)/100;

				// everybody needs good neighbours
				if ( xdiff == 0 ) {
					if ( zdiff == 1 ) {
						tmptp.zplus = tp.altitude;
						tmptp.zplusKnown = true;
					} else if ( zdiff == -1 ) {
						tp.zplus = tmptp.altitude;
						tp.zplusKnown = true;
					}
				} else if ( zdiff == 0 ) {
					if ( xdiff == 1 ) {
						tmptp.xplus = tp.altitude;
						tmptp.xplusKnown = true;
					} else if ( xdiff == -1 ) {
						tp.xplus = tmptp.altitude;
						tp.xplusKnown = true;
					}
				} else if ( xdiff == 1 ) {
					if ( zdiff == 1 ) {
						tmptp.xpluszplus = tp.altitude;
						tmptp.xpluszplusKnown = true;
					}
				} else if ( xdiff == -1 ) {
					if ( zdiff == -1 ) {
						tp.xpluszplus = tmptp.altitude;
						tp.xpluszplusKnown = true;
					}
				}
				if ( tmptp.model == null && tmptp.xplusKnown && tmptp.zplusKnown && tmptp.xpluszplusKnown ) {
					
					tmptp.model = _engine.CreateTerrain( tmptp.instance_id.ToString(), ResourceManager.LoadTexture( tmptp.texture_id), tmptp.altitude, tmptp.xplus, tmptp.zplus, tmptp.xpluszplus );
					tmptp.model.Position = new Vector3D( tmptp.x, 0, tmptp.z );
				}
			}
			if ( tp.model == null && tp.xplusKnown && tp.zplusKnown && tp.xpluszplusKnown ) {
				tp.model = _engine.CreateTerrain( tp.instance_id.ToString(), ResourceManager.LoadTexture( tp.texture_id ), tp.altitude, tp.xplus, tp.zplus, tp.xpluszplus );
				tp.model.Position = new Vector3D( tp.x, 0, tp.z );
			}
			terrainPieces.Add( tp.instance_id, tp );
		}

		public void Remove( int instance_id ) {
			// remove it
			terrainPieces.Remove( instance_id );
		}
	}
}
