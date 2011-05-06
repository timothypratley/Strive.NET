namespace Strive.Network.Messages.ToServer
{
    public class TransferPartyLeadership
    {
        public string Leader;

        public TransferPartyLeadership(string leader)
        {
            Leader = leader;
        }
    }
}
