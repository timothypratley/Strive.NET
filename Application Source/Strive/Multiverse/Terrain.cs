using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Mobile.
	/// </summary>
	public class Terrain : PhysicalObject
	{
		public Schema.TerrainPhysicalObjectRow terrain;

		public Terrain (
			Schema.TerrainPhysicalObjectRow terrain,
			Schema.PhysicalObjectRow physicalObject,
			Schema.RespawnPointRow respawnPoint
		) : base ( physicalObject, respawnPoint ) {
			this.terrain = terrain;
		}
	}
}
