using System;
using System.Collections;
using Strive.Multiverse;
using Strive.Network.Server;
using Strive.Network.Messages;

namespace Strive.Server
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
		public static int squareSize = 100;
		public ArrayList physicalObjects = new ArrayList();
		public ArrayList clients = new ArrayList();
		ArrayList[,] heightMap = new ArrayList[Square.squareSize,Square.squareSize];

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

		public void CalculateHeightMap() {
			int i, j;
			for ( i=0; i<Square.squareSize; i++ ) {
				for ( j=0; j<Square.squareSize; j++ ) {
					float [] occupiableSpace = new float[2];
					occupiableSpace[0] = 0.0f;
					occupiableSpace[1] = 100.0f;
					heightMap[i,j] = new ArrayList();
					heightMap[i,j].Add( occupiableSpace );
				}
			}
		}
	}
}
