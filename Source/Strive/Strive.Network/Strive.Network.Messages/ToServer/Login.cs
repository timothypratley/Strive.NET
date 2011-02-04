using System;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for Login.
	/// </summary>
	[Serializable]
	public class Login : IMessage
	{
		public Login(){}
		public Login( string username, string password, NetworkProtocolType protocol = NetworkProtocolType.TcpOnly)
		{
			this.username = username;
			this.password = password;
			this.protocol = protocol;
		}

		public string username;
		public string password;
		public NetworkProtocolType protocol;
	}
}
