using System;
using Strive.Multiverse;
using Strive.Network.Server;

namespace Strive.Server
{
	/// <summary>
	/// A server side Avatar is a Mobile that
	/// optionally has a client associated with it.
	/// The server world contains only Avatars;
	/// no Mobiles, as all Mobiles are possesable and hence are Avatars.
	/// If a client is associated to an Avatar,
	/// that client is controlling the Mobile in question.
	/// </summary>
	public class MobileAvatar : Mobile
	{
		public Client client = null;
		public PhysicalObject target = null;
		public bool hasStruck = false;

		public MobileAvatar(
			Schema.TemplateMobileRow mobile,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( mobile, template, instance ) {
		}

		public void CombatUpdate() {
			if (  target != null ) {
				PhysicalAttack( target );
			}
		}

		public void PeaceUpdate() {

		}

		public void PhysicalAttack( PhysicalObject po ) {
			if ( po is Mobile ) {
				Mobile opponent = po as Mobile;
				// avoidance phase: ratio of Dexterity
				if ( Dexterity == 0 || Global.random.Next(100) <= opponent.Dexterity/Dexterity * 20 ) {
					// 20% chance for equal dex player to avoid
					// tell players
					return;
				}

				// hit phase: hitroll determines if you miss, hit armour, or bypass armour
				int hitroll = 80;
				if ( Global.random.Next( hitroll ) < 20 ) {
				}

				// damage phase: weapon damage + bonuses
				int damage = 10;
				opponent.HitPoints -= damage;
			} else {
				// attacking object
			}
		}
	}
}
