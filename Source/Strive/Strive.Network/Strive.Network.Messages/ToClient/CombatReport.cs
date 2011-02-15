using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    public class CombatReport : IMessage
    {
        public int AttackerObjectInstanceId;
        public int TargetObjectInstanceId;
        public EnumCombatEvent CombatEvent;
        public float Damage;

        public CombatReport(Mobile attacker, PhysicalObject target, EnumCombatEvent combatEvent, float damage)
        {
            AttackerObjectInstanceId = attacker.ObjectInstanceId;
            TargetObjectInstanceId = target == null ? 0 : target.ObjectInstanceId;
            CombatEvent = combatEvent;
            Damage = damage;
        }
    }
}
