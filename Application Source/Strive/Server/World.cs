using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections;
using System.Threading;
using Strive.Network.Server;
using Strive.Multiverse;

namespace Strive.Server {
	public class World {
		bool isRunning = false;

		double highX = 100000.0;
		double highZ = 100000.0;
		double lowX = -100000.0;
		double lowZ = -100000.0;
		int squaresInX = 1;
		int squaresInZ = 1;
		int squareSize = 10000;
		
		// the multiverse schema is used for dataset access
		Schema multiverse;
		// squares are used to group physical objects
		protected Square[,] squares;
		// all physical objects are indexed in a hashtable
		protected Hashtable physicalObjects = new Hashtable();

		public World() {
			System.Console.WriteLine( "Loading world..." );
			multiverse = Data.MultiverseFactory.loadMultiverse();

			// find highX and lowX for our world dimensions
			highX = ((Schema.RespawnPointRow)multiverse.RespawnPoint.Select( "X = max(X)" )[0]).X;
			lowX = ((Schema.RespawnPointRow)multiverse.RespawnPoint.Select( "X = min(X)" )[0]).X;
			highZ = ((Schema.RespawnPointRow)multiverse.RespawnPoint.Select( "Z = max(Z)" )[0]).Z;
			lowZ = ((Schema.RespawnPointRow)multiverse.RespawnPoint.Select( "Z = min(Z)" )[0]).Z;

			// figure out how many squares we need
			squaresInX = (int)(highX-lowX)/squareSize + 1;
			squaresInZ = (int)(highZ-lowZ)/squareSize + 1;

			// allocate the grid of squares used for grouping
			// physical objects that are close to each other
			squares = new Square[squaresInX,squaresInZ];
			for ( int i=0; i<squaresInX; i++ ) {
				for ( int j=0; j<squaresInZ; j++ ) {
					squares[i,j] = new Square();
				}
			}

			// for each respawn, create the wrapper and add it to the world
			foreach ( Schema.RespawnPointRow rpr in multiverse.RespawnPoint ) {
				// Load the underlying physical object,
				// AreaID 0 means it is a Player character or item
				Schema.PhysicalObjectRow por = multiverse.PhysicalObject.FindByPhysicalObjectID( rpr.PhysicalObjectID );
				if ( por.AreaID == 0 ) {
					// player objects/items are not loaded until needed
					System.Console.WriteLine( "Player " + rpr.SpawnID + " not loaded" );
					continue;
				}

				// For each respawn, try to figure out what it is
				Schema.MobilePhysicalObjectRow mr = multiverse.MobilePhysicalObject.FindByMobileID( rpr.PhysicalObjectID );
				Schema.QuaffableItemRow qr = multiverse.QuaffableItem.FindByItemID( rpr.PhysicalObjectID );
				Schema.EquipableItemRow er = multiverse.EquipableItem.FindByItemID( rpr.PhysicalObjectID );
				Schema.ReadableItemRow rr = multiverse.ReadableItem.FindByItemID( rpr.PhysicalObjectID );
				Schema.JunkItemRow jr = multiverse.JunkItem.FindByItemID( rpr.PhysicalObjectID );
				Schema.WieldableItemRow wr = multiverse.WieldableItem.FindByItemID( rpr.PhysicalObjectID );
				Schema.TerrainPhysicalObjectRow tr = multiverse.TerrainPhysicalObject.FindByTerrainID( rpr.PhysicalObjectID );
				
				if ( mr != null ) {
					Avatar a = new Avatar(
						mr,
						por,
						rpr	);
					// NB: we only add avatars to our world, not mobiles
					Add( a );
				} else if ( qr != null ) {
					Quaffable q = new Quaffable(
						qr,
						multiverse.ItemPhysicalObject.FindByItemID( qr.ItemID ),
						por,
						rpr );
					Add( q );
				} else if ( er != null ) {
					Equipable e = new Equipable(
						er,
						multiverse.ItemPhysicalObject.FindByItemID( er.ItemID ),
						por,
						rpr );
					Add( e );
				} else if ( rr != null ) {
					Readable r = new Readable(
						rr,
						multiverse.ItemPhysicalObject.FindByItemID( rr.ItemID ),
						por,
						rpr );
					Add( r );
				} else if ( wr != null ) {
					Wieldable w = new Wieldable(
						wr,
						multiverse.ItemPhysicalObject.FindByItemID( wr.ItemID ),
						por,
						rpr );
					Add( w );
				} else if ( jr != null ) {
					Junk j = new Junk(
						jr,
						multiverse.ItemPhysicalObject.FindByItemID( jr.ItemID ),
						por,
						rpr );
					Add( j );
				} else if ( tr != null ) {
					Terrain t = new Terrain(
						tr,
						por,
						rpr );
					Add( t );
				} else {
					System.Console.WriteLine( "ERROR: respawn of non-entity " + rpr.PhysicalObjectID ) ;
				}
			}
			System.Console.WriteLine( "Loaded world" );
		}

