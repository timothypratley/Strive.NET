using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for Acknowledge.
	/// </summary>
	[Serializable]
	public class LogMessage : IMessage 
	{
		public LogMessage(){}
		public LogMessage( string Message ) 
		{
			this.Message = Message;
		}
		public string Message;
	}
}
