using System;
using System.Windows.Media.Media3D;

using Strive.Server.Model;
using Strive.Common;


namespace Strive.Network.Messages.ToClient
{
    public class MobileState : IMessage
    {
        public int ObjectInstanceId;
        public EnumMobileState State;
        public Vector3D Position;

        public MobileState(Mobile mob)
        {
            ObjectInstanceId = mob.ObjectInstanceID;
            State = mob.MobileState;

            // TODO: evaluate if this should FARKING be here
            Position = mob.Position;
        }
    }
}
