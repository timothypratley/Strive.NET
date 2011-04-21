using System.Windows.Media.Media3D;
using Strive.Common;


namespace Strive.Network.Messages.ToServer
{
    public class MyPosition
    {
        public int PossessingId;
        public Vector3D Position;
        public Quaternion Rotation;
        public EnumMobileState MobileState;

        public MyPosition(int possessingId, Vector3D position, Quaternion rotation, EnumMobileState mobileState)
        {
            PossessingId = possessingId;
            Position = position;
            Rotation = rotation;
            MobileState = mobileState;
        }
    }
}
