using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToServer.GameCommand
{
	/// <summary>
	/// Summary description for TargetAny.
	/// </summary>
	[Serializable]
	public class UseSkill : IMessage	{
		public int SkillID;
		public int [] TargetPhysicalObjectIDs;
		public UseSkill(){}
		public UseSkill( EnumSkill SkillID ) {
			this.SkillID = (int)SkillID;
			this.TargetPhysicalObjectIDs = new int[0];
		}
		public UseSkill( EnumSkill SkillID, int [] TargetPhysicalObjectIDs )	{
			this.SkillID = (int)SkillID;
			this.TargetPhysicalObjectIDs = TargetPhysicalObjectIDs;
		}
	}
}
