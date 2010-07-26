using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for TargetAny.
	/// </summary>
	[Serializable]
	public class UseSkill : IMessage	{
		public int SkillID;
		public int InvokationID;	// this is so the client can cancel specific invokations
		public int [] TargetPhysicalObjectIDs;
		public UseSkill(){}
		public UseSkill( EnumSkill SkillID, int InvokationID ) {
			this.SkillID = (int)SkillID;
			this.InvokationID = InvokationID;
			this.TargetPhysicalObjectIDs = new int[0];
		}
		public UseSkill( EnumSkill SkillID, int InvokationID, int [] TargetPhysicalObjectIDs )	{
			this.SkillID = (int)SkillID;
			this.InvokationID = InvokationID;
			this.TargetPhysicalObjectIDs = TargetPhysicalObjectIDs;
		}
	}
}
