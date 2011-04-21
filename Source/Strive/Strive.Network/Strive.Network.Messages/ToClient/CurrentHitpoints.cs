using Strive.Model;

namespace Strive.Network.Messages.ToClient
{
    public class CurrentHealth
    {
        public float Health;

        public CurrentHealth(EntityModel e)
        {
            Health = e.Health;
        }
    }
}
