using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToServer.GameCommand.Party
{
	[Serializable]
	public class CreateParty : IMessage	{
		string name;
		public CreateParty(){}
		public CreateParty( string name ) {
			this.name = name;
		}
	}
}
