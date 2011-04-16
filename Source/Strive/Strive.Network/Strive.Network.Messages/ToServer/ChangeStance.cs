using Strive.Common;

namespace Strive.Network.Messages.ToServer
{
    public class ChangeStance
    {
        public EnumStance StanceId;

        public ChangeStance(EnumStance stanceId)
        {
            StanceId = stanceId;
        }
    }
}
