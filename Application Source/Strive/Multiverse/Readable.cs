using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Readable.
	/// </summary>
	public class Readable : Item
	{
		public Schema.ReadableItemRow readable;

		public Readable(
			Schema.ReadableItemRow readable,
			Schema.ItemPhysicalObjectRow item,
			Schema.PhysicalObjectRow physicalObject,
			Schema.RespawnPointRow respawnPoint
		) : base( item, physicalObject, respawnPoint ) {
			this.readable = readable;
		}
	}
}
