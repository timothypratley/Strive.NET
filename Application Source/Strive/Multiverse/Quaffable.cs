using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Quaffable.
	/// </summary>
	public class Quaffable : Item
	{
		public Schema.QuaffableItemRow quaffable;

		public Quaffable(
			Schema.QuaffableItemRow quaffable,
			Schema.ItemPhysicalObjectRow item,
			Schema.PhysicalObjectRow physicalObject,
			Schema.RespawnPointRow respawnPoint
		) : base( item, physicalObject, respawnPoint ) {
			this.quaffable = quaffable;
		}
	}
}
