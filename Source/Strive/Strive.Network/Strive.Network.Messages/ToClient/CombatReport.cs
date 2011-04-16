using Strive.Model;
namespace Strive.Network.Messages.ToClient
{
    public class CombatReport
    {
        public int AttackerObjectInstanceId;
        public int TargetObjectInstanceId;
        public EnumCombatEvent CombatEvent;
        public float Damage;

        public CombatReport(EntityModel attacker, EntityModel target, EnumCombatEvent combatEvent, float damage)
        {
            AttackerObjectInstanceId = attacker.Id;
            TargetObjectInstanceId = target == null ? 0 : target.Id;
            CombatEvent = combatEvent;
            Damage = damage;
        }
    }
}
