using System;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for Communication.
	/// </summary>
	[Serializable]
	public struct Communication : IMessage
	{
		public Communication( string message, CommunicationType communicationType )
		{
			this.message = message;
			this.communicationType = communicationType;
		}

		public string message;
		public CommunicationType communicationType;
	}
}
