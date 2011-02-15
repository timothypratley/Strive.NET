using System.Windows.Media.Media3D;
using Strive.Server.Model;


namespace Strive.Network.Messages.ToClient
{
    public class PositionUpdate : IMessage
    {
        public PositionUpdate(PhysicalObject po)
        {
            InstanceId = po.ObjectInstanceId;
            Position = po.Position;
            Rotation = po.Rotation;
        }

        public int InstanceId;
        public Vector3D Position;
        public Quaternion Rotation;
    }
}
