using System;
using Strive.Multiverse;
using Strive.Network.Server;
using Strive.Math3D;

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
		public World world = null;
		DateTime lastAttackUpdate = DateTime.Now;
		DateTime lastHealUpdate = DateTime.Now;
		DateTime lastBehaviourUpdate = DateTime.Now;
		DateTime lastMoveUpdate = DateTime.Now;

		public MobileAvatar(
			World world,
			Schema.TemplateMobileRow mobile,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( mobile, template, instance ) {
			this.world = world;
		}

		public bool IsPlayer() {
			return AreaID == 0;
		}

		public void Update() {
			if ( target != null ) {
				CombatUpdate();
			} else if ( IsPlayer() ) {
				BehaviourUpdate();
			}
			PeaceUpdate();
		}

		public void CombatUpdate() {
			if ( lastAttackUpdate - Global.now > TimeSpan.FromSeconds(3) ) {
				// combat
				lastAttackUpdate = Global.now;
				PhysicalAttack( target );
			}
		}

		public void BehaviourUpdate() {
			// continue doing whatever you were doing
			if ( Global.now - lastMoveUpdate > TimeSpan.FromSeconds( 1 ) ) {
				if ( MobileState >= EnumMobileState.Standing ) {
					Heading.Transform( Strive.Math3D.Matrix3D.FromRotation( Global.up, (float)(Global.random.NextDouble()/10.0) ) );
				}
				Vector3D velocity = Heading.GetUnit();
				switch ( MobileState ) {
					case EnumMobileState.Running:
						world.Relocate( this, Position+velocity );
						break;
					case EnumMobileState.Walking:
						velocity.Divide( 3.0F );
						world.Relocate( this, Position+velocity );
						break;
					default:
						// do nothing
						break;
				}
			}
			if ( Global.now - lastBehaviourUpdate > TimeSpan.FromSeconds( 10 ) ) {
				// change behaviour?
				lastBehaviourUpdate = Global.now;
				if ( MobileState > EnumMobileState.Incapacitated ) {
					int rand = Global.random.Next( 5 ) - 2;
					if ( rand > 1 && MobileState > EnumMobileState.Sleeping ) {
						MobileState--;
						System.Console.WriteLine( ObjectTemplateName + " changed behaviour from " + (MobileState+1) + " to " + MobileState );
					} else if ( rand < -1 && MobileState < EnumMobileState.Running ) {
						MobileState++;
						System.Console.WriteLine( ObjectTemplateName + " changed behaviour from " + (MobileState-1) + " to " + MobileState );
					}
				}
			}
		}

		public void PeaceUpdate() {
			if ( Global.now - lastHealUpdate > TimeSpan.FromSeconds( 100/Constitution ) ) {
				// EEERRR this is wrong atm
				// heal
				lastHealUpdate = Global.now;
				HitPoints++;
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