		public void Start() {
			isRunning = true;
			Thread myThread = new Thread(
				new ThreadStart( Handle )
				);
			myThread.Start();
		}

		public void Stop() {
			isRunning = false;
		}

		private void Handle() {
			while ( isRunning ) {
				// update objects
				foreach ( PhysicalObject po in physicalObjects ) {
					//PhysicalObjectUpdate( po );
				}
			}
		}

		public void Add( PhysicalObject po ) {
			if (
				po.respawnPoint.X > highX || po.respawnPoint.Z > highZ
				|| po.respawnPoint.X < lowX || po.respawnPoint.Z < lowZ
			) {
				System.Console.WriteLine( "ERROR: tried to add physical object outside the world" );
				return;
			}
			int squareX = (int)(po.respawnPoint.X-lowX)/squareSize;
			int squareZ = (int)(po.respawnPoint.Z-lowZ)/squareSize;
			int i, j;

			// if a new client has entered the world,
			// notify them about surrounding physical objects
			if ( po is Avatar ) {
				Client client = ((Avatar)po).client;
				if ( client != null ) {
					ArrayList nearbyPhysicalObjects = new ArrayList();
					ArrayList nearbyClients = new ArrayList();
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
					}
				}
			}

			// notify all nearby clients that a new
			// physical object has entered the world
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
					squares[squareX+i, squareZ+j].NotifyClientsAdd( po );
				}
			}

			// actually add the object to the world
			physicalObjects.Add( po.respawnPoint.SpawnID, po );
			squares[squareX,squareZ].Add( po );
			System.Console.WriteLine( "Added new " + po.GetType() + " " + po.respawnPoint.SpawnID + " to the world at (" + po.respawnPoint.X + "," + po.respawnPoint.Y + "," +po.respawnPoint.Z + ")" );
		}

		public void Move( PhysicalObject po, float x, float y, float z ) {
			int fromSquareX = (int)po.respawnPoint.X/squareSize;
			int fromSquareZ = (int)po.respawnPoint.Z/squareSize;
			int toSquareX = (int)x/squareSize;
			int toSquareZ = (int)z/squareSize;
			int i, j;
			Strive.Network.Messages.ToClient.Position message = new Strive.Network.Messages.ToClient.Position( po );

			for ( i=-1; i<=1; i++ ) {
				for ( j=-1; j<=1; j++ ) {
					if (
						Math.Abs(fromSquareX+i - toSquareX) > 1
						&& Math.Abs(fromSquareZ+j - toSquareZ) > 1
					) {
						if (
							fromSquareX+i >= 0 && fromSquareX+i < squaresInX
							&& fromSquareZ+j >= 0 && fromSquareZ+j < squaresInZ
						) {
							squares[fromSquareX+i, fromSquareZ+j].NotifyClientsRemove( po );
						}
						if (
							toSquareX+i >= 0 && toSquareX+i < squaresInX
							&& toSquareZ+j >= 0 && toSquareZ+j < squaresInZ
						) {
							squares[toSquareX-i, toSquareZ-j].NotifyClientsAdd( po );
						}
					}
					if (
						toSquareX+i >= 0 && toSquareX+i < squaresInX
						&& toSquareZ+j >= 0 && toSquareZ+j < squaresInZ
						) {
						squares[toSquareX-i, toSquareZ-j].NotifyClientsMessage( message );
					}
				}
			}
			if ( fromSquareX != toSquareX || fromSquareZ != toSquareZ ) {
				squares[fromSquareX,fromSquareZ].Remove( po );
				squares[toSquareX,toSquareZ].Add( po );
			}
			po.respawnPoint.X = x;
			po.respawnPoint.Y = y;
			po.respawnPoint.Z = z;
		}

		public Mobile LoadMobile( int spawnID ) {
			Schema.RespawnPointRow rpr = (Schema.RespawnPointRow)multiverse.RespawnPoint.FindBySpawnID( spawnID );
			if ( rpr == null ) return null;
			Schema.PhysicalObjectRow por = multiverse.PhysicalObject.FindByPhysicalObjectID( rpr.PhysicalObjectID );
			if ( por == null ) return null;
			Schema.MobilePhysicalObjectRow mr = multiverse.MobilePhysicalObject.FindByMobileID( rpr.PhysicalObjectID );
			if ( mr == null ) return null;
			Mobile mobile = new Mobile( mr, por, rpr );
			return mobile;
		}

		public bool UserLookup( string email, string password ) {
			DataRow[] dr = multiverse.Player.Select( "Email = '" + email + "'" );
			if ( dr.Length != 1 ) {
				System.Console.WriteLine( "ERROR: " + dr.Length + " players found with email '" + email + "'" );
				return false;
			} else {
				if ( String.Compare( (string)dr[0]["password"], password ) == 0 ) {
					return true;
				} else {
					System.Console.WriteLine( "ERROR: incorrect password for player with email '" + email + "'" );
					return false;
				}
			}
		}
	}
}
