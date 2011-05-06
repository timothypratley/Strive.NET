using System.Windows.Media.Media3D;
using Strive.Model;
using Strive.Common;


namespace Strive.Network.Messages.ToClient
{
    public class PositionUpdate
    {
        public PositionUpdate(EntityModel entity)
        {
            Id = entity.Id;
            State = entity.MobileState;
            Position = entity.Position;
            Rotation = entity.Rotation;
        }

        public int Id;
        public EnumMobileState State;
        public Vector3D Position;
        public Quaternion Rotation;
    }
}
