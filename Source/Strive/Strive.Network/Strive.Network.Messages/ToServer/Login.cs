using System;

namespace Strive.Network.Messages.ToServer
{
	[Serializable]
	public class Login : IMessage
	{
		public Login(){}
		public Login( string username, string password)
		{
			Username = username;
			Password = password;
		}

		public string Username;
		public string Password;
	}
}
