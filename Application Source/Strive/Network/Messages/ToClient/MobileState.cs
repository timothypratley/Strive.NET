using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for MobileStatus.
	/// </summary>
	[Serializable]
	public class MobileState : IMessage {
		public int ObjectInstanceID;
		public int State;
		public MobileState(){}
		public MobileState( Mobile mob ) {
			this.ObjectInstanceID = mob.ObjectInstanceID;
			this.State = (int)mob.MobileState;
		}
	}
}
