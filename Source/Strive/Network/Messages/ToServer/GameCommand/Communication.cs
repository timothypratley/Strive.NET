using System;

namespace Strive.Network.Messages.ToServer.GameCommand
{
	/// <summary>
	/// Summary description for Communication.
	/// </summary>
	[Serializable]
	public class Communication : IMessage {
		public int communicationType;
		public string message;
		public Communication(){}
		public Communication( CommunicationType communicationType, string message ) {
			this.communicationType = (int)communicationType;
			this.message = message;
		}
	}
}
