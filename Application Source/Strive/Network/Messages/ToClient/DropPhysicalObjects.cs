using System;
using System.Collections;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for DropPhysicalObject.
	/// </summary>
	[Serializable]
	public class DropPhysicalObjects : IMessage {
		public DropPhysicalObjects( ArrayList physicalObjects ) {
			instanceIDs = new int[physicalObjects.Count];
			int i = 0;
			foreach ( PhysicalObject po in physicalObjects ) {
				instanceIDs[i] = po.instance.ObjectInstanceID;
				i++;
			}
		}

		public int [] instanceIDs;
	}
}
