using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToServer.GameCommand
{
	/// <summary>
	/// Summary description for TargetAny.
	/// </summary>
	[Serializable]
	public class CancelSkill : IMessage	{
		public int InvokationID;	// this is so the client can cancel specific invokations
		public CancelSkill(){}
		public CancelSkill( int InvokationID ) {
			this.InvokationID = InvokationID;
		}
	}
}
