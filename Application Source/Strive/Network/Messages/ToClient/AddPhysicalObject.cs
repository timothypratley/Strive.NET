using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddPhysicalObject.
	/// </summary>
	[Serializable]
	public class AddPhysicalObject : IMessage
	{
		public AddPhysicalObject( PhysicalObject po ) {
			this.spawn_id = po.respawnPoint.SpawnID;
			this.model_id = po.physicalObject.ModelID;
			this.name = po.physicalObject.PhysicalObjectName;
			this.x = (float)po.respawnPoint.X;
			this.y = (float)po.respawnPoint.Y;
			this.z = (float)po.respawnPoint.Z;
			this.heading_x = (float)po.respawnPoint.HeadingX;
			this.heading_y = (float)po.respawnPoint.HeadingY;
			this.heading_z = (float)po.respawnPoint.HeadingZ;
		}

		public int spawn_id;
		public int model_id;
		public string name;
		public float x;
		public float y;
		public float z;
		public float heading_x;
		public float heading_y;
		public float heading_z;
	}
}
