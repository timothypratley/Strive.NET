using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToServer
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
