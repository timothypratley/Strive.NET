using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToServer.GameCommand.Party
{
	[Serializable]
	public class InviteToParty : IMessage	{
		public int ObjectInstanceID;
		public InviteToParty(){}
		public InviteToParty( int ObjectInstanceID ) {
			this.ObjectInstanceID = ObjectInstanceID;
		}
	}
}
