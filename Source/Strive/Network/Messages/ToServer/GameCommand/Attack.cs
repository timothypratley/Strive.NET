using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToServer.GameCommand
{
	/// <summary>
	/// Summary description for Attack.
	/// </summary>
	[Serializable]
	public class Attack : IMessage	{
		public int targetObjectInstanceID;
		public Attack( PhysicalObject target )	{
			this.targetObjectInstanceID = target.ObjectInstanceID;
		}
	}
}
