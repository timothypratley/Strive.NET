using Strive.Model;

namespace Strive.Network.Messages.ToServer
{
    public class CreateMission
    {
        public CreateMission(MissionModel mission)
        {
            Mission = mission;
        }

        public MissionModel Mission;
    }
}
