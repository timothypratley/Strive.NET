using System;

namespace Strive.Network.Messages.ToClient
{
	[Serializable]
	public class Ping : IMessage {
        public int SequenceNumber;

        public Ping(){}
        public Ping(int sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
        }
	}
}
