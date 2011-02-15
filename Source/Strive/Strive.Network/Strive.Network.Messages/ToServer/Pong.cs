namespace Strive.Network.Messages.ToServer
{
    public class Pong : IMessage
    {
        public int SequenceNumber;

        public Pong(int sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
        }
    }
}
