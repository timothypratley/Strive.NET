using Strive.Model;

namespace Strive.Network.Messages.ToServer
{
    public class CreatePlan
    {
        public CreatePlan(PlanModel plan)
        {
            Plan = plan;
        }

        public PlanModel Plan;
    }
}
