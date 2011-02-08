using System;

namespace Strive.Network.Messages.ToClient
{
	[Serializable]
	public class Communication : IMessage
	{
        public string Name;
        public string Message;
        public CommunicationType CommunicationType;
        
        public Communication(){}
		public Communication( string name, string message, CommunicationType communicationType )
		{
			Name = name;
			Message = message;
			CommunicationType = communicationType;
		}
	}
}
