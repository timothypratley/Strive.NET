using System;

namespace Strive.Network.Messages.ToClient
{
	[Serializable]
	public class ServerInfo : IMessage {
		public int Version;
		public int Build;
		public string Description;

		public ServerInfo(){}
        public ServerInfo(int version, int build, int servername, string description)
        {
            Version = version;
            Build = build;
            Description = description;
        }
	}
}
