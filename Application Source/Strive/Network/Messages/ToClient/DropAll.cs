using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for DropPhysicalObject.
	/// </summary>
	[Serializable]
	public class DropPhysicalObject : IMessage {
		public DropPhysicalObject(){}
		public DropPhysicalObject( PhysicalObject po ) {
			this.instance_id = po.ObjectInstanceID;
		}

		public int instance_id;
	}
}
