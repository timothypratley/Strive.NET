using System;


namespace Strive.Model
{
    public enum EnumPlanAction
    {
        Move,
        Harvest,
        Build,
        Destroy,
        Protect,
        Capture
    }

    public class PlanModel
    {
        /// <summary>
        /// A PlanModel indicates some action the user wishes to occur, such as moving an entity to a target destination.
        /// </summary>
        /// <param name="action">what will happen</param>
        /// <param name="protagonist">who will do the action</param>
        /// <param name="startTime">when the plan can be started</param>
        /// <param name="start">entity target start state</param>
        /// <param name="finishTime">when the plan should be finished</param>
        /// <param name="finish">entity target finish state</param>
        /// <param name="lateFee">how important it is to finish on time</param>
        public PlanModel(int id, EnumPlanAction action, EntityModel protagonist,
            DateTime startTime, EntityModel start, DateTime finishTime, EntityModel finish, float lateFee)
        {
            Id = id;
            Action = action;
            Protagonist = protagonist;
            StartTime = startTime;
            Start = start;
            FinishTime = finishTime;
            Finish = finish;
            LateFee = LateFee;
        }

        public int Id { get; private set; }
        public string Owner { get; private set; }
        public EnumPlanAction Action { get; private set; }
        public EntityModel Protagonist { get; private set; }

        public DateTime StartTime { get; private set; }
        public EntityModel Start { get; private set; }
        public DateTime FinishTime { get; private set; }
        public EntityModel Finish { get; private set; }
        public float LateFee { get; private set; }

        // TODO: not sure if we need them
        //public EntityModel DeliverTo;     // If the finish location is on another entity
        //public EntityModel HeldBy;        // If the start location is on another entity
    }
}
