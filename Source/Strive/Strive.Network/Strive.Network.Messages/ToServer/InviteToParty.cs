using System;

namespace Strive.Network.Messages.ToServer
{
	[Serializable]
	public class InviteToParty : IMessage	{
		public int ObjectInstanceId;
		public InviteToParty(){}
        public InviteToParty(int objectInstanceId)
        {
            ObjectInstanceId = objectInstanceId;
        }
	}
}
