using Strive.Common;


namespace Strive.Network.Messages.ToServer
{
    public class UseSkill
    {
        public EnumSkill Skill;
        public int InvokationId;	// this is so the client can cancel specific invocations
        public int[] TargetPhysicalObjectIDs;

        public UseSkill(EnumSkill skill, int invokationId)
        {
            Skill = skill;
            InvokationId = invokationId;
            TargetPhysicalObjectIDs = new int[0];
        }
        public UseSkill(EnumSkill skill, int invokationId, int[] targetPhysicalObjectIDs)
        {
            Skill = skill;
            InvokationId = invokationId;
            TargetPhysicalObjectIDs = targetPhysicalObjectIDs;
        }
    }
}
