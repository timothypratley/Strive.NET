using System;

namespace Strive.Network.Messages.ToServer
{
	[Serializable]
	public class Communication : IMessage {
		public CommunicationType CommunicationType;
		public string Message;

		public Communication(){}
        public Communication(CommunicationType communicationType, string message)
        {
            CommunicationType = communicationType;
            Message = message;
        }
	}
}
