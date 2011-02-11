using System;

namespace Strive.Network.Messages.ToServer
{
	[Serializable]
	public class Communicate : IMessage
	{
	    public string To;
		public CommunicationType CommunicationType;
		public string Message;

		public Communicate(){}
        public Communicate(string to, CommunicationType communicationType, string message)
        {
            To = to;
            CommunicationType = communicationType;
            Message = message;
        }
	}
}
