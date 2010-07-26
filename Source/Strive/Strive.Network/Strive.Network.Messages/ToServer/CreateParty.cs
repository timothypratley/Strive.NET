using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToServer
{
	[Serializable]
	public class CreateParty : IMessage	{
		public string name;
		public CreateParty(){}
		public CreateParty( string name ) {
			this.name = name;
		}
	}
}
