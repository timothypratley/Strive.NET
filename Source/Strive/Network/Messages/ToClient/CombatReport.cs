using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for CombatReport.
	/// </summary>
	[Serializable]
	public class CombatReport : IMessage {
		public int attackerObjectInstanceID;
		public int targetObjectInstanceID;
		public EnumCombatEvent combat_event;
		public float damage;
		public CombatReport(){}
		public CombatReport( Mobile attacker, PhysicalObject target, EnumCombatEvent combat_event, float damage ) {
			this.attackerObjectInstanceID = attacker.ObjectInstanceID;
			if ( target == null ) {
				this.targetObjectInstanceID = 0;
			} else {
				this.targetObjectInstanceID = target.ObjectInstanceID;
			}
			this.combat_event = combat_event;
			this.damage = damage;
		}
	}
}
