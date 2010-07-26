using System;

using Strive.Server.Model;
using Strive.Math3D;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for Position.
	/// </summary>
	[Serializable]
	public class Position : IMessage
	{
		public Position(){}

		public Position( PhysicalObject po ) {
			instance_id = po.ObjectInstanceID;
			position = po.Position;
			rotation = po.Rotation;
		}

		public void Apply( PhysicalObject po ) {
			po.ObjectInstanceID = instance_id;
			po.Position = position;
			po.Rotation = rotation;
		}

		public int instance_id;
		public Vector3D position;
		public Vector3D rotation;
	}
}
