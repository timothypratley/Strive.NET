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
		protected ArrayList mobilesArrayList = new ArrayList();

		public World() {
			System.Console.WriteLine( "Loading world..." );
			multiverse = Data.MultiverseFactory.loadMultiverse();

			// find highX and lowX for our world dimensions
			highX = ((Schema.ObjectInstanceRow)multiverse.ObjectInstance.Select( "X = max(X)" )[0]).X;
			lowX = ((Schema.ObjectInstanceRow)multiverse.ObjectInstance.Select( "X = min(X)" )[0]).X;
			highZ = ((Schema.ObjectInstanceRow)multiverse.ObjectInstance.Select( "Z = max(Z)" )[0]).Z;
			lowZ = ((Schema.ObjectInstanceRow)multiverse.ObjectInstance.Select( "Z = min(Z)" )[0]).Z;

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
			foreach ( Schema.ObjectInstanceRow rpr in multiverse.ObjectInstance ) {
				// Load the underlying physical object,
				// AreaID 0 means it is a Player character or item
				Schema.ObjectTemplateRow por = multiverse.ObjectTemplate.FindByObjectTemplateID( rpr.ObjectTemplateID );
				if ( por.AreaID == 0 ) {
					// player objects/items are not loaded until needed
					System.Console.WriteLine( "Player " + rpr.ObjectInstanceID + " not loaded" );
					continue;
				}

				// For each respawn, try to figure out what it is
				Schema.TemplateMobileRow mr = multiverse.TemplateMobile.FindByObjectTemplateID( rpr.ObjectTemplateID );
				Schema.ItemQuaffableRow qr = multiverse.ItemQuaffable.FindByObjectTemplateID( rpr.ObjectTemplateID );
				Schema.ItemEquipableRow er = multiverse.ItemEquipable.FindByObjectTemplateID( rpr.ObjectTemplateID );
				Schema.ItemReadableRow rr = multiverse.ItemReadable.FindByObjectTemplateID( rpr.ObjectTemplateID );
				Schema.ItemJunkRow jr = multiverse.ItemJunk.FindByObjectTemplateID( rpr.ObjectTemplateID );
				Schema.ItemWieldableRow wr = multiverse.ItemWieldable.FindByObjectTemplateID( rpr.ObjectTemplateID );
				Schema.TemplateTerrainRow tr = multiverse.TemplateTerrain.FindByObjectTemplateID( rpr.ObjectTemplateID );
				
				if ( mr != null ) {
					MobileAvatar a = new MobileAvatar(
						mr,
						por,
						rpr	);
					// NB: we only add avatars to our world, not mobiles
					Add( a );
				} else if ( qr != null ) {
					Quaffable q = new Quaffable(
						qr,
						multiverse.TemplateItem.FindByObjectTemplateID( qr.ObjectTemplateID ),
						por,
						rpr );
					Add( q );
				} else if ( er != null ) {
					Equipable e = new Equipable(
						er,
						multiverse.TemplateItem.FindByObjectTemplateID( er.ObjectTemplateID ),
						por,
						rpr );
					Add( e );
				} else if ( rr != null ) {
					Readable r = new Readable(
						rr,
						multiverse.TemplateItem.FindByObjectTemplateID( rr.ObjectTemplateID ),
						por,
						rpr );
					Add( r );
				} else if ( wr != null ) {
					Wieldable w = new Wieldable(
						wr,
						multiverse.TemplateItem.FindByObjectTemplateID( wr.ObjectTemplateID ),
						por,
						rpr );
					Add( w );
				} else if ( jr != null ) {
					Junk j = new Junk(
						jr,
						multiverse.TemplateItem.FindByObjectTemplateID( jr.ObjectTemplateID ),
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
					System.Console.WriteLine( "ERROR: respawn of non-entity " + rpr.ObjectTemplateID ) ;
				}
			}
			System.Console.WriteLine( "Loaded world" );
		}

		public void Update() {
			foreach ( MobileAvatar mob in physicalObjects ) {
				mob.Update();
			}
		}

		public void Add( PhysicalObject po ) {
			if (
				po.X > highX || po.Z > highZ
				|| po.X < lowX || po.Z < lowZ
			) {
				System.Console.WriteLine( "ERROR: tried to add physical object outside the world" );
				return;
			}
			int squareX = (int)(po.X-lowX)/squareSize;
			int squareZ = (int)(po.Z-lowZ)/squareSize;
			int i, j;

			// if a new client has entered the world,
			// notify them about surrounding physical objects
			if ( po is MobileAvatar ) {
				Client client = ((MobileAvatar)po).client;
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
			physicalObjects.Add( po.ObjectInstanceID, po );
			if ( po is Mobile ) {
				mobilesArrayList.Add( po );
			}
			squares[squareX,squareZ].Add( po );
			System.Console.WriteLine( "Added new " + po.GetType() + " " + po.ObjectInstanceID + " to the world at (" + po.X + "," + po.Y + "," +po.Z + ")" );
		}

		public void Move( PhysicalObject po, float x, float y, float z ) {
			int fromSquareX = (int)po.X/squareSize;
			int fromSquareZ = (int)po.Z/squareSize;
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
			po.X = x;
			po.Y = y;
			po.Z = z;
		}

		public MobileAvatar LoadMobile( int instanceID ) {
			Schema.ObjectInstanceRow rpr = (Schema.ObjectInstanceRow)multiverse.ObjectInstance.FindByObjectInstanceID( instanceID );
			if ( rpr == null ) return null;
			Schema.ObjectTemplateRow por = multiverse.ObjectTemplate.FindByObjectTemplateID( rpr.ObjectTemplateID );
			if ( por == null ) return null;
			Schema.TemplateMobileRow mr = multiverse.TemplateMobile.FindByObjectTemplateID( rpr.ObjectTemplateID );
			if ( mr == null ) return null;
			return new MobileAvatar( mr, por, rpr );
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
