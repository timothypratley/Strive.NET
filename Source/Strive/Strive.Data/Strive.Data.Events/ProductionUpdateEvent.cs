
namespace Strive.Data.Events
{
    public class ProductionUpdateEvent : Event
    {
        public ProductionUpdateEvent(int entityId, float progressChange, string description)
        {
            EntityId = entityId;
            ProgressChange = progressChange;
            Description = description;
        }

        public int EntityId { get; private set; }
        public float ProgressChange { get; private set; }
    }
}
