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
			spawnIDs = new int[physicalObjects.Count];
			int i = 0;
			foreach ( PhysicalObject po in physicalObjects ) {
				spawnIDs[i] = po.respawnPoint.SpawnID;
				i++;
			}
		}

		public int [] spawnIDs;
	}
}
