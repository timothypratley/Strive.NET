
namespace Strive.Network.Messages.ToServer
{
    public class JoinParty
    {
        public string Leader;	// this is so the client can cancel specific invocations

        public JoinParty(string leader)
        {
            Leader = leader; // party leader
        }
    }
}
