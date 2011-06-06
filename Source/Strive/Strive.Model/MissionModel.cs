using System;
using System.Windows.Media.Media3D;
using Microsoft.FSharp.Collections;


namespace Strive.Model
{
    public enum EnumMissionAction
    {
        Move,
        Harvest,
        Build,
        Destroy,
        Protect,
        Capture
    }

    public class MissionModel
    {
        /// <summary>
        /// A MissionModel indicates some action the user wishes to occur, such as moving an entity to a target destination.
        /// </summary>
        /// <param name="action">what will happen</param>
        /// <param name="protagonist">who will do the action</param>
        /// <param name="startTime">when the mission can be started</param>
        /// <param name="start">entity target start state</param>
        /// <param name="finishTime">when the mission should be finished</param>
        /// <param name="destination">entity target finish state</param>
        /// <param name="lateFee">how important it is to finish on time</param>
        public MissionModel(int id, EnumMissionAction action, FSharpSet<int> doerIds,
            DateTime startTime, FSharpSet<int> targets, DateTime finishTime, Vector3D destination, float lateFee)
        {
            Id = id;
            Owner = String.Empty;
            Action = action;
            DoerIds = doerIds;
            StartTime = startTime;
            Targets = targets;
            FinishTime = finishTime;
            Destination = destination;
            LateFee = LateFee;
        }

        // convenience constructor often only one actor is involved
        public MissionModel(int id, EnumMissionAction action, int doerId,
            DateTime startTime, FSharpSet<int> targets, DateTime finishTime, Vector3D destination, float lateFee)
            : this(id, action, new FSharpSet<int>(new[] { doerId }), startTime, targets, finishTime, destination, lateFee) { }

        public int Id { get; private set; }
        public string Owner { get; private set; }
        public EnumMissionAction Action { get; private set; }
        public FSharpSet<int> DoerIds { get; private set; }

        public DateTime StartTime { get; private set; }
        public FSharpSet<int> Targets { get; private set; }
        public DateTime FinishTime { get; private set; }
        public Vector3D Destination { get; private set; }
        public float LateFee { get; private set; }

        // TODO: not sure if we need them
        //public EntityModel DeliverTo;     // If the finish location is on another entity
        //public EntityModel HeldBy;        // If the start location is on another entity
    }
}
