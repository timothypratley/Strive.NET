using System;
using System.Collections;

using Strive.Multiverse;
using Strive.Network.Server;
using Strive.Network.Messages;
using Strive.Math3D;
using Strive.Logging;

namespace Strive.Server.Shared
{
	/// <summary>
	/// A server side Avatar is a Mobile that
	/// optionally has a client associated with it.
	/// The server world contains only Avatars;
	/// no Mobiles, as all Mobiles are possesable and hence are Avatars.
	/// If a client is associated to an Avatar,
	/// that client is controlling the Mobile in question.
	/// </summary>
	public class MobileAvatar : Mobile {
		public Client client = null;
		public World world = null;
		public DateTime lastAttackUpdate = Global.now;
		public DateTime lastHealUpdate = Global.now;
		public DateTime lastBehaviourUpdate = Global.now;
		public DateTime lastMoveUpdate = Global.now;

		// if fighting someone or something
		public PhysicalObject target = null;

		// if in a party
		public Party party = null;

		// currently invoking a skill
		public Strive.Network.Messages.ToServer.GameCommand.UseSkill activatingSkill = null;
		public DateTime activatingSkillTimestamp = Global.now;
		public TimeSpan activatingSkillLeadTime = TimeSpan.FromSeconds(0);

		// any queued up skills to be executed after the current one
		public Queue skillQueue = new Queue();

		// todo: put these in the database schema
		public float AffinityAir = 0;
		public float AffinityEarth = 0;
		public float AffinityFire = 0;
		public float AffinityLife = 0;
		public float AffinityWater = 0;

		public MobileAvatar(
			World world,
			Schema.TemplateMobileRow mobile,
			Schema.TemplateObjectRow template,
			Schema.ObjectInstanceRow instance
			) : base( mobile, template, instance ) {
			this.world = world;
		}

		public void SetMobileState( EnumMobileState ms ) {
			MobileState = ms;
			// TODO: this will prolly change if we use anything more
			// advance than stick to ground.
			// changing state may have moved the mobile.
			float altitude = world.AltitudeAt( Position.X, Position.Z ) + CurrentHeight/2;
			Position.Y = altitude;

			// NB: MobileState message has position info
			// as it is likely that this will have changed
			world.InformNearby( this, new Strive.Network.Messages.ToClient.MobileState( this ) );
		}


		public void SendLog( string message ) {
			if ( client == null ) {
				Log.WarningMessage( ObjectInstanceID + ", no client: " + message );
			} else {
				client.Send(
					new Strive.Network.Messages.ToClient.LogMessage( message ) );
			}
		}


		public void Update() {
			// check for activating skills
			if ( activatingSkill != null ) {
				if ( activatingSkillTimestamp + activatingSkillLeadTime <= Global.now ) {
					SkillCommandProcessor.UseSkillNow( this, activatingSkill );
					activatingSkill = null;
				}
			} else {
				// check for queued skills
				if ( skillQueue.Count > 0 ) {
					activatingSkill = (Strive.Network.Messages.ToServer.GameCommand.UseSkill)skillQueue.Dequeue();
				}
			}

			if ( target != null ) {
				CombatUpdate();
			} else if ( !IsPlayer ) {
				BehaviourUpdate();
			} else {
				if (
					Global.now - lastMoveUpdate > TimeSpan.FromSeconds( 1 )
					&& ( MobileState == EnumMobileState.Running
					|| MobileState == EnumMobileState.Walking)
				) {
					SetMobileState( EnumMobileState.Standing );
				}
			}
			HealUpdate();
		}

		public void CombatUpdate() {
			if ( Global.now - lastAttackUpdate > TimeSpan.FromSeconds(3) ) {
				// combat
				lastAttackUpdate = Global.now;
				PhysicalAttack( target );
			}
		}

