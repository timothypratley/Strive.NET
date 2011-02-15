using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    public class DropPhysicalObject : IMessage
    {
        public int InstanceId;

        public DropPhysicalObject(PhysicalObject po)
        {
            InstanceId = po.ObjectInstanceId;
        }
    }
}
