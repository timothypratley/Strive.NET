namespace Strive.Network.Messages.ToServer
{
    public class InviteToParty
    {
        public int ObjectInstanceId;

        public InviteToParty(int objectInstanceId)
        {
            ObjectInstanceId = objectInstanceId;
        }
    }
}
