namespace Strive.Network.Messages.ToClient
{
    public class Acknowledge : IMessage
    {
        public int SequenceNumber;

        public Acknowledge(int sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
        }
    }
}
