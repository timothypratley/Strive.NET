using System;
using System.Collections;

using Strive.Resources;
using Strive.Rendering;
using Strive.Rendering.Models;
using Strive.Math3D;
using Strive.Multiverse;
using Strive.Common;

namespace Strive.UI.WorldView {
	/// <summary>
	/// Summary description for TerrrainPieces.
	/// </summary>
	public class TerrainCollection {
		public Hashtable terrainPiecesXYIndex = new Hashtable();
		IEngine _engine;
		IScene _scene;
		ResourceManager _resource_manager;

		const int xorder = Constants.terrainXOrder;
		const int zorder = Constants.terrainZOrder;
		const int zoomorder = Constants.terrainZoomOrder;
		public ITerrainChunk [,,] TC = new ITerrainChunk[xorder,zorder,zoomorder];
		int [] CX = new int[zoomorder];
		int [] CZ = new int[zoomorder];
		int ts = Constants.terrainPieceSize;
		int hpc = Constants.terrainHeightsPerChunk;
		float center_x, center_z;

		public TerrainCollection( ResourceManager rm, IEngine engine, IScene scene ) {
			_engine = engine;
			_scene = scene;
			_resource_manager = rm;

			// now create chunks at the empty points
			int i, j, k, cs;
			center_x = 0;
			center_z = 0;
			for ( k=0; k<zoomorder; k++ ) {
				cs = (int)(ts*Math.Pow(hpc,k+1));
				CX[k] = 0;
				CZ[k] = 0;
				for ( i=0; i<xorder; i++ ) {
					for ( j=0; j<zorder; j++ ) {
						TC[i,j,k] = _engine.CreateTerrainChunk(i*cs, j*cs, cs/hpc, hpc);
						if ( k > 0 ) {
							// TODO: bad, don't use hardcoded 20!
							TC[i,j,k].SetTexture( _resource_manager.GetTexture( 20 ).ID );
						}
					}
				}
			}
		}

		public void Recenter( float x, float z ) {
			int i, j, k, cs;
			int xdiff, zdiff;
			float tcx, tcz, px, pz;
			int l, m;
			Vector2D loc;
			Terrain t;

			// first loop through all chunks to see if any should be reused
			int cx = Helper.DivTruncate( (int)x, ts );
			int cz = Helper.DivTruncate( (int)z, ts );
			for ( k=0; k<zoomorder; k++ ) {
				cs = (int)(ts*Math.Pow(hpc,k+1));
				cx = Helper.DivTruncate( cx, hpc );
				cz = Helper.DivTruncate( cz, hpc );
				if ( x - (cx*cs) < (cx+1)*cs - x ) cx--;
				if ( z - (cz*cs) < (cz+1)*cs - z ) cz--;
				xdiff = cx - CX[k];
				zdiff = cz - CZ[k];

				// no difference, keep all the chunks
				if ( xdiff == 0 && zdiff == 0 ) continue;

				for ( i=(xdiff<0?xorder-1:0); (xdiff<0?i>=0:i<xorder); ) {
					for ( j=(zdiff<0?zorder-1:0); (zdiff<0?j>=0:j<zorder); ) {
						// see if we can replace it with an existing chunk
						tcx = cx*cs + i*cs;
						tcz = cz*cs + j*cs;
						if (
							i+xdiff >= 0
							&& i+xdiff < xorder
							&& j+zdiff >= 0
							&& j+zdiff < zorder
							) {
							// swap
							ITerrainChunk tc = TC[i,j,k];
							TC[i,j,k] = TC[i+xdiff,j+zdiff,k];
							TC[i+xdiff,j+zdiff,k] = tc;
						} else {
							TC[i,j,k].Position = new Vector3D(tcx, 0, tcz );
							// update it with any known heights
							// Refresh( TC[i,j,k] );
							for (l=0; l<=hpc; l++ ) {
								for (m=0; m<=hpc; m++) {
									px = tcx + l*cs/hpc;
									pz = tcz + m*cs/hpc;
									loc = new Vector2D( px, pz );
									t = (Terrain)terrainPiecesXYIndex[loc];
									if ( t != null ) {
										TC[i,j,k].SetHeight( px, pz, t.Position.Y );
										if ( k==0 ) TC[i,j,k].SetTexture( _resource_manager.GetTexture( t.ResourceID ).ID, px, pz, t.Rotation.Y );
									} else {
										TC[i,j,k].SetHeight( px, pz, 0 );
									}
								}
							}
						}
						if ( zdiff<0 ) j--; else j++;
					}
					if ( xdiff<0 ) i--; else i++;
				}

				CX[k] = cx;
				CZ[k] = cz;
			}

			/***
			// Make sure we update the previous x,z heights for underlying landscapes
			// to their correct values
			loc = new Vector2D( prev_x, prev_z );
			t = (Terrain)terrainPiecesXYIndex[loc];
			if ( t != null ) {
				Set( t.Position.X, t.Position.Z, t.Position.Y, -1, 0 );
			}

			// set underlying landscapes to 0 at x,z
			Set( x, z, 0, -1, 0 );
			*/

			center_x = x; center_z = z;
		}

		public void AddMany( float start_x, float start_z, int width, int height, int gap_size, Terrain [,] map ) {
			foreach ( Terrain t in map ) {
				Add( t );
			}
		}

