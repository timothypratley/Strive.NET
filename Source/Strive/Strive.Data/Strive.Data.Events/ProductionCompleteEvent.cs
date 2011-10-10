using Strive.Model;

namespace Strive.Data.Events
{
    public class ProductionCompleteEvent : Event
    {
        public ProductionCompleteEvent(int producerId, EntityModel product, string description)
        {
            ProducerId = producerId;
            Product = product;
            Description = description;
        }

        public int ProducerId { get; private set; }
        public EntityModel Product { get; private set; }
    }
}
