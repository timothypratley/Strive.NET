namespace Strive.Network.Messages.ToClient
{
    public class Ping : IMessage
    {
        public int SequenceNumber;

        public Ping(int sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
        }
    }
}
