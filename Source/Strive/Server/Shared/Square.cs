using System;
using System.Collections;
using Strive.Multiverse;
using Strive.Network.Server;
using Strive.Network.Messages;
//using Strive.Rendering;
//using Strive.Rendering.Models;
//using Strive.Resources;


namespace Strive.Server.Shared
{
	/// <summary>
	/// A square encompasses all the physical objects and clients
	/// in a discreet area.
	/// The World is split into Squares so that nearby objects and clients
	/// may be referenced with ease.
	/// Squares are used to define a clients world view,
	/// as only neigbouring physical objects affect the client.
	/// </summary>
	public class Square
	{
		public static int squareSize = 200;
		public static int terrainSize = 100;
		public ArrayList physicalObjects = new ArrayList();
		public ArrayList clients = new ArrayList();
		public float[,] heightMap = new float[squareSize/terrainSize,squareSize/terrainSize];

		public Square() {
		}

		public void Add( PhysicalObject po ) {
			physicalObjects.Add( po );
			if ( po is MobileAvatar ) {
				MobileAvatar a = (MobileAvatar)po;
				if ( a.client != null ) {
					clients.Add( a.client );
				}
			}
			if ( po is Terrain ) {
				Terrain t = (Terrain)po;
				heightMap[
					((int)t.Position.X % squareSize) / terrainSize,
					((int)t.Position.Z % squareSize) / terrainSize
				] = t.Position.Y;
			}
		}

		public void Remove( PhysicalObject po ) {
			physicalObjects.Remove( po );
			if ( po is MobileAvatar ) {
				MobileAvatar a = (MobileAvatar)po;
				if ( a.client != null ) {
					clients.Remove( a.client );
				}
			}
		}

		public void NotifyClients( IMessage message ) {
			foreach ( Client c in clients ) {
				c.Send( message );
			}
		}

		public void NotifyClientsExcept( IMessage message, Client client ) {
			foreach ( Client c in clients ) {
				if ( c == client ) continue;
				c.Send( message );
			}
		}
	}
}
