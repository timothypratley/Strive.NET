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
		public ArrayList physicalObjects = new ArrayList();
		public ArrayList clients = new ArrayList();

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

		public void NotifyClientsAdd( PhysicalObject po ) {
			Strive.Network.Messages.ToClient.AddPhysicalObject message = new Strive.Network.Messages.ToClient.AddPhysicalObject( po );
			foreach ( Client c in clients ) {
				c.Send( message );
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

		public void NotifyClientsRemove( PhysicalObject po ) {
			Strive.Network.Messages.ToClient.DropPhysicalObject message = new Strive.Network.Messages.ToClient.DropPhysicalObject( po );
			foreach ( Client c in clients ) {
				c.Send( message );
			}
		}

		public void NotifyClientsMessage( IMessage message ) {
			foreach ( Client c in clients ) {
				c.Send( message );
			}
		}
	}
}
