using System;
using System.Collections;

using Strive.Rendering;
using Strive.Math3D;

namespace Strive.Resources
{
	/// <summary>
	/// Summary description for TerrrainPieces.
	/// </summary>
	public class TerrainCollection {
		public Hashtable terrainPieces = new Hashtable();
		IEngine _engine;
		public static int terrainSize = 100;


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

		public float AltitudeAt( float x, float z ) {
			// check every terrain piece, is this point on it?
			foreach ( TerrainPiece t in terrainPieces.Values ) {
				if (
					x >= t.x && x < t.x + terrainSize
					&& z >= t.z && z < t.z + terrainSize
				) {
					// w00t on this piece lookup its height
					// if it is fully defined
					if ( t.xplusKnown && t.zplusKnown && t.xpluszplusKnown ) {
						float dx = x - t.x;
						float dz = z - t.z;

						// terrain is a diagonally split square, forming two triangles
						// which touch the altitude points of 4 neighbouring terrain
						// points, the current terrain and its xplus, zplus, xpluszplus.
						// so for either triangle, just apply the slope in x and z
						// to find the altitude at that point
						float xslope;
						float zslope;
						if ( dz < dx ) {
							// lower triangle
							xslope = ( t.xplus - t.altitude ) / terrainSize;
							zslope = ( t.xpluszplus - t.xplus ) / terrainSize;
						} else {
							// upper triangle
							xslope = ( t.xpluszplus - t.zplus ) / terrainSize;
							zslope = ( t.zplus - t.altitude ) / terrainSize;
						}
						return t.altitude + xslope * dx + zslope * dz;
					} else {
						// terrain piece not defined yet
						return -100;
					}
				}
			}
			// no terrain found
			return -100;
		}
	}
}