		public void BehaviourUpdate() {
			// continue doing whatever you were doing
			if ( Global.now - lastMoveUpdate > TimeSpan.FromSeconds( 1 ) ) {
				if ( MobileState >= EnumMobileState.Standing ) {
					Rotation.Y += (float)(Global.random.NextDouble()*40-20);
					while ( Rotation.Y < 0 ) Rotation.Y += 360;
					while ( Rotation.Y >= 360 ) Rotation.Y -= 360;
				}
				Vector3D velocity = Helper.GetHeadingFromRotation( Rotation );
				switch ( MobileState ) {
					case EnumMobileState.Running:
						// TODO: using timing, not constant values
						world.Relocate( this, (Position+3*velocity/10), Rotation );
						break;
					case EnumMobileState.Walking:
						world.Relocate( this, (Position+velocity/10), Rotation );
						break;
					default:
						// do nothing
						break;
				}
			}
			if ( Global.now - lastBehaviourUpdate > TimeSpan.FromSeconds( 3 ) ) {
				// change behaviour?
				lastBehaviourUpdate = Global.now;
				if ( MobileState > EnumMobileState.Incapacitated ) {
					int rand = Global.random.Next( 5 ) - 2;
					if ( rand > 1 && MobileState > EnumMobileState.Sleeping ) {
						SetMobileState( MobileState-1 );
						//Log.LogMessage( TemplateObjectName + " changed behaviour from " + (MobileState+1) + " to " + MobileState + "." );
					} else if ( rand < -1 && MobileState < EnumMobileState.Running ) {
						SetMobileState( MobileState+1 );
						//Log.LogMessage( TemplateObjectName + " changed behaviour from " + (MobileState-1) + " to " + MobileState + "." );
					}
				}
			}
		}

		public void HealUpdate() {
			if ( Global.now - lastHealUpdate > TimeSpan.FromSeconds( 1 ) ) {
				lastHealUpdate = Global.now;
				if ( MobileState == EnumMobileState.Incapacitated ) {
					HitPoints -= 0.5F;
					Energy -= 0.5F;
				} else if ( MobileState == EnumMobileState.Sleeping ) {
					HitPoints += Constitution/10.0F;
					if ( HitPoints > MaxHitPoints ) {
						HitPoints = MaxHitPoints;
					}
					Energy += Constitution/10.0F;
					if ( Energy > MaxEnergy ) {
						Energy = MaxEnergy;
					}
				} else if ( MobileState == EnumMobileState.Resting ) {
					HitPoints += Constitution/40.0F;
					if ( HitPoints > MaxHitPoints ) {
						HitPoints = MaxHitPoints;
					}
					Energy += Constitution/40.0F;
					if ( Energy > MaxEnergy ) {
						Energy = MaxEnergy;
					}
				}
			}

			// we might have died, or recovered
			UpdateState();
		}

		public void Damage( float damage ) 
		{
			HitPoints -= damage;
			UpdateState();
		}

		public void Attack( PhysicalObject target ) {
			this.target = target;
			MobileState = EnumMobileState.Fighting;
			world.InformNearby(
				this,
				new Strive.Network.Messages.ToClient.CombatReport(
					this, target, EnumCombatEvent.Attacks, 0
				)
			);
		}

		public void Kick( PhysicalObject target ) {
			// TODO: would be nice to have a baseclass physical object with damage function,
			// but needs multiple inheritance
			target.HitPoints -= 20;
			world.InformNearby(
				this,
				new Strive.Network.Messages.ToClient.CombatReport(
					this, target, EnumCombatEvent.Hits, 20
				)
			);
			((MobileAvatar)target).UpdateState();
		}

