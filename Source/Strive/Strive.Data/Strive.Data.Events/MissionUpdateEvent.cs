using Strive.Model;

namespace Strive.Data.Events
{
    public class MissionUpdateEvent : Event
    {
        public MissionUpdateEvent(MissionModel mission, string description)
        {
            Mission = mission;
            Description = description;
        }

        public MissionModel Mission { get; set; }
    }
}
