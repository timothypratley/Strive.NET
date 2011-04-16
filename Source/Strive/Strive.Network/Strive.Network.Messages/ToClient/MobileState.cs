using System.Windows.Media.Media3D;
using Strive.Common;
using Strive.Model;


namespace Strive.Network.Messages.ToClient
{
    public class MobileState
    {
        public int ObjectInstanceId;
        public EnumMobileState State;
        public Vector3D Position;
        public Quaternion Rotation;

        public MobileState(EntityModel mob)
        {
            ObjectInstanceId = mob.Id;
            State = mob.MobileState;
            Position = mob.Position;
            Rotation = mob.Rotation;
        }
    }
}
