using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Item.
	/// </summary>
	public abstract class Item : PhysicalObject
	{
		Schema.ItemPhysicalObjectRow item;

		public Item(
			Schema.ItemPhysicalObjectRow item,
			Schema.PhysicalObjectRow physicalObject,
			Schema.RespawnPointRow respawnPoint
		) : base( physicalObject, respawnPoint ) {
			this.item = item;
		}
	}
}
