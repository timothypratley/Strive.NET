
using Strive.Model;
namespace Strive.Data.Events
{
    public class PlanCompleteEvent : Event
    {
        public PlanCompleteEvent(PlanModel plan, string description)
        {
            Plan = plan;
            Description = description;
        }

        public PlanModel Plan { get; set; }
    }
}
