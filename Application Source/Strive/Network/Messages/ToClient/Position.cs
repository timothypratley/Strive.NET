using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for Position.
	/// </summary>
	[Serializable]
	public class Position : IMessage
	{
		public Position() {
		}

		public Position( PhysicalObject po ) {
			this.position_x = (float)po.X;
			this.position_y = (float)po.Y;
			this.position_z = (float)po.Z;
			this.heading_x = (float)po.HeadingX;
			this.heading_y = (float)po.HeadingY;
			this.heading_z = (float)po.HeadingZ;
		}

		public void Apply( PhysicalObject po ) {
			po.ObjectInstanceID = this.instance_id;
			po.X = this.position_x;
			po.Y = this.position_y;
			po.Z = this.position_z;
			po.HeadingX = this.heading_x;
			po.HeadingY = this.heading_y;
			po.HeadingZ = this.heading_z;
		}

		public int instance_id;
		public float position_x;
		public float position_y;
		public float position_z;
		public float heading_x;
		public float heading_y;
		public float heading_z;
	}
}
