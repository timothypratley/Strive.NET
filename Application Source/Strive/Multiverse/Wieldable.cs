using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Wieldable.
	/// </summary>
	public class Wieldable : Item
	{
		public Schema.WieldableItemRow wieldable;

		public Wieldable(
			Schema.WieldableItemRow wieldable,
			Schema.ItemPhysicalObjectRow item,
			Schema.PhysicalObjectRow physicalObject,
			Schema.RespawnPointRow respawnPoint
		) : base( item, physicalObject, respawnPoint ) {
			this.wieldable = wieldable;
		}
	}
}