		public void Set( float x, float z, float altitude, int texture_id, float rotation ) {
			int cs = ts;
			int cx = Helper.DivTruncate( (int)x, ts );
			int cz = Helper.DivTruncate( (int)z, ts );
			int xdiff, zdiff;
			int k;

			if ( texture_id == -1 ) {
				// This should only be applied to the low detail landscapes
				k=1;

				// miss one iteration, so advance cs and prediv cx,cz
				cs = cs*hpc;
				cx = Helper.DivTruncate( cx, hpc );
				cz = Helper.DivTruncate( cz, hpc );
			} else {
				k=0;
			}

			for ( ; k<zoomorder; k++ ) {
				if ( texture_id == -1 ) {
					if ( x - (cx*cs) >= (cx+1)*cs - x ) cx++;
					if ( z - (cz*cs) >= (cz+1)*cs - z ) cz++;
					x = cx*cs;
					z = cz*cs;
				}
				cs = cs*hpc;
				cx = Helper.DivTruncate( cx, hpc );
				cz = Helper.DivTruncate( cz, hpc );
				xdiff = cx - CX[k];
				zdiff = cz - CZ[k];

				if ( xdiff >= 0 && xdiff < xorder && zdiff >= 0 && zdiff < zorder ) {
					TC[xdiff,zdiff,k].SetHeight( x, z, altitude );
					if ( k==0 ) TC[xdiff,zdiff,k].SetTexture( texture_id, x, z, rotation );
				}

				// edges need to update both chunks
				if ( x % cs == 0 ) {
					if ( xdiff-1 >= 0 && xdiff-1 < xorder && zdiff >= 0 && zdiff < zorder ) {
						TC[xdiff-1,zdiff,k].SetHeight( x, z, altitude );
					}
				}
				if ( z % cs == 0 ) {
					if ( xdiff >= 0 && xdiff < xorder && zdiff-1 >= 0 && zdiff-1 < zorder ) {
						TC[xdiff,zdiff-1,k].SetHeight( x, z, altitude );
					}
				}

				// corner
				if ( ( x % cs == 0 ) && ( z % cs == 0 ) ) {
					if ( xdiff-1 >= 0 && xdiff-1 < xorder && zdiff-1 >= 0 && zdiff-1 < zorder ) {
						TC[xdiff-1,zdiff-1,k].SetHeight( x, z, altitude );
					}
				} else {
					if ( texture_id == -1 ) {
						// keep going
					} else {
						break;	// only corners matter for higher orders
					}
				}
			}
		}

		public void Add( Terrain t ) {
			Vector2D loc = new Vector2D( t.Position.X, t.Position.Z );
			if ( terrainPiecesXYIndex.Contains( loc ) ) {
				//Strive.Logging.Log.WarningMessage( "Replacing terrain peice " + tpexists.instance_id + " with " + tp.instance_id );
				terrainPiecesXYIndex.Remove( loc );
			}
			terrainPiecesXYIndex.Add( loc, t );

			Set( t.Position.X, t.Position.Z, t.Position.Y, _resource_manager.GetTexture(t.ResourceID).ID, t.Rotation.Y );
		}

		public void Clear() {
			/* no point dropping them, seeing we can reuse them right?
			int i,j,k;
			for (k=0; k<zoomorder; k++ ) {
				for (i=0; i<xorder; i++ ) {
					for (j=0; j<zorder; j++ ) {
						if ( TC[i,j,k] != null ) {
							TC[i,j,k].Delete();
							TC[i,j,k] = null;
						}
					}
				}
			}
			*/
			// TODO: could drop the reference too...
			terrainPiecesXYIndex.Clear();
		}

		public void Render() {
			int i, j, k, l, m;
			int cs;
			bool renderright;
			bool renderup;

			// Disable Z for the higher order landscapes, but for the best detail one,
			// re-enable Z (we want it on top, and to hide objects etc as normal
			_engine.DisableZ();
			for ( k=zoomorder-1; k>=0; k-- ) {
				if ( k==0 ) _engine.EnableZ();
				cs = (int)(ts*Math.Pow(hpc,k+1));

				if ( center_x - CX[k]*cs < ( CX[k]+xorder-1)*cs - center_x ) {
					renderright = true;
				} else {
					renderright = false;
				}
				if ( center_z - CZ[k]*cs < ( CZ[k]+zorder-1)*cs - center_z ) {
					renderup = true;
				} else {
					renderup = false;
				}

				for ( i=0; i<xorder; i++ ) {
					if ( renderright ) {
						l = i;
					} else {
						l = xorder-i-1;
					}
					for ( j=0; j<zorder; j++ ) {
						if ( renderup ) {
							m = i;
						} else {
							m = zorder-j-1;
						}
						if ( TC[l,m,k] != null && TC[l,m,k].Visible ) {
							TC[l,m,k].Render();
						}
					}

				}
			}
		}

		public class InvalidLocationException : Exception {}
		public float AltitudeAt( float x, float z ) {
			int cx = Helper.DivTruncate( (int)x, ts );
			int cz = Helper.DivTruncate( (int)z, ts );
			int cs;
			int xdiff, zdiff;
			for ( int k=0; k<zoomorder; k++ ) {
				cs = (int)(ts*Math.Pow(hpc,k+1));
				cx = Helper.DivTruncate( cx, hpc );
				cz = Helper.DivTruncate( cz, hpc );
				xdiff = cx - CX[k];
				zdiff = cz - CZ[k];
				if ( xdiff < 0 || xdiff >= xorder || zdiff < 0 || zdiff >= zorder ) {
					continue;
				}
				// TODO: fix!
				if ( TC[xdiff,zdiff,k] == null ) continue;
				return TC[xdiff,zdiff,k].GetHeight( x, z );
			}
			//TODO: fix!
			//throw new InvalidLocationException();
			return 0;
		}
	}
}
