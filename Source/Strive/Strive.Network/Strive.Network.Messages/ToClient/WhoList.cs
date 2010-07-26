using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	public class WhoList : IMessage {
		public int [] MobileID;
		public string [] MobileName;
		public WhoList(){}
		public WhoList( int [] MobileID, string [] MobileName ) {
			this.MobileID = MobileID;
			this.MobileName = MobileName;
		}
	}
}
