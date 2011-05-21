
namespace Strive.Data.Events
{
    public class ProductionStartedEvent : Event
    {
        public ProductionStartedEvent(int producerId, int productId, string description)
        {
            ProducerId = producerId;
            ProductId = productId;
            Description = description;
        }

        public int ProducerId { get; private set; }
        public int ProductId { get; private set; }
    }
}
