using System;
using System.Collections;

using Strive.Resources;
using Strive.Rendering;
using Strive.Math3D;

namespace Strive.UI.WorldView
{
	/// <summary>
	/// Summary description for TerrrainPieces.
	/// </summary>
	public class TerrainCollection {
		public Hashtable terrainPieces = new Hashtable();
		public Hashtable terrainPiecesXYIndex = new Hashtable();
		IEngine _engine;
		IScene _scene;
		ResourceManager _resource_manager;
		public static int terrainSize = 10;


		public TerrainCollection( ResourceManager rm, IEngine engine, IScene scene ) {
			_engine = engine;
			_scene = scene;
			_resource_manager = rm;
		}

		public void Add( TerrainPiece tp ) {
			TerrainPiece tpexists = terrainPieces[tp.instance_id] as TerrainPiece;
			if ( tpexists != null ) {
				if ( tp.x == tpexists.x && tp.z == tpexists.z &&tp.altitude == tpexists.altitude ) {
					// already have this peice
					Strive.Logging.Log.WarningMessage( "Tried to add duplicate terrain peice " + tp.instance_id );
					return;
				} else {
					Remove( tpexists.instance_id );
				}
			}

			int x = (int)tp.x/terrainSize;
			int z = (int)tp.z/terrainSize;
			Vector2D loc = new Vector2D( x, z );
			tpexists = terrainPiecesXYIndex[loc] as TerrainPiece;
			if ( tpexists != null ) {
				Strive.Logging.Log.WarningMessage( "Replacing terrain peice " + tpexists.instance_id + " with " + tp.instance_id );
				Remove( tpexists.instance_id );
			}

			terrainPieces.Add( tp.instance_id, tp );
			terrainPiecesXYIndex.Add( loc, tp );

			// everybody needs good neighbours
			TerrainPiece tptmp;
			for ( int i=-1; i<=1; i++ ) {
				for ( int j=-1; j<=1; j++ ) {
					if ( i==0 && j == 0 ) {
						continue;
					} else {
						loc.Set(x+i, z+j );
						tptmp = (TerrainPiece)terrainPiecesXYIndex[ loc ];
					}
					if ( tptmp != null ) {
						if ( i==-1 && j==0 ) {
							tptmp.xplus = tp.altitude;
							tptmp.xplusKnown = true;
						} else if ( i==-1 && j == -1 ) {
							tptmp.xpluszplus = tp.altitude;
							tptmp.xpluszplusKnown = true;
						} else if ( i==0 && j == -1 ) {
							tptmp.zplus = tp.altitude;
							tptmp.zplusKnown = true;
						} else if ( i==1 && j==0 ) {
							tp.xplus = tptmp.altitude;
							tp.xplusKnown = true;
						} else if ( i==1 && j == 1 ) {
							tp.xpluszplus = tptmp.altitude;
							tp.xpluszplusKnown = true;
						} else if ( i==0 && j == 1 ) {
							tp.zplus = tptmp.altitude;
							tp.zplusKnown = true;
						}

						if ( tptmp.model == null && tptmp.xplusKnown && tptmp.zplusKnown && tptmp.xpluszplusKnown ) {
							if ( tptmp.model != null ) _scene.Models.Remove( tptmp.instance_id );
							try {
								tptmp.model = _engine.CreateTerrain( tptmp.instance_id.ToString(), _resource_manager.GetTexture(tptmp.texture_id), tptmp.physicalObject.Rotation.Y, tptmp.altitude, tptmp.xplus, tptmp.zplus, tptmp.xpluszplus );
							} catch ( Exception e ) {
								Exception xe = new Exception( "Failed to create terrain peice.", e );
								Strive.Logging.Log.ErrorMessage( xe );
								return;
							}
							tptmp.model.Position = new Vector3D( tptmp.x, 0, tptmp.z );
							_scene.Models.Add( tptmp.instance_id, tptmp.model );
						}
					}
				}
				if ( tp.model == null && tp.xplusKnown && tp.zplusKnown && tp.xpluszplusKnown ) {
					if ( tp.model != null ) _scene.Models.Remove( tp.instance_id );
					tp.model = _engine.CreateTerrain( tp.instance_id.ToString(), _resource_manager.GetTexture(tp.texture_id), tp.physicalObject.Rotation.Y, tp.altitude, tp.xplus, tp.zplus, tp.xpluszplus );
					tp.model.Position = new Vector3D( tp.x, 0, tp.z );
					_scene.Models.Add( tp.instance_id, tp.model );
				}
			}
			DateTime b = DateTime.Now;
		}

		public void Remove( int instance_id ) {
			// remove it
			TerrainPiece tp = terrainPieces[ instance_id ] as TerrainPiece;
			if ( tp != null ) {
				if ( tp.model != null ) {
					_scene.Models.Remove( tp.instance_id );
				}
				terrainPieces.Remove( instance_id );
				int x = (int)tp.x/terrainSize;
				int z = (int)tp.z/terrainSize;
				Vector2D loc = new Vector2D( x, z );
				terrainPiecesXYIndex.Remove( loc );
			}
		}

		public void Clear() {
			foreach( TerrainPiece tp in terrainPieces.Values ){
				if ( tp.model != null ) _scene.Models.Remove( tp.instance_id );
			}
			terrainPieces.Clear();
		}

		public class InvalidLocationException : Exception {}
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
						throw new InvalidLocationException();
					}
				}
			}
			// no terrain found
			throw new InvalidLocationException();
		}
	}
}
