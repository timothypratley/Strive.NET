using System;
using Strive.Network.Server;
using Strive.Multiverse;

namespace Strive.Server.Shared
{
	/// <summary>
	/// Summary description for GameCommandProcessor.
	/// </summary>
	public class SkillCommandProcessor
	{
		public static void ProcessUseSkill( Client client, Strive.Network.Messages.ToServer.GameCommand.UseSkill message ) {
			Schema.EnumSkillRow esr = Global.multiverse.EnumSkill.FindByEnumSkillID( (int)message.SkillID );
			if ( esr.LeadTime <= 0 ) {
				// process it now
				UseSkillNow( client, message );
			} else {
				// process it later, after leadtime is elapsed
				MobileAvatar ma = client.Avatar as MobileAvatar;
				ma.activatingSkill = message;
				ma.activatingSkillTimestamp = Global.now;
				ma.activatingSkillLeadTime = TimeSpan.FromSeconds( esr.LeadTime );
			}
		}

		public static void UseSkillNow( Client client, Strive.Network.Messages.ToServer.GameCommand.UseSkill message ) {
			Schema.EnumSkillRow esr = Global.multiverse.EnumSkill.FindByEnumSkillID( (int)message.SkillID );
			MobileAvatar caster = client.Avatar as MobileAvatar;
			MobileAvatar target;

			// casting affects affinity with the elements
			caster.AffinityAir += esr.AirAffinity/1000;
			caster.AffinityEarth += esr.EarthAffinity/1000;
			caster.AffinityFire += esr.FireAffinity/1000;
			caster.AffinityLife += esr.LifeAffinity/1000;
			caster.AffinityWater += esr.WaterAffinity/1000;
			switch ( (EnumTargetType)esr.EnumTargetTypeID ) {
				case EnumTargetType.TargetSelf:
					TargetSkill( caster, caster, esr );
					break;
				case EnumTargetType.TargetMobile:
					target = (MobileAvatar)Global.world.physicalObjects[ message.TargetPhysicalObjectIDs[0] ];
					if ( (caster.Position - target.Position).GetMagnitude() > esr.Range ) {
						// target is out of range
						Global.log.LogMessage( "Target is out of range" );
						return;
					}
					TargetSkill( caster, target, esr );
					break;
				default:
					throw new Exception( "n0rty n0rty, unhandled targettype" );
			}
		}

		public static void TargetSkill( MobileAvatar caster, MobileAvatar target, Schema.EnumSkillRow esr ) {
			switch ( (EnumActivationType)esr.EnumActivationTypeID ) {
				case EnumActivationType.AttackSpell:
					float damage = esr.EnergyCost;
					damage += esr.AirAffinity * caster.AffinityAir / target.AffinityAir;
					damage += esr.EarthAffinity * caster.AffinityEarth / target.AffinityEarth;
					damage += esr.FireAffinity * caster.AffinityFire / target.AffinityFire;
					damage += esr.LifeAffinity * caster.AffinityLife / target.AffinityLife;
					damage += esr.WaterAffinity * caster.AffinityWater / target.AffinityWater;
					caster.MagicalAttack( target, damage );
					Global.log.LogMessage( "Attack spell cast!" );
					break;
				case EnumActivationType.Enchantment:
					break;
				case EnumActivationType.Glamour:
					break;
				case EnumActivationType.HealingSpell:
					break;
				case EnumActivationType.Skill:
					break;
				case EnumActivationType.Sorcery:
					break;
				default:
					throw new Exception( "n0rty n0rty, unhandled activationtype" );
			}
		}

		public static void PhysicalAttack( MobileAvatar attacker, MobileAvatar target, int damage ) {

		}
	}
}
