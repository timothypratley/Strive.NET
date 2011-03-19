using System.Windows.Media.Media3D;
using Strive.Common;
using Strive.Server.Model;


namespace Strive.Network.Messages.ToClient
{
    public class MobileState : IMessage
    {
        public int ObjectInstanceId;
        public EnumMobileState State;
        public Vector3D Position;
        public Quaternion Rotation;

        public MobileState(Mobile mob)
        {
            ObjectInstanceId = mob.ObjectInstanceId;
            State = mob.MobileState;
            Position = mob.Position;
            Rotation = mob.Rotation;
        }
    }
}
