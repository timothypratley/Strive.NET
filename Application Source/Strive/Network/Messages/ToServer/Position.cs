using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for Position.
	/// </summary>
	[Serializable]
	public struct Position : IMessage
	{
		public Position( PhysicalObject po ) {
			this.position_x = (float)po.respawnPoint.X;
			this.position_y = (float)po.respawnPoint.Y;
			this.position_z = (float)po.respawnPoint.Z;
			this.heading_x = (float)po.respawnPoint.HeadingX;
			this.heading_y = (float)po.respawnPoint.HeadingY;
			this.heading_z = (float)po.respawnPoint.HeadingZ;
		}

		public void Apply( PhysicalObject po ) {
			po.respawnPoint.X = this.position_x;
			po.respawnPoint.Y = this.position_y;
			po.respawnPoint.Z = this.position_z;
			po.respawnPoint.HeadingX = this.heading_x;
			po.respawnPoint.HeadingY = this.heading_y;
			po.respawnPoint.HeadingZ = this.heading_z;
		}

		public float position_x;
		public float position_y;
		public float position_z;
		public float heading_x;
		public float heading_y;
		public float heading_z;
	}
}
