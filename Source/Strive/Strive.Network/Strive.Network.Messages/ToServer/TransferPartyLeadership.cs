namespace Strive.Network.Messages.ToServer
{
    public class TransferPartyLeadership
    {
        public int ObjectInstanceId;

        public TransferPartyLeadership(int objectInstanceId)
        {
            ObjectInstanceId = objectInstanceId;
        }
    }
}
