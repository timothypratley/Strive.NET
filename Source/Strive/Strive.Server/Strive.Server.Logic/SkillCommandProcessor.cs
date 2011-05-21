using System;
using Common.Logging;
using Strive.Common;
using Strive.Data.Events;
using Strive.Model;
using Strive.Network.Messages.ToServer;
using Strive.Network.Messaging;
using Strive.Server.DB;


namespace Strive.Server.Logic
{
    // TODO: should this just be a partial class instead of using extension methods??
    public static class SkillCommandProcessor
    {
        static ILog Log = LogManager.GetCurrentClassLogger();

        public static void ProcessUseSkill(this World world, ClientConnection client, UseSkill message)
        {
            var source = client.Avatar as CombatantModel;
            if (source == null)
            {
                client.LogMessage("Requested a skill without an avatar.");
                return;
            }

            Schema.EnumSkillRow esr = Global.Schema.EnumSkill.FindByEnumSkillID((int)message.Skill);
            if (esr == null)
            {
                client.LogMessage("Requested an invalid skill " + message.Skill);
                return;
            }

            EntityModel target;
            switch ((EnumTargetType)esr.EnumTargetTypeID)
            {
                case EnumTargetType.TargetSelf:
                    target = source;
                    break;
                case EnumTargetType.TargetMobile:
                    if (message.TargetPhysicalObjectIDs.Length == 0)
                    {
                        world.LogMessage(source, "No target specified, this skill may only be used on Mobiles.");
                        return;
                    }
                    var o = world.History.Head.Entity.TryFind(message.TargetPhysicalObjectIDs[0]);
                    target = o == null ? null : o.Value;
                    if (target == null)
                    {
                        world.LogMessage(source, "Target " + message.TargetPhysicalObjectIDs[0] + " not found.");
                        return;
                    }
                    break;
                default:
                    world.LogMessage(source, "That skill has an unsupported target type " + esr.EnumTargetTypeID);
                    Log.Error("Unhandled target type " + esr.EnumTargetTypeID + " for skill " + esr.EnumSkillID + " " + esr.EnumSkillName);
                    return;
            }


            if (source.ActivatingSkill != EnumSkill.None)         // queue the request for later.
                world.Apply(new EntityUpdateEvent(
                    source.EnqueueSkill(message.Skill, target),
                    "Enqueuing Skill " + message.Skill));
            else if (esr.LeadTime <= 0)                 // process it now
                world.UseSkillNow(source, esr, target);
            else                                        // process it later, after lead-time has elapsed
                world.Apply(new EntityUpdateEvent(
                    source.StartSkill(message.Skill, target, Global.Now, TimeSpan.FromSeconds(esr.LeadTime)),
                    "Activating Skill " + message.Skill));
        }

        public static void ProcessCancelSkill(this World world, ClientConnection client, CancelSkill message)
        {
            var avatar = client.Avatar as CombatantModel;
            if (avatar == null)
            {
                client.LogMessage("Canceled a skill invocation, but don't have an avatar.");
                return;
            }

            // If already performing invocation, just cancel it
            bool found = false;
            if (avatar.ActivatingSkill != EnumSkill.None)
            // TODO:
            //&& avatar.ActivatingSkill.InvokationId == message.InvokationId)
            {
                world.Apply(new EntityUpdateEvent(avatar.StartSkill(EnumSkill.None, null, Global.Now, TimeSpan.FromSeconds(0)),
                    "Started using skill"));
                found = true;
            }
            else
            {
                // TODO: search for it in queued skill invocations
                // just generate a new queue with the invocation missing
                /*
                var newQueue = new Queue<UseSkill>();
                foreach (UseSkill m in avatar.SkillQueue)
                {
                    if (m.InvokationId == message.InvokationId)
                    {
                        // don't add it
                        found = true;
                    }
                    else
                        newQueue.Enqueue(m);
                }
                avatar.SkillQueue = newQueue;
                 */
                avatar.EmptyQueue();
            }
            if (found)
                client.LogMessage("Successfully canceled invocation " + message.InvokationId);
            else
                client.LogMessage("Failed to cancel invocation " + message.InvokationId);
        }

