using System;

namespace Strive.Network.Messages.ToServer.GameCommand
{
	/// <summary>
	/// Summary description for Communication.
	/// </summary>
	[Serializable]
	public class Communication : IMessage {
		public CommunicationType communicationType;
		public string message;
		public Communication(){}
		public Communication( CommunicationType communicationType, string message ) {
			this.communicationType = communicationType;
			this.message = message;
		}
	}
}
