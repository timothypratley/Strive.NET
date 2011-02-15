using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    public class CurrentHitpoints : IMessage
    {
        public float HitPoints;

        public CurrentHitpoints(PhysicalObject po)
        {
            HitPoints = po.HitPoints;
        }
    }
}
