using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToServer
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
