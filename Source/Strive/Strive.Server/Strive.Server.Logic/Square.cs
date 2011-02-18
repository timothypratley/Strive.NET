using System.Collections.Generic;
using Strive.Server.Model;
using Strive.Network.Messaging;
using Strive.Network.Messages;
using Strive.Common;


namespace Strive.Server.Logic
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
		public const int SquareSize = Constants.ObjectScopeRadius;
        public List<PhysicalObject> PhysicalObjects = new List<PhysicalObject>();
        public List<ClientConnection> Clients = new List<ClientConnection>();

	    public void Add( PhysicalObject po ) {
			PhysicalObjects.Add( po );
			if ( po is MobileAvatar ) {
				var a = (MobileAvatar)po;
				if ( a.Client != null ) {
					Clients.Add( a.Client );
				}
			}
		}

		public void Remove( PhysicalObject po ) {
			PhysicalObjects.Remove( po );
			if ( po is MobileAvatar ) {
				var a = (MobileAvatar)po;
				if ( a.Client != null ) {
					Clients.Remove( a.Client );
				}
			}
		}

		public void NotifyClients( IMessage message ) {
			foreach ( ClientConnection c in Clients ) {
				c.Send( message );
			}
		}

		public void NotifyClientsExcept( IMessage message, ClientConnection client ) {
			foreach ( ClientConnection c in Clients ) {
				if ( c == client ) continue;
				c.Send( message );
			}
		}
	}
}
