namespace Strive.Network.Messages.ToClient
{
    public class Acknowledge
    {
        public int SequenceNumber;

        public Acknowledge(int sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
        }
    }
}
