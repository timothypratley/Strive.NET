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
		DateTime lastAttack = DateTime.Now;
		DateTime lastHeal = DateTime.Now;
		DateTime lastAI = DateTime.Now;

		public MobileAvatar(
			Schema.TemplateMobileRow mobile,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( mobile, template, instance ) {
		}

		public void Update() {
			if ( target != null ) {
				if ( lastAttack - Global.now > TimeSpan.FromSeconds(3) ) {
					// combat
					PhysicalAttack( target );
				}
			} else {
				if ( lastAI - Global.now > TimeSpan.FromSeconds( 1 ) ) {
					// wander
				}
			}
			if ( lastHeal - Global.now > TimeSpan.FromSeconds( 1 ) ) {
				// heal
			}
			
		}

		public void PeaceUpdate() {
			if ( MobileState == EnumMobileState.Dead ) {
				// respawn!
			} else if ( MobileState == EnumMobileState.Incapacitated ) {
				HitPoints -= 0.1F;
			} else if ( MobileState == EnumMobileState.Sleeping ) {
				HitPoints += Constitution/10.0F;
			} else if ( MobileState == EnumMobileState.Resting ) {
				HitPoints += Constitution/40.0F;
			}
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
				Item item = target as Item;
				int damage = 10;
				item.HitPoints -= damage;
				if ( item.HitPoints <= 0 ) {
					// omg j00 destoryed teh item!
				}
			}
		}
	}
}
