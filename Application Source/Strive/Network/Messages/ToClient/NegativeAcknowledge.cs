using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for Acknowledge.
	/// </summary>
	[Serializable]
	public class NegativeAcknowledge : IMessage {
		public NegativeAcknowledge( int SequenceNumber ) {
			this.SequenceNumber = SequenceNumber;
		}
		public int SequenceNumber;
	}
}
