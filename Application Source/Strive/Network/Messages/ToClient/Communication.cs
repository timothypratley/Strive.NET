using System;
using Strive.Network.Messages;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for Communication.
	/// </summary>
	[Serializable]
	public class Communication : IMessage
	{
		public Communication(){}
		public Communication( string name, string message, CommunicationType communicationType )
		{
			this.name = name;
			this.message = message;
			this.communicationType = communicationType;
		}

		public string name;
		public string message;
		public CommunicationType communicationType;
	}
}
