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
			this.instance_id = po.instance.ObjectInstanceID;
			this.position_x = (float)po.instance.X;
			this.position_y = (float)po.instance.Y;
			this.position_z = (float)po.instance.Z;
			this.heading_x = (float)po.instance.HeadingX;
			this.heading_y = (float)po.instance.HeadingY;
			this.heading_z = (float)po.instance.HeadingZ;
		}

		public void Apply( PhysicalObject po ) {
			po.instance.ObjectInstanceID = this.instance_id;
			po.instance.X = this.position_x;
			po.instance.Y = this.position_y;
			po.instance.Z = this.position_z;
			po.instance.HeadingX = this.heading_x;
			po.instance.HeadingY = this.heading_y;
			po.instance.HeadingZ = this.heading_z;
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
