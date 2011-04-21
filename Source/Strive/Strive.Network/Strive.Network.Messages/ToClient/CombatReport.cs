using Strive.Common;
using Strive.Model;
namespace Strive.Network.Messages.ToClient
{
    public class CombatReport
    {
        public int AttackerObjectInstanceId;
        public int TargetObjectInstanceId;
        public EnumSkill Skill;
        public float Damage;

        public CombatReport(EntityModel attacker, EnumSkill skill, EntityModel target, float damage)
        {
            AttackerObjectInstanceId = attacker.Id;
            TargetObjectInstanceId = target == null ? 0 : target.Id;
            Skill = skill;
            Damage = damage;
        }
    }
}
