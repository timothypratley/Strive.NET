using System;
using Strive.Multiverse;
using Strive.Network.Server;

namespace Strive.Server
{
	/// <summary>
	/// A server side Avatar is a Mobile that
	/// optionally has a client associated with it.
	/// The server world contains only Avatars;
	/// no Mobiles, as all Mobiles are possesable and hence are Avatars.
	/// If a client is associated to an Avatar,
	/// that client is controlling the Mobile in question.
	/// </summary>
	public class Avatar : Mobile
	{
		public Client client = null;

		public Avatar( Client c, Mobile m ) : base( m.mobile, m.physicalObject, m.respawnPoint ) {
			client = c;
		}

		public Avatar(
			Schema.MobilePhysicalObjectRow mobile,
			Schema.PhysicalObjectRow physicalObject,
			Schema.RespawnPointRow respawnPoint
		) : base( mobile, physicalObject, respawnPoint ) {
		}
	}
}
