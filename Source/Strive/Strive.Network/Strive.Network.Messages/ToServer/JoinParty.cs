
namespace Strive.Network.Messages.ToServer
{
    public class JoinParty
    {
        public int LeaderId;	// this is so the client can cancel specific invocations

        public JoinParty(int leaderId)
        {
            LeaderId = leaderId; // party leader
        }
    }
}
