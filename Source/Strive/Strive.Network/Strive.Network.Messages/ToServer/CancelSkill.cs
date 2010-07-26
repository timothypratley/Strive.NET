using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToServer
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
