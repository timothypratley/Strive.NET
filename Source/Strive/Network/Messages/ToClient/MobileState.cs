using System;

using Strive.Math3D;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for MobileStatus.
	/// </summary>
	[Serializable]
	public class MobileState : IMessage {
		public int ObjectInstanceID;
		public EnumMobileState State;
		public Vector3D position;

		public MobileState(){}
		public MobileState( Mobile mob ) {
			this.ObjectInstanceID = mob.ObjectInstanceID;
			this.State = mob.MobileState;
			this.position = mob.Position;
		}
	}
}
