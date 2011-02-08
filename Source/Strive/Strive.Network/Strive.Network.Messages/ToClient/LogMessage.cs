using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	[Serializable]
	public class LogMessage : IMessage 
	{
        public string Message;
        
        public LogMessage() { }
        public LogMessage(string message)
        {
            Message = message;
        }
	}
}
