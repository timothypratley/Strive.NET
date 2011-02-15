using System.Linq;
using System.Collections.Generic;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    public class DropPhysicalObjects : IMessage
    {
        public int[] InstanceIDs;

        public DropPhysicalObjects(IEnumerable<PhysicalObject> physicalObjects)
        {
            InstanceIDs = physicalObjects.Select(po => po.ObjectInstanceId).ToArray();
        }
    }
}
