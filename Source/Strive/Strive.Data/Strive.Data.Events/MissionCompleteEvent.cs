
using Strive.Model;
namespace Strive.Data.Events
{
    public class MissionCompleteEvent : Event
    {
        public MissionCompleteEvent(MissionModel mission, string description)
        {
            Mission = mission;
            Description = description;
        }

        public MissionModel Mission { get; set; }
    }
}
