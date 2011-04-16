using Strive.Common;


namespace Strive.Network.Messages.ToServer
{
    public class UseSkill
    {
        public int SkillId;
        public int InvokationId;	// this is so the client can cancel specific invocations
        public int[] TargetPhysicalObjectIDs;

        public UseSkill(EnumSkill skillId, int invokationId)
        {
            SkillId = (int)skillId;
            InvokationId = invokationId;
            TargetPhysicalObjectIDs = new int[0];
        }
        public UseSkill(EnumSkill skillId, int invokationId, int[] targetPhysicalObjectIDs)
        {
            SkillId = (int)skillId;
            InvokationId = invokationId;
            TargetPhysicalObjectIDs = targetPhysicalObjectIDs;
        }
    }
}
