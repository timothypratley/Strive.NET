using System;

namespace Strive.Network.Messages.ToServer
{
	[Serializable]
	public class CancelSkill : IMessage	{
		public int InvokationId;	// this is so the client can cancel specific invokations

		public CancelSkill(){}
        public CancelSkill(int invokationId)
        {
            InvokationId = invokationId;
        }
	}
}
