using System;

using Strive.Server.Model;
using Strive.Math3D;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for Position.
	/// </summary>
	[Serializable]
	public class Position : IMessage
	{
		public Position(){}
		public Position( Vector3D position, Vector3D rotation ) {
			this.position = position;
			this.rotation = rotation;
		}

		public Position( PhysicalObject po ) {
			position = po.Position;
			rotation = po.Rotation;
		}

		public void Apply( PhysicalObject po ) {
			po.Position = position;
			po.Rotation = rotation;
		}
		
		public Vector3D position;
		public Vector3D rotation;
	}
}
