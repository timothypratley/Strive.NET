using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for PhysicalObject.
	/// </summary>
	public abstract class PhysicalObject
	{
		public Schema.RespawnPointRow respawnPoint;
		public Schema.PhysicalObjectRow physicalObject;

		public PhysicalObject(
			Schema.PhysicalObjectRow physicalObject,
			Schema.RespawnPointRow respawnPoint
		) {
			this.respawnPoint = respawnPoint;
			this.physicalObject = physicalObject;
		}
	}
}
