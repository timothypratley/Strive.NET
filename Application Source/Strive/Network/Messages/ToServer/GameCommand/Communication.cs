using System;

namespace Strive.Network.Messages.ToServer.GameCommand
{
	/// <summary>
	/// Summary description for Communication.
	/// </summary>
	[Serializable]
	public class Communication : IMessage {
		public Communication( CommunicationType communicationType, string message )
		{
			this.communicationType = communicationType;
			this.message = message;
		}

		public CommunicationType communicationType;
		public string message;
	}
}
