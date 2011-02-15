using Strive.Common;

namespace Strive.Network.Messages.ToServer
{
    public class ChangeStance : IMessage
    {
        public EnumStance StanceId;

        public ChangeStance(EnumStance stanceId)
        {
            StanceId = stanceId;
        }
    }
}
