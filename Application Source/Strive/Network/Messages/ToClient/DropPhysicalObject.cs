using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for DropPhysicalObject.
	/// </summary>
	[Serializable]
	public class DropPhysicalObject : IMessage {
		public DropPhysicalObject( PhysicalObject po ) {
			this.instance_id = po.instance.ObjectInstanceID;
		}

		public int instance_id;
	}
}
