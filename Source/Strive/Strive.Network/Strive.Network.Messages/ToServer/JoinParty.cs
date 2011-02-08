using System;

namespace Strive.Network.Messages.ToServer
{
	[Serializable]
	public class JoinParty : IMessage	{
		public int ObjectInstanceId;	// this is so the client can cancel specific invokations
		public JoinParty(){}
        public JoinParty(int objectInstanceId)
        {
            ObjectInstanceId = objectInstanceId; // party leader
        }
	}
}
