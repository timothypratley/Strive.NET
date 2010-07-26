using System;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for Pong.
	/// </summary>
	[Serializable]
	public class Pong : IMessage
	{
		public int SequenceNumber;
		public Pong(){}
		public Pong( int SequenceNumber ) {
			this.SequenceNumber = SequenceNumber;
		}
	}
}