		public void PhysicalAttack( PhysicalObject po ) {
			// TODO use the real range of kill
			if ( (Position - po.Position).GetMagnitude() > 100 ) {
				// target is out of range
				SendLog( target.TemplateObjectName + " is out of range." );
				return;
			}
			if ( po is MobileAvatar ) 
			{
				MobileAvatar opponent = po as MobileAvatar;

				// if not already in a fight, your opponent automatically
				// fights back
				if ( opponent.target == null ) {
					opponent.target = this;
				}

				// avoidance phase: ratio of Dexterity
				if ( Dexterity == 0 || Global.random.Next(100) <= opponent.Dexterity/Dexterity * 20 ) {
					// 20% chance for equal dex player to avoid
					world.InformNearby(
						this,
						new Strive.Network.Messages.ToClient.CombatReport(
							this, target, EnumCombatEvent.Avoids, 0 )
					);
					return;
				}

				// hit phase: hitroll determines if you miss, hit armour, or bypass armour
				int hitroll = 80;
				int attackroll = Global.random.Next( hitroll );

				if ( attackroll < 20 ) {
					// ~ %20 chance to miss for weapon with 100 hitroll
					world.InformNearby(
						this,
						new Strive.Network.Messages.ToClient.CombatReport(
							this, target, EnumCombatEvent.Misses, 0 )
					);
				}

				// damage phase: weapon damage + bonuses
				int damage = 10;
				if ( attackroll < 50 ) {
					damage -= 8; // opponent.ArmourRating
				}
				if ( damage < 0 ) damage = 0;

				damage *= Strength/opponent.Constitution;
				opponent.HitPoints -= damage;
				opponent.MobileState = EnumMobileState.Fighting;
				opponent.UpdateState();
				world.InformNearby(
					this,
					new Strive.Network.Messages.ToClient.CombatReport(
						this, target, EnumCombatEvent.Hits, damage )
				);
			} else if ( po is Item ) {
				// attacking object
				Item item = target as Item;
				int damage = 10;
				item.HitPoints -= damage;
				world.InformNearby(
					this,
					new Strive.Network.Messages.ToClient.CombatReport(
						this, target, EnumCombatEvent.Hits, damage )
				);

				if ( item.HitPoints <= 0 ) {
					// omg j00 destoryed teh item!
					world.Remove( item );
				}
			} else {
				throw new Exception( "ERROR: attacking a " + po.GetType().ToString() + " " + po );
			}
		}

		public void MagicalAttack( PhysicalObject po, float damage ) {
			if ( po is MobileAvatar ) {
				MobileAvatar opponent = po as MobileAvatar;

				// if not already in a fight, your opponent automatically
				// fights back
				if ( opponent.target == null ) {
					opponent.target = this;
				}

				// avoidance phase: Dexterity
				if ( Global.random.Next(100) <= opponent.Dexterity ) {
					world.InformNearby(
						this,
						new Strive.Network.Messages.ToClient.CombatReport(
						this, target, EnumCombatEvent.Avoids, 0 )
					);
					return;
				}

				// damage phase
				opponent.HitPoints -= damage * Cognition/opponent.Willpower;
				opponent.UpdateState();
				world.InformNearby(
					this,
					new Strive.Network.Messages.ToClient.CombatReport(
					this, target, EnumCombatEvent.Hits, damage )
				);
			} else if ( po is Item ) {
				// attacking object
				Item item = target as Item;
				item.HitPoints -= damage;
				world.InformNearby(
					this,
					new Strive.Network.Messages.ToClient.CombatReport(
					this, target, EnumCombatEvent.Hits, damage )
					);

				if ( item.HitPoints <= 0 ) {
					// omg j00 destoryed teh item!
					world.Remove( item );
				}
			} else {
				throw new Exception( "ERROR: attacking a " + po.GetType().ToString() + " " + po );
			}
		}

		public void UpdateState() {
			if ( MobileState >= EnumMobileState.Sleeping ) {
				if ( HitPoints <= 0 ) {
					HitPoints = 0;
					SetMobileState( EnumMobileState.Incapacitated );
				}
			} else if ( MobileState == EnumMobileState.Incapacitated ) {
				if ( HitPoints <= -50 ) {
					Death();
				} else if ( HitPoints > 0 ) {
					SetMobileState( EnumMobileState.Resting );
				}
			}
		}

		public void Death() {
			// RIP
			SetMobileState( EnumMobileState.Dead );

			if ( IsPlayer ) {
				// respawn!
				HitPoints = MaxHitPoints;

				// TODO: where should we respawn?
				Position.Set( 0, 0, 0 );
				Rotation.Set( 0, 0, 0 );
				world.Relocate( this, Position, Rotation );
				
				// set resting in new location to let everyone know
				SetMobileState( EnumMobileState.Resting );
			} else {
				// TODO: should probably stay around as a corpse
				// world.Remove( this );
				// but we need some decay/repop code
			}
		}

		public float CurrentHeight {
			get {
				if ( MobileState <= EnumMobileState.Resting ) {
					return 0;
				} else {
					return Height;
				}
			}
		}

		public float GetCompetancy( EnumSkill skill ) {
			Schema.MobileHasSkillRow mhs = Global.multiverse.MobileHasSkill.FindByTemplateObjectIDEnumSkillID( TemplateObjectID, (int)skill );
			if ( mhs != null ) {
				return (float)mhs.Rating;
			} else {
				return 0;
			}
		}
	}
}
