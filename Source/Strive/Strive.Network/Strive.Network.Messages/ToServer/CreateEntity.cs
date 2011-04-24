using Strive.Model;

namespace Strive.Network.Messages.ToServer
{
    public class CreateEntity
    {
        public CreateEntity(EntityModel entity)
        {
            Entity = entity;
        }

        public EntityModel Entity;
    }
}
