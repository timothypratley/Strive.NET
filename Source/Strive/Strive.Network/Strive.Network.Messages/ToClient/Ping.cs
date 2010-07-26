using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for Ping.
	/// </summary>
	[Serializable]
	public class Ping : IMessage {
		public Ping(){}
		public Ping( int SequenceNumber ) {
			this.SequenceNumber = SequenceNumber;
		}
		public int SequenceNumber;
	}
}
