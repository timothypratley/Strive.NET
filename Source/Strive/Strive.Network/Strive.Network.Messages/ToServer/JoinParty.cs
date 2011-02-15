
namespace Strive.Network.Messages.ToServer
{
    public class JoinParty : IMessage
    {
        public int ObjectInstanceId;	// this is so the client can cancel specific invokations

        public JoinParty(int objectInstanceId)
        {
            ObjectInstanceId = objectInstanceId; // party leader
        }
    }
}