        public static void UseSkillNow(this World world, CombatantModel source, Schema.EnumSkillRow skill, EntityModel target)
        {
            if (source.MobileState == EnumMobileState.Dead || source.MobileState == EnumMobileState.Incapacitated)
            {
                world.LogMessage(source, "Unable to use " + skill.EnumSkillName + " while " + source.MobileState);
                return;
            }

            /* TODO: skill specific states
            if ( skill.EnumMobileState > caster.MobileState ) {
                caster.SendLog( "Not while " + caster.MobileState.Name );
            }
             */

            if ((source.Position - target.Position).Length > skill.Range)
            {
                world.LogMessage(source, target.Name + " is out of range");
                return;
            }

            if (skill.EnergyCost > source.Energy)
            {
                world.LogMessage(source, "Not enough energy to use " + skill.EnumSkillName + ", requires " + skill.EnergyCost);
                return;
            }
            
            // deduct energy regardless
            // TODO: can remove the explicit cast here by using generics
            source = (CombatantModel)source.WithEnergyChange(-skill.EnergyCost);

            bool succeeds = source.Succeeds(skill);

            bool hits = source.Hits(target, skill);

            bool avoids = target.Avoids(source, skill);

            // successful casting affects affinity with the elements
            if (succeeds && hits && !avoids)
            {
                // TODO: can remove the explicit cast here by using generics
                source = (CombatantModel)source.WithAffinityChange(
                    skill.AirAffinity / 1000f,
                    skill.EarthAffinity / 1000f,
                    skill.FireAffinity / 1000f,
                    skill.LifeAffinity / 1000f,
                    skill.WaterAffinity / 1000f);

                // TODO: just get the damage number instead??

                switch ((EnumActivationType)skill.EnumActivationTypeID)
                {
                    case EnumActivationType.AttackSpell:
                        target = target.MagicallyDamagedBy(source, skill);
                        break;
                    case EnumActivationType.Enchantment:
                        target = target.EnchantedBy(source, skill);
                        break;
                    case EnumActivationType.Glamour:
                        source.Activates(skill, target);
                        break;
                    case EnumActivationType.HealingSpell:
                        target = target.HealedBy(source, skill);
                        break;
                    case EnumActivationType.Skill:
                        target = target.DamagedBy(source, skill);
                        target = target.AffectedBy(source, skill);
                        source.Activates(skill, target);
                        break;
                    case EnumActivationType.Sorcery:
                        break;
                    default:
                        world.LogMessage(source, "That skill does not work yet, contact admin.");
                        Log.Error("Unhandled activation type " + (EnumActivationType)skill.EnumActivationTypeID + " for skill " + skill.EnumSkillName);
                        break;
                }
            }

            source = source.FinishSkill();
            string description = avoids ? "Avoided" : !hits ? "Missed" : !succeeds ? "Failed" : "Success";
            world.Apply(new SkillEvent(source, (EnumSkill)skill.EnumSkillID, target, succeeds, hits, avoids, description));
        }

        public static bool Avoids(this EntityModel target, CombatantModel source, Schema.EnumSkillRow skill)
        {
            var opponent = target as CombatantModel;
            if (opponent == null || opponent != source)
                return false;
            else if ((EnumActivationType)skill.EnumActivationTypeID == EnumActivationType.Skill)
                // physical attack: ratio of Dexterity
                // 20% chance for equal dexterity player to avoid
                return (source.Dexterity == 0 || Global.Rand.Next(100) <= opponent.Dexterity / source.Dexterity * 20);
            else
                // magical attack: Dexterity
                return (Global.Rand.Next(100) <= opponent.Dexterity);
        }

        public static bool Succeeds(this CombatantModel source, Schema.EnumSkillRow skill)
        {
            return GetCompetancy(source, skill) < Global.Rand.Next(100);
        }

