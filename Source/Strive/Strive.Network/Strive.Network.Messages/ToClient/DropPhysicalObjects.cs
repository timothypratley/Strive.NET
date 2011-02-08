using System;
using System.Linq;
using System.Collections.Generic;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	[Serializable]
	public class DropPhysicalObjects : IMessage {
        public int[] InstanceIDs;
        
        public DropPhysicalObjects(){}
        public DropPhysicalObjects(IEnumerable<PhysicalObject> physicalObjects)
        {
            InstanceIDs = physicalObjects.Select(po => po.ObjectInstanceID).ToArray();
        }
	}
}
