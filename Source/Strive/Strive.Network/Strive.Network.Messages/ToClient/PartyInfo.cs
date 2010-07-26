using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	public class PartyInfo : IMessage {
		public int [] MobileID;
		public string [] MobileName;
		public PartyInfo(){}
		public PartyInfo( int [] MobileID, string [] MobileName ) {
			this.MobileID = MobileID;
			this.MobileName = MobileName;
		}
	}
}
