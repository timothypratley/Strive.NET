using Strive.Model;

namespace Strive.Network.Messages.ToClient
{
    public class CurrentHitpoints
    {
        public float Health;

        public CurrentHitpoints(EntityModel e)
        {
            Health = e.Health;
        }
    }
}
