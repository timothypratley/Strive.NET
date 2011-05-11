using Strive.Model;

namespace Strive.Data.Events
{
    public class PlanUpdateEvent : Event
    {
        public PlanUpdateEvent(PlanModel plan, string description)
        {
            Plan = plan;
            Description = description;
        }

        public PlanModel Plan { get; set; }
    }
}
