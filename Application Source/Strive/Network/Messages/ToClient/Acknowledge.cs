using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for Acknowledge.
	/// </summary>
	[Serializable]
	public struct Acknowledge : IMessage {
		public Acknowledge( int SequenceNumber ) {
			this.SequenceNumber = SequenceNumber;
		}
		public int SequenceNumber;
	}
}
