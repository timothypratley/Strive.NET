using System;

namespace Strive.Network.Messages.ToServer
{
	[Serializable]
	public class TransferPartyLeadership : IMessage	{
		public int ObjectInstanceId;

		public TransferPartyLeadership(){}
        public TransferPartyLeadership(int objectInstanceId)
        {
            ObjectInstanceId = objectInstanceId;
        }
	}
}
