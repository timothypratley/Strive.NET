using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections;
using System.Threading;

using Strive.Network.Server;
using Strive.Network.Messages;
using Strive.Multiverse;
using Strive.Math3D;
using Strive.Data;
using Strive.Logging;

// todo: this object needs to be made threadsafe

namespace Strive.Server.Shared {
	public class World {
		double highX = 500.0;
		double highZ = 500.0;
		double lowX = -500.0;
		double lowZ = -500.0;
		int squaresInX;		// see squareSize in Square
		int squaresInZ;
		int world_id;
		
		// squares are used to group physical objects
		protected Square[,] squares;
		// all physical objects are indexed in a hashtable
		public Hashtable physicalObjects;
		protected ArrayList mobilesArrayList;

		protected Strive.Network.Messages.ToClient.Weather weather = new Strive.Network.Messages.ToClient.Weather( 1, 0, 0 );

		public World( int world_id ) {
			this.world_id = world_id;
			Load();
		}

		public void Load() {
			physicalObjects = new Hashtable();
			mobilesArrayList = new ArrayList();

			// todo: would be nice to be able to load only the
			// world in question... but for now load them all
			Log.LogMessage( "Loading Global.multiverse..." );
			Global.multiverse = Strive.Data.MultiverseFactory.getMultiverse();
			Log.LogMessage( "Global.multiverse loaded." );

			// find highX and lowX for our world dimensions
			highX = ((Schema.ObjectInstanceRow)Global.multiverse.ObjectInstance.Select( "X = max(X)" )[0]).X;
			lowX = ((Schema.ObjectInstanceRow)Global.multiverse.ObjectInstance.Select( "X = min(X)" )[0]).X;
			highZ = ((Schema.ObjectInstanceRow)Global.multiverse.ObjectInstance.Select( "Z = max(Z)" )[0]).Z;
			lowZ = ((Schema.ObjectInstanceRow)Global.multiverse.ObjectInstance.Select( "Z = min(Z)" )[0]).Z;
			Log.LogMessage( "Global.multiverse bounds are " + lowX + "," + lowZ + " " + highX + "," + highZ );

			// figure out how many squares we need
			squaresInX = (int)(highX-lowX)/Square.squareSize + 1;
			squaresInZ = (int)(highZ-lowZ)/Square.squareSize + 1;

			if ( squaresInX * squaresInZ > 10000 ) {
				throw new Exception( "World is too big. Total area must not exceed " + 10000*Square.squareSize + ". Please fix the database." );
			}

			// allocate the grid of squares used for grouping
			// physical objects that are close to each other
			squares = new Square[squaresInX,squaresInZ];
			for ( int i=0; i<squaresInX; i++ ) {
				for ( int j=0; j<squaresInZ; j++ ) {
					squares[i,j] = new Square();
				}
			}

			Schema.WorldRow wr = Global.multiverse.World.FindByWorldID( world_id );
			if ( wr == null ) {
				throw new Exception( "ERROR: World ID not valid!" );	
			}
			
			Log.LogMessage( "Loading world \"" + wr.WorldName + "\"..." );
			foreach ( Schema.AreaRow ar in wr.GetAreaRows() ) {
				Log.LogMessage( "Loading area \"" + ar.AreaName + "\"..." );
				// don't load area 0, its players and their eq
				if ( ar.AreaID == 0 ) continue;
				foreach ( Schema.ObjectTemplateRow otr in ar.GetObjectTemplateRows() ) {
					foreach ( Schema.TemplateMobileRow tmr in otr.GetTemplateMobileRows() ) {
						foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
							// NB: we only add avatars to our world, not mobiles
							MobileAvatar a = new MobileAvatar( this, tmr, otr, oir );
							Add( a );
						}
					}
					foreach ( Schema.TemplateItemRow tir in otr.GetTemplateItemRows() ) {
						foreach ( Schema.ItemEquipableRow ier in tir.GetItemEquipableRows() ) {
							foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
								Equipable e = new Equipable( ier, tir, otr, oir );
								Add( e );
							}
						}
						foreach ( Schema.ItemJunkRow ijr in tir.GetItemJunkRows() ) {
							foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
								Junk j = new Junk( ijr, tir, otr, oir );
								Add( j );
							}
						}
						foreach ( Schema.ItemQuaffableRow iqr in tir.GetItemQuaffableRows() ) {
							foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
								Quaffable q = new Quaffable( iqr, tir, otr, oir );
								Add( q );
							}
						}
						foreach ( Schema.ItemReadableRow irr in tir.GetItemReadableRows() ) {
							foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
								Readable r = new Readable( irr, tir, otr, oir );
								Add( r );
							}
						}
						foreach ( Schema.ItemWieldableRow iwr in tir.GetItemWieldableRows() ) {
							foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
								Wieldable w = new Wieldable( iwr, tir, otr, oir );
								Add( w );
							}

						}
					}
					foreach ( Schema.TemplateTerrainRow ttr in otr.GetTemplateTerrainRows() ) {
						foreach ( Schema.ObjectInstanceRow oir in otr.GetObjectInstanceRows() ) {
							Terrain t = new Terrain( ttr, otr, oir );
							Add( t );
						}
					}
				}
			}
			Log.LogMessage( "Loaded world." );

			// Calculate Terrain heightmaps
			for ( int i=0; i<squaresInX; i++ ) {
				for ( int j=0; j<squaresInZ; j++ ) {
					// EEERRR not working just yet
					//squares[i,j].CalculateHeightMap();
				}
			}
		}

		public void Update() {
			foreach ( PhysicalObject po in physicalObjects.Values ) {
				if ( po is MobileAvatar ) {
					(po as MobileAvatar).Update();
				}
			}
			WeatherUpdate();
		}

		void WeatherUpdate() {
			bool weatherChanged = false;
			if ( Global.random.NextDouble() > 0.999 ) {
				weather.Fog++;
				weatherChanged = true;
			}
			if ( Global.random.NextDouble() > 0.999 ) {
				weather.Lighting++;
				weatherChanged = true;
			}
			if ( Global.random.NextDouble() > 0.995 ) {
				weather.SkyTextureID = (weather.SkyTextureID + 1) % 9 + 1;
				weatherChanged = true;
			}
			if ( weatherChanged ) {
				NotifyMobiles( weather );
			}
		}

		public void NotifyMobiles( Strive.Network.Messages.IMessage message ) {
			foreach ( MobileAvatar ma in mobilesArrayList ) {
				if ( ma.client != null ) {
					ma.client.Send( message );
				}
			}
		}

		public void Add( PhysicalObject po ) {
			if (
				po.Position.X > highX || po.Position.Z > highZ
				|| po.Position.X < lowX || po.Position.Z < lowZ
			) {
				Log.ErrorMessage( "Tried to add physical object outside the world." );
				return;
			}

			// add the object to the world
			physicalObjects.Add( po.ObjectInstanceID, po );
			if ( po is Mobile ) {
				mobilesArrayList.Add( po );
			}
			int squareX = (int)(po.Position.X-lowX)/Square.squareSize;
			int squareZ = (int)(po.Position.Z-lowZ)/Square.squareSize;
			squares[squareX,squareZ].Add( po );

			// notify all nearby clients that a new
			// physical object has entered the world
			InformNearby( po, new Strive.Network.Messages.ToClient.AddPhysicalObject( po ) );
			if ( po is Mobile ) {
				InformNearby( po, new Strive.Network.Messages.ToClient.MobileState( (Mobile)po )	);
			}


			Log.LogMessage( "Added new " + po.GetType() + " " + po.ObjectInstanceID + " at (" + po.Position.X + "," + po.Position.Y + "," +po.Position.Z + ") - ("+squareX+","+squareZ+")" );
		}

		public void Remove( PhysicalObject po ) {
			int squareX = (int)(po.Position.X-lowX)/Square.squareSize;
			int squareZ = (int)(po.Position.Z-lowZ)/Square.squareSize;
			InformNearby( po, new Strive.Network.Messages.ToClient.DropPhysicalObject( po ) );
			squares[squareX,squareZ].Remove( po );
			physicalObjects.Remove( po.ObjectInstanceID );
			Log.LogMessage( "Removed " + po.GetType() + " " + po.ObjectInstanceID + " from the world." );
		}

		public void Relocate( PhysicalObject po, Vector3D newPos, Vector3D newHeading ) {
			if ( newPos.X > highX ) {
				newPos.X = (float)highX;
			}
			if ( newPos.Z > highZ ) {
				newPos.Z = (float)highZ;
			}
			if ( newPos.X < lowX ) {
				newPos.X = (float)lowX;
			}
			if ( newPos.Z < lowZ ) {
				newPos.Z = (float)lowZ;
			}
			int fromSquareX = (int)(po.Position.X - lowX)/Square.squareSize;
			int fromSquareZ = (int)(po.Position.Z - lowZ)/Square.squareSize;
			int toSquareX = (int)(newPos.X - lowX)/Square.squareSize;
			int toSquareZ = (int)(newPos.Z - lowZ)/Square.squareSize;
			int i, j;

			MobileAvatar ma;
			if ( po is MobileAvatar ) {
				ma = (MobileAvatar)po;
			} else {
				ma = null;
			}

			// check that the object can fit there
			foreach ( PhysicalObject spo in squares[toSquareX,toSquareZ].physicalObjects ) {
				// ignoring terrain for now
				if ( spo is Terrain || spo == po ) continue;
				// distance between two objects in 2d space
				// todo, convert to 3d space when object centers is sorted
				float dx = newPos.X - spo.Position.X;
				float dz = newPos.Z - spo.Position.Z;
				//float dy = newPos.Y - spo.Position.Y;
				float distance_squared = dx*dx + dz*dz; // + dy*dy;
				if ( distance_squared <= spo.BoundingSphereRadiusSquared + po.BoundingSphereRadiusSquared ) {
					// Log.LogMessage( "Collision " + po.ObjectInstanceID + " with " + spo.ObjectInstanceID + "." );
					// objects would be touching, reject the move
					// if its a player, slap their wrist with a position update
					if ( ma != null ) {
						// only if not already collided!
						float dx1 = ma.Position.X - spo.Position.X;
						float dz1 = ma.Position.Z - spo.Position.Z;
						//float dy1 = ma.Position.Y - spo.Position.Y;
						float distance_squared1 = dx1*dx1 + dz1*dz1;// + dy1*dy1;
						if ( distance_squared <= spo.BoundingSphereRadiusSquared + po.BoundingSphereRadiusSquared ) {
							return;
						}
						if ( ma.client != null ) {
							ma.client.Send(
								new Strive.Network.Messages.ToClient.Position( ma ) );
						}
					}
					return;
				}
			}

			po.Position.X = newPos.X;
			po.Position.Y = newPos.Y;
			po.Position.Z = newPos.Z;
			po.Heading.X = newHeading.X;
			po.Heading.Y = newHeading.Y;
			po.Heading.Z = newHeading.Z;

			for ( i=-1; i<=1; i++ ) {
				for ( j=-1; j<=1; j++ ) {
					if (
						Math.Abs(fromSquareX+i - toSquareX) > 1
						|| Math.Abs(fromSquareZ+j - toSquareZ) > 1
					) {
						// squares which need to have their clients
						// add or remove the object
						// as the jump has brought the object in or out of focus

						// remove from
						if (
							// check the square exists
							fromSquareX+i >= 0 && fromSquareX+i < squaresInX
							&& fromSquareZ+j >= 0 && fromSquareZ+j < squaresInZ
						) {
							squares[fromSquareX+i, fromSquareZ+j].NotifyClients(
								new Strive.Network.Messages.ToClient.DropPhysicalObject( po ) );
							// if the object is a mobile, it needs to be made aware
							// of its new world view
							if ( ma != null && ma.client != null ) {
								foreach( PhysicalObject toDrop in squares[fromSquareX+i, fromSquareZ+j].physicalObjects ) {
									ma.client.Send(
										new Strive.Network.Messages.ToClient.DropPhysicalObject( toDrop ) );
									//Log.LogMessage( "Told client to drop " + toDrop.ObjectInstanceID + "." );
								}
							}
						}

						// add to
						if (
							// check the square exists
							toSquareX-i >= 0 && toSquareX-i < squaresInX
							&& toSquareZ-j >= 0 && toSquareZ-j < squaresInZ
						) {
							squares[toSquareX-i, toSquareZ-j].NotifyClients(
								new Strive.Network.Messages.ToClient.AddPhysicalObject( po ) );
							// if the object is a player, it needs to be made aware
							// of its new world view
							if ( ma != null && ma.client != null ) {
								foreach( PhysicalObject toAdd in squares[toSquareX-i, toSquareZ-j].physicalObjects ) {
									ma.client.Send(
										new Strive.Network.Messages.ToClient.AddPhysicalObject( toAdd ) );
									//Log.LogMessage( "Told client to add " + toAdd.ObjectInstanceID + "." );
								}
							}
						}
					} else {
						// clients that have the object already in scope need to be
						// told its new position
						if (
							// check the square exists
							toSquareX+i >= 0 && toSquareX+i < squaresInX
							&& toSquareZ+j >= 0 && toSquareZ+j < squaresInZ
						) {
							if ( ma != null && ma.client != null ) {
								squares[toSquareX+i, toSquareZ+j].NotifyClientsExcept(
									new Strive.Network.Messages.ToClient.Position( po ),
									ma.client );
							} else {
								squares[toSquareX+i, toSquareZ+j].NotifyClients(
									new Strive.Network.Messages.ToClient.Position( po ) );
							}
						}
					}
				}
			}

			// transition the object to its new square if it changed squares
			if ( fromSquareX != toSquareX || fromSquareZ != toSquareZ ) {
				squares[fromSquareX,fromSquareZ].Remove( po );
				squares[toSquareX,toSquareZ].Add( po );
			}
		}

		public MobileAvatar LoadMobile( int instanceID ) {
			Schema.ObjectInstanceRow rpr = (Schema.ObjectInstanceRow)Global.multiverse.ObjectInstance.FindByObjectInstanceID( instanceID );
			if ( rpr == null ) return null;
			Schema.ObjectTemplateRow por = Global.multiverse.ObjectTemplate.FindByObjectTemplateID( rpr.ObjectTemplateID );
			if ( por == null ) return null;
			Schema.TemplateMobileRow mr = Global.multiverse.TemplateMobile.FindByObjectTemplateID( rpr.ObjectTemplateID );
			if ( mr == null ) return null;
			return new MobileAvatar( this, mr, por, rpr );
		}

		public bool UserLookup( string email, string password, ref int playerID ) {
			Strive.Data.MultiverseFactory.refreshPlayerList( Global.multiverse );
			DataRow[] dr = Global.multiverse.Player.Select( "Email = '" + email + "'" );
			if ( dr.Length != 1 ) {
				Log.ErrorMessage( dr.Length + " players found with email '" + email + "'." );
				return false;
			} else {
				if ( String.Compare( (string)dr[0]["Password"], password ) == 0 ) {
					playerID = (int)dr[0]["PlayerID"];
					return true;
				} else {
					Log.LogMessage( "Incorrect password for player with email '" + email + "'." );
					return false;
				}
			}
		}

		public void InformNearby( PhysicalObject po, IMessage message ) {
			// notify all nearby clients
			int squareX = (int)(po.Position.X-lowX)/Square.squareSize;
			int squareZ = (int)(po.Position.Z-lowZ)/Square.squareSize;
			int i, j;
			for ( i=-1; i<=1; i++ ) {
				for ( j=-1; j<=1; j++ ) {
					// check that neigbour exists
					if (
						squareX+i < 0 || squareX+i >= squaresInX
						|| squareZ+j < 0 || squareZ+j >= squaresInZ
					) {
						continue;
					}
					// need to send a message to all nearby clients
					squares[squareX+i, squareZ+j].NotifyClients( message );
				}
			}
		}

		public void SendInitialWorldView( Client client ) {
			// if a new client has entered the world,
			// notify them about surrounding physical objects
			// NB: this routine will send the client mobile's
			// position as one of the 'nearby' mobiles.
			Mobile mob = client.Avatar;
			client.Send( weather );
			int squareX = (int)(mob.Position.X-lowX)/Square.squareSize;
			int squareZ = (int)(mob.Position.Z-lowZ)/Square.squareSize;
			int i, j;
			if ( client != null ) {
				ArrayList nearbyPhysicalObjects = new ArrayList();
				for ( i=-1; i<=1; i++ ) {
					for ( j=-1; j<=1; j++ ) {
						// check that neigbour exists
						if (
							squareX+i < 0 || squareX+i >= squaresInX
							|| squareZ+j < 0 || squareZ+j >= squaresInZ
						) {
							continue;
						}
						// add all neighbouring physical objects
						// to the clients world view
						foreach ( PhysicalObject p in squares[squareX+i,squareZ+j].physicalObjects ) {
							nearbyPhysicalObjects.Add( p );
						}
					}
				}
				/*
				Strive.Network.Messages.ToClient.AddPhysicalObjects message = new Strive.Network.Messages.ToClient.AddPhysicalObjects(
					nearbyPhysicalObjects
				);
				client.Send( message );
				*/
				foreach ( PhysicalObject p in nearbyPhysicalObjects ) {
					Strive.Network.Messages.ToClient.AddPhysicalObject message = new Strive.Network.Messages.ToClient.AddPhysicalObject( p );
					client.Send( message );
					if ( p is Mobile ) {
						client.Send(
							new Strive.Network.Messages.ToClient.MobileState( (Mobile)p )
						);
					}
				}
			}
		}

		public Strive.Network.Messages.ToClient.CanPossess.id_name_tuple[] getPossessable( string username ) {
			DataRow[] dr = Global.multiverse.Player.Select( "Email = '" + username + "'" );
			Schema.PlayerRow pr = Global.multiverse.Player.FindByPlayerID( (int)dr[0][0] );
			Schema.MobilePossesableByPlayerRow [] mpbpr = pr.GetMobilePossesableByPlayerRows();
			ArrayList list = new ArrayList();
			foreach ( Schema.MobilePossesableByPlayerRow mpr in mpbpr ) {
				Strive.Network.Messages.ToClient.CanPossess.id_name_tuple tuple = new Strive.Network.Messages.ToClient.CanPossess.id_name_tuple(
					mpr.ObjectInstanceID, mpr.ObjectInstanceRow.ObjectTemplateRow.ObjectTemplateName
				);
				list.Add( tuple );
			}
			return (Strive.Network.Messages.ToClient.CanPossess.id_name_tuple [])list.ToArray( typeof( Strive.Network.Messages.ToClient.CanPossess.id_name_tuple ) );
		}
	}
}
