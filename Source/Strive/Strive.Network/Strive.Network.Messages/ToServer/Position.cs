using System.Windows.Media.Media3D;
using Strive.Common;


namespace Strive.Network.Messages.ToServer
{
    public class MyPosition
    {
        public int PossessingId;
        public Vector3D Position;
        public Quaternion Rotation;
        public EnumMobileState State;

        public MyPosition(int possessingId, Vector3D position, Quaternion rotation, EnumMobileState state)
        {
            PossessingId = possessingId;
            Position = position;
            Rotation = rotation;
            State = state;
        }
    }
}