        public static bool Hits(this CombatantModel source, EntityModel target, Schema.EnumSkillRow skill)
        {
            // TODO: use actual hitroll
            int hitroll = 80;

            // %20 chance to miss for weapon with 100 hit-roll
            return (Global.Rand.Next(hitroll) >= 20);
        }

        public static EntityModel MagicallyDamagedBy(this EntityModel target, CombatantModel source, Schema.EnumSkillRow skill)
        {
            float damage = skill.EnergyCost * 2;
            damage += Math.Max(skill.AirAffinity * source.Affinity.Air / target.Affinity.Air, 0);
            damage += Math.Max(skill.EarthAffinity * source.Affinity.Earth / target.Affinity.Earth, 0);
            damage += Math.Max(skill.FireAffinity * source.Affinity.Fire / target.Affinity.Fire, 0);
            damage += Math.Max(skill.LifeAffinity * source.Affinity.Life / target.Affinity.Life, 0);
            damage += Math.Max(skill.WaterAffinity * source.Affinity.Water / target.Affinity.Water, 0);

            var opponent = target as CombatantModel;
            if (opponent != null)
                damage *= source.Cognition / opponent.Willpower;

            return target.WithHealthChange(-damage);
        }

        public static EntityModel DamagedBy(this EntityModel target, CombatantModel source, Schema.EnumSkillRow skill)
        {
            // TODO: use actual damage and hitroll
            float damage = skill.EnergyCost * 2;
            int hitroll = 80;

            // hit armor, or bypass armor
            // TODO: use armor rating
            if (Global.Rand.Next(hitroll) < 50)
                damage -= 8; // opponent.ArmorRating
            if (damage < 0)
                damage = 0;

            var opponent = target as CombatantModel;
            if (opponent != null)
                damage *= source.Strength / opponent.Constitution;

            target = target.WithHealthChange(-damage);

            if (target.MobileState != EnumMobileState.Dead
                && target.MobileState != EnumMobileState.Incapacitated
                && target.MobileState != EnumMobileState.Fighting)
            {
                target = target.WithState(EnumMobileState.Fighting);
                if (opponent != null && opponent.Target == null)
                    target = opponent.WithTarget(source);
            }

            return target;
        }

        public static EntityModel HealedBy(this EntityModel target, EntityModel source, Schema.EnumSkillRow skill)
        {
            return target.WithHealthChange(skill.EnergyCost * 2);
        }

        public static EntityModel AffectedBy(this EntityModel target, EntityModel source, Schema.EnumSkillRow skill)
        {
            // TODO: add affects
            return target;
        }

        public static EntityModel EnchantedBy(this EntityModel target, EntityModel source, Schema.EnumSkillRow skill)
        {
            // TODO: add enchantments
            return target;
        }

        public static double GetCompetancy(CombatantModel combatant, Schema.EnumSkillRow skill)
        {
            Schema.MobileHasSkillRow mhs = Global.Schema.MobileHasSkill.FindByTemplateObjectIDEnumSkillID(
                // TODO: do we even want templates?
                //TemplateObjectId,
                combatant.Id,
                skill.EnumSkillID);
            if (mhs != null)
                return mhs.Rating;
            return 0;
        }

        public static void Activates(this CombatantModel source, Schema.EnumSkillRow skill, EntityModel target)
        {

            switch ((EnumSkill)skill.EnumSkillID)
            {
                case EnumSkill.Kill:
                    DoKill(source, target);
                    break;
                case EnumSkill.Kick:
                    DoKick(source, target);
                    break;
                default:
                    Log.Error("Unhandled Skill " + skill.EnumSkillName + ", SkillID " + skill.EnumSkillID);
                    break;
            }
        }

        public static CombatantModel FinishSkill(this CombatantModel source)
        {
            return source.StartSkill(EnumSkill.None, null, Global.Now, TimeSpan.MinValue);
        }

        public static void DoKill(CombatantModel source, EntityModel target)
        {
            //source.Attack(target);
        }

        public static void DoKick(CombatantModel source, EntityModel target)
        {
            //source.Kick(target);
        }
    }
}
