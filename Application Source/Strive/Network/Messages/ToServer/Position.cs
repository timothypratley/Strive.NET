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
			this.position_x = (float)po.Position.X;
			this.position_y = (float)po.Position.Y;
			this.position_z = (float)po.Position.Z;
			this.heading_x = (float)po.Heading.X;
			this.heading_y = (float)po.Heading.Y;
			this.heading_z = (float)po.Heading.Z;
		}

		public void Apply( PhysicalObject po ) {
			po.Position.X = this.position_x;
			po.Position.Y = this.position_y;
			po.Position.Z = this.position_z;
			po.Heading.X = this.heading_x;
			po.Heading.Y = this.heading_y;
			po.Heading.Z = this.heading_z;
		}

		public float position_x;
		public float position_y;
		public float position_z;
		public float heading_x;
		public float heading_y;
		public float heading_z;
	}
}
