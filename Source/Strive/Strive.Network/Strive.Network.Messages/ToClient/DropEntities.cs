using System.Collections.Generic;
using System.Linq;
using Strive.Model;

namespace Strive.Network.Messages.ToClient
{
    public class DropEntities
    {
        public int[] InstanceIDs;

        public DropEntities(IEnumerable<EntityModel> entities)
        {
            InstanceIDs = entities.Select(po => po.Id).ToArray();
        }
    }
}
