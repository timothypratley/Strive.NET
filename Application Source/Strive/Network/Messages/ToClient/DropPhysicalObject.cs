using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for DropPhysicalObject.
	/// </summary>
	[Serializable]
	public class DropPhysicalObject : IMessage {
		public DropPhysicalObject( PhysicalObject po ) {
			this.respawn_id = po.respawnPoint.SpawnID;
		}

		public int respawn_id;
	}
}
