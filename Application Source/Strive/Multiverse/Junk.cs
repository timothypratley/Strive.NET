using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Junk.
	/// </summary>
	public class Junk : Item {
		public Schema.JunkItemRow junk;

		public Junk(
			Schema.JunkItemRow junk,
			Schema.ItemPhysicalObjectRow item,
			Schema.PhysicalObjectRow physicalObject,
			Schema.RespawnPointRow respawnPoint
		) : base( item, physicalObject, respawnPoint ) {
			this.junk = junk;
		}
	}
}
