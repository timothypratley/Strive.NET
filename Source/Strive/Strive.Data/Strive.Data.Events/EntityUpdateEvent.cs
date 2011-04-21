using Strive.Model;

namespace Strive.Data.Events
{
    public class EntityUpdateEvent : Event
    {
        public EntityUpdateEvent(EntityModel entity, string description)
        {
            Entity = entity;
            Description = description;
        }

        public EntityModel Entity { get; set; }
    }
}
