using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for Acknowledge.
	/// </summary>
	[Serializable]
	public class ServerInfo : IMessage {
		public int version;
		public int build;
		public string description;

		public ServerInfo(){}
		public ServerInfo( int version, int build, int servername, string description ) {
			this.version = version;
			this.build = build;
			this.description = description;
		}
	}
}
