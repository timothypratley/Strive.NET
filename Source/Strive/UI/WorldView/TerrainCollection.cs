using System;
using System.Collections;

using Strive.Resources;
using Strive.Rendering;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
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

		public TerrainCollection( ResourceManager rm, IEngine engine, IScene scene ) {
			_engine = engine;
			_scene = scene;
			_resource_manager = rm;

			// now create chunks at the empty points
			int i, j, k, cs;
			for ( k=0; k<zoomorder; k++ ) {
				cs = (int)(ts*Math.Pow(hpc,k+1));
				CX[k] = 0;
				CZ[k] = 0;
				for ( i=0; i<xorder; i++ ) {
					for ( j=0; j<zorder; j++ ) {
						TC[i,j,k] = _engine.CreateTerrainChunk(i*cs, j*cs, cs/hpc, hpc);
						ITexture texture = engine.CreateTexture( "land", 256*hpc, 256*hpc );
						TC[i,j,k].SetTexture( texture );

						// TODO: fix this don't hardcode it... and don't set it per terrain chunk
						//if ( k == zoomorder-1 ){
							// TODO:
						//	TC[i,j,k].SetClouds(_resource_manager.GetTexture(8));
						//}
					}
				}
			}
		}

		public void Recenter( float x, float z ) {
			int i, j, k, cs, cx, cz;
			int xdiff, zdiff;
			float tcx, tcz, px, pz, altitude;
			int l, m;
			Vector2D loc = new Vector2D(0,0);
			Terrain t1, t2;

			// drop all terrain data that is now out of scope
			ArrayList arrayList = new ArrayList(terrainPiecesXYIndex.Keys);
			foreach ( Vector2D key in arrayList ) {
				for ( k=0; k<Constants.terrainZoomOrder; k++ ) {
					if (
						key.X > x+Constants.xRadius[k]*ts || key.Y > z+Constants.zRadius[k]*ts
						|| key.X < x-Constants.xRadius[k]*ts || key.Y < z-Constants.zRadius[k]*ts
					) {
						if (
							// there is no higher zoom order
							k == (Constants.terrainZoomOrder-1)
							// this is not a higher order point
							|| (key.X % (ts*Constants.scale[k+1])) != 0 || (key.Y % (ts*Constants.scale[k+1])) != 0
						) {
							// TODO: why doesn't this wurk??? umg
							terrainPiecesXYIndex.Remove( key );
							break;
						}
					} else {
						break;
					}
				}
			}

			// loop through all chunks to see if any can be reused
			for ( k=zoomorder-1; k>=0; k++ ) {
				cs = (int)(ts*Math.Pow(hpc,k+1));
				cx = Helper.DivTruncate( (int)x, cs );
				cz = Helper.DivTruncate( (int)z, cs );
				if ( x - (cx*cs) < (cx+1)*cs - x ) cx--;
				if ( z - (cz*cs) < (cz+1)*cs - z ) cz--;
				cx -= (xorder/2-1);
				cz -= (zorder/2-1);
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
							// Keep the data in TC[i+xdiff,j+zdiff,k]
							// but move it to TC[i,j,k]
							ITerrainChunk tc = TC[i,j,k];
							TC[i,j,k] = TC[i+xdiff,j+zdiff,k];
							TC[i+xdiff,j+zdiff,k] = tc;

							// update edges with real values
							// this is 'unseaming' from the higher order
							if ( (i+xdiff)==0 ) {
								for ( m=0; m<hpc; m++ ) {
									px = TC[i,j,k].Position.X;
									pz = TC[i,j,k].Position.Z + m*cs/hpc;
									loc.Set( px, pz );
									altitude = 0;
									t1 = (Terrain)terrainPiecesXYIndex[loc];
									if ( t1!=null ) {
										altitude = t1.Position.Y;
									}
									TC[i,j,k].SetHeight(px, pz, altitude);
								}
							} else if ( (i+xdiff)==(xorder-1) ) {
								for ( m=0; m<hpc; m++ ) {
									px = TC[i,j,k].Position.X + cs;
									pz = TC[i,j,k].Position.Z + m*cs/hpc;
									loc.Set( px, pz );
									altitude = 0;
									t1 = (Terrain)terrainPiecesXYIndex[loc];
									if ( t1!=null ) {
										altitude = t1.Position.Y;
									}
									TC[i,j,k].SetHeight(px, pz, altitude);
								}
							}
							if ( (j+zdiff)==0 ) {
								for ( l=0; l<hpc; l++ ) {
									px = TC[i,j,k].Position.X + l*cs/hpc;
									pz = TC[i,j,k].Position.Z;
									loc.Set( px, pz );
									altitude = 0;
									t1 = (Terrain)terrainPiecesXYIndex[loc];
									if ( t1!=null ) {
										altitude = t1.Position.Y;
									}
									TC[i,j,k].SetHeight(px, pz, altitude);
								}
							} else if ( (j+zdiff)==(zorder-1) ) {
								for ( l=0; l<hpc; l++ ) {
									px = TC[i,j,k].Position.X + l*cs/hpc;
									pz = TC[i,j,k].Position.Z + cs;
									loc.Set( px, pz );
									altitude = 0;
									t1 = (Terrain)terrainPiecesXYIndex[loc];
									if ( t1!=null ) {
										altitude = t1.Position.Y;
									}
									TC[i,j,k].SetHeight(px, pz, altitude);
								}
							}
						} else {
							// TC[i,j,k] is dirty and ready for
							// reuse... this means that its higher order peice
							// is going to become visible again (if it exists).
							MakeVisible( k+1, TC[i,j,k].Position.X, TC[i,j,k].Position.Z );

							// The new location needs to be made invisible
							TC[i,j,k].Position = new Vector3D(tcx, 0, tcz );
							MakeInvisible( k+1, TC[i,j,k].Position.X, TC[i,j,k].Position.Z );

							// update it with any known heights
							// Refresh( TC[i,j,k] );
							for (l=0; l<=hpc; l++ ) {
								for (m=0; m<=hpc; m++) {
									px = tcx + l*cs/hpc;
									pz = tcz + m*cs/hpc;
									altitude = 0;
									if ( l==0&&i==0 || l==(hpc-1)&&i==(xorder-1) ) {
										// edges need to be seamed with higher order edges
										loc.Set( tcx, tcz );
										t1 = (Terrain)terrainPiecesXYIndex[loc];
										loc.Set( tcx, tcz+cs );
										t2 = (Terrain)terrainPiecesXYIndex[loc];
										if ( t1!=null && t2!=null ) {
											altitude = t1.Position.Y + (t2.Position.Y-t1.Position.Y)*m/hpc;
										}
									} else if ( m==0&&j==0 || m==(hpc-1)&&j==(zorder-1) ) {
										// edges need to be seamed with higher order edges
										loc.Set( tcx, tcz );
										t1 = (Terrain)terrainPiecesXYIndex[loc];
										loc.Set( tcx+cs, tcz );
										t2 = (Terrain)terrainPiecesXYIndex[loc];
										if ( t1!=null && t2!=null ) {
											altitude = t1.Position.Y + (t2.Position.Y-t1.Position.Y)*m/hpc;
										}
									} else {
										loc.Set( px, pz );
										t1 = (Terrain)terrainPiecesXYIndex[loc];
										if ( t1!=null ) {
											altitude = t1.Position.Y;
										}
									}
									TC[i,j,k].SetHeight( px, pz, altitude );

									// regardless of altitude, always use the right texture
									loc.Set( px, pz );
									t1 = (Terrain)terrainPiecesXYIndex[loc];
									if ( t1 != null ) {
										//if ( k==0 )
										TC[i,j,k].DrawTexture( _resource_manager.GetTexture( t1.ResourceID ), px, pz, t1.Rotation.Y );
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
		}

		public void AddMany( float start_x, float start_z, int width, int height, int gap_size, Terrain [,] map ) {
			foreach ( Terrain t in map ) {
				Add( t );
			}
		}

		void MakeVisible( int k, float x, float z ) {
			int cs = (int)(ts*Math.Pow(hpc,k+1));
			int i = Helper.DivTruncate( (int)x, cs ) - CX[k];
			int j = Helper.DivTruncate( (int)z, cs ) - CZ[k];
			Vector2D loc = new Vector2D( x, z );
			Terrain t = (Terrain)terrainPiecesXYIndex[loc];
			if ( t!=null ) {
				TC[i,j,k].DrawTexture( _resource_manager.GetTexture( t.ResourceID ), x, z, t.Rotation.Y );
			}
		}
		
		void MakeInvisible( int k, float x, float z ) {
			int cs = (int)(ts*Math.Pow(hpc,k+1));
			int i = Helper.DivTruncate( (int)x, cs ) - CX[k];
			int j = Helper.DivTruncate( (int)z, cs ) - CZ[k];
			// TODO: need a default 'invis' texture
			TC[i,j,k].Clear( x, z, 256, 256 );
		}

		public void Set( float x, float z, float altitude, ITexture texture, float rotation ) {
			int cs = ts;
			int cx = Helper.DivTruncate( (int)x, ts );
			int cz = Helper.DivTruncate( (int)z, ts );
			int xdiff, zdiff;
			int k;

			if ( texture.ID == -1 ) {
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
				if ( texture.ID == -1 ) {
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

					// set the texture for higher order terrain or not
					//if ( k==0 )
					TC[xdiff,zdiff,k].DrawTexture( texture, x, z, rotation );
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
					if ( texture.ID == -1 ) {
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

			Set( t.Position.X, t.Position.Z, t.Position.Y, _resource_manager.GetTexture(t.ResourceID), t.Rotation.Y );
		}

		public void Clear() {
			// TODO: could clear the underlying terrain chunks if we wanted to
			terrainPiecesXYIndex.Clear();
		}

		public void Render() {
			int i, j, k;
			for ( k=zoomorder-1; k>=0; k-- ) {
				for ( i=0; i<xorder; i++ ) {
					for ( j=0; j<zorder; j++ ) {
						if ( TC[i,j,k] != null && TC[i,j,k].Visible ) {
							TC[i,j,k].Render();
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
