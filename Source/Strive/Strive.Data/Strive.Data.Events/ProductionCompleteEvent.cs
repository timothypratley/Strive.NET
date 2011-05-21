using Strive.Model;

namespace Strive.Data.Events
{
    public class ProductionCompleteEvent : Event
    {
        public ProductionCompleteEvent(int producerId, EntityModel entity, string description)
        {
            ProducerId = producerId;
            Entity = entity;
            Description = description;
        }

        public int ProducerId { get; private set; }
        public EntityModel Entity { get; private set; }
    }
}
