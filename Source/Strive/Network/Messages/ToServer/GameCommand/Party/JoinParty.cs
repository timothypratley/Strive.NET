using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToServer.GameCommand.Party
{
	[Serializable]
	public class JoinParty : IMessage	{
		public int ObjectInstanceID;	// this is so the client can cancel specific invokations
		public JoinParty(){}
		public JoinParty( int ObjectInstanceID ) {
			this.ObjectInstanceID = ObjectInstanceID;
		}
	}
}
