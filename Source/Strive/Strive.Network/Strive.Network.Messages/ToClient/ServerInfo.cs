namespace Strive.Network.Messages.ToClient
{
    public class ServerInfo : IMessage
    {
        public int Version;
        public int Build;
        public string ServerName;
        public string Description;

        public ServerInfo(int version, int build, string servername, string description)
        {
            Version = version;
            Build = build;
            ServerName = servername;
            Description = description;
        }
    }
}
