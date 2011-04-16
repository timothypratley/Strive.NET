using System.Windows.Media.Media3D;
using Strive.Model;


namespace Strive.Network.Messages.ToClient
{
    public class PositionUpdate
    {
        public PositionUpdate(EntityModel e)
        {
            Id = e.Id;
            Position = e.Position;
            Rotation = e.Rotation;
        }

        public int Id;
        public Vector3D Position;
        public Quaternion Rotation;
    }
}
