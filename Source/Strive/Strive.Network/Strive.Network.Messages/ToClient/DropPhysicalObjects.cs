using System.Collections.Generic;
using System.Linq;
using Strive.Model;

namespace Strive.Network.Messages.ToClient
{
    public class DropPhysicalObjects
    {
        public int[] InstanceIDs;

        public DropPhysicalObjects(IEnumerable<EntityModel> physicalObjects)
        {
            InstanceIDs = physicalObjects.Select(po => po.Id).ToArray();
        }
    }
}
