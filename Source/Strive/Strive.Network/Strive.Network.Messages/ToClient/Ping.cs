namespace Strive.Network.Messages.ToClient
{
    public class Ping
    {
        public int SequenceNumber;

        public Ping(int sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
        }
    }
}
