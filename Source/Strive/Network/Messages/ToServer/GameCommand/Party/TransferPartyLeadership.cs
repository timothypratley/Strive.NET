using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToServer.GameCommand.Party
{
	[Serializable]
	public class TransferPartyLeadership : IMessage	{
		public int ObjectInstanceID;	// this is so the client can cancel specific invokations
		public TransferPartyLeadership(){}
		public TransferPartyLeadership( int ObjectInstanceID ) {
			this.ObjectInstanceID = ObjectInstanceID;
		}
	}
}
