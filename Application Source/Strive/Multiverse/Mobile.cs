using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Mobile.
	/// </summary>
	public class Mobile : PhysicalObject
	{
		public Schema.MobilePhysicalObjectRow mobile;

		public Mobile (
			Schema.MobilePhysicalObjectRow mobile,
			Schema.PhysicalObjectRow physicalObject,
			Schema.RespawnPointRow respawnPoint
		) : base ( physicalObject, respawnPoint ) {
			this.mobile = mobile;
		}
	}
}
