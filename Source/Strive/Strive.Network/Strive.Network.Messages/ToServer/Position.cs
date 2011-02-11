using System;
using System.Windows.Media.Media3D;

using Strive.Server.Model;


namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for Position.
	/// </summary>
	[Serializable]
	public class MyPosition : IMessage
	{
		public MyPosition(){}
		public MyPosition( Vector3D position, Quaternion rotation ) {
			this.position = position;
			this.rotation = rotation;
		}

		public MyPosition( PhysicalObject po ) {
			position = po.Position;
			rotation = po.Rotation;
		}

		public void Apply( PhysicalObject po ) {
			po.Position = position;
			po.Rotation = rotation;
		}
		
		public Vector3D position;
		public Quaternion rotation;
	}
}
