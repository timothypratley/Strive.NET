namespace Strive.Network.Messages.ToServer
{
    public class TransferPartyLeadership : IMessage
    {
        public int ObjectInstanceId;

        public TransferPartyLeadership(int objectInstanceId)
        {
            ObjectInstanceId = objectInstanceId;
        }
    }
}
