using System;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for Login.
	/// </summary>
	[Serializable]
	public struct Login : IMessage
	{
		public Login( string username, string password )
		{
			this.username = username;
			this.password = password;
		}

		public string username;
		public string password;
	}
}
