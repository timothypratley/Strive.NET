using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Equipable.
	/// </summary>
	public class Equipable : Item
	{
		public Schema.EquipableItemRow equipable;

		public Equipable(
			Schema.EquipableItemRow equipable,
			Schema.ItemPhysicalObjectRow item,
			Schema.PhysicalObjectRow physicalObject,
			Schema.RespawnPointRow respawnPoint
		) : base( item,	physicalObject,	respawnPoint ) {
			this.equipable = equipable;
		}
	}
}
