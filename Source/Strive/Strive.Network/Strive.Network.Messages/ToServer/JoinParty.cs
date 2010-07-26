using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToServer
{
	[Serializable]
	public class JoinParty : IMessage	{
		public int ObjectInstanceID;	// this is so the client can cancel specific invokations
		public JoinParty(){}
		public JoinParty( int ObjectInstanceID ) {
			this.ObjectInstanceID = ObjectInstanceID; // party leader
		}
	}
}
