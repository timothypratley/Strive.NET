namespace Strive.Network.Messages.ToServer
{
    public class InviteToParty : IMessage
    {
        public int ObjectInstanceId;

        public InviteToParty(int objectInstanceId)
        {
            ObjectInstanceId = objectInstanceId;
        }
    }
}
