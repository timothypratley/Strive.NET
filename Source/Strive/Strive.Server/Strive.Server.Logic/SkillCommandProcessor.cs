using System;
using System.Collections.Generic;
using Common.Logging;
using Strive.Common;
using Strive.Data.Events;
using Strive.Model;
using Strive.Network.Messages.ToServer;
using Strive.Network.Messaging;
using Strive.Server.DB;


namespace Strive.Server.Logic
{
    public class SkillCommandProcessor
    {
        static ILog Log = LogManager.GetCurrentClassLogger();

        public static void ProcessUseSkill(World world, ClientConnection client, UseSkill message)
        {
            var avatar = client.Avatar as Avatar;
            if (avatar == null)
            {
                client.LogMessage("Requested a skill, but doesn't have an avatar.");
                return;
            }
            Schema.EnumSkillRow esr = Global.Schema.EnumSkill.FindByEnumSkillID((int)message.SkillId);
            if (esr == null)
            {
                client.LogMessage("Requested an invalid skill " + message.SkillId);
                return;
            }

            // If already performing a skill invocation, just queue the request for later.
            if (avatar.ActivatingSkill != null)
            {
                avatar.SkillQueue.Enqueue(message);
                return;
            }

            if (esr.LeadTime <= 0)
            {
                // process it now
                UseSkillNow(world, avatar, message);
            }
            else
            {
                // process it later, after lead-time has elapsed
                avatar.ActivatingSkill = message;
                avatar.ActivatingSkillTimestamp = Global.Now;
                avatar.ActivatingSkillLeadTime = TimeSpan.FromSeconds(esr.LeadTime);
            }
        }

        public static void ProcessCancelSkill(World world, ClientConnection client, CancelSkill message)
        {
            var avatar = client.Avatar as Avatar;
            if (avatar == null)
            {
                client.LogMessage("Canceled a skill invocation, but don't have an avatar.");
                return;
            }

            // If already performing invocation, just cancel it
            bool found = false;
            if (avatar.ActivatingSkill != null && avatar.ActivatingSkill.InvokationId == message.InvokationId)
            {
                avatar.ActivatingSkill = null;
                found = true;
            }
            else
            {
                // search for it in queued skill invocations
                // just generate a new queue with the invocation missing
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
            }
            if (found)
                client.LogMessage("Successfully canceled invocation " + message.InvokationId);
            else
                client.LogMessage("Failed to cancel invocation " + message.InvokationId);
        }

        public static void UseSkillNow(World world, Avatar caster, UseSkill message)
        {
            Schema.EnumSkillRow esr = Global.Schema.EnumSkill.FindByEnumSkillID((int)message.SkillId);
            if (esr == null)
            {
                caster.SendLog("Requested an invalid skill " + message.SkillId);
                return;
            }

            if (esr.EnergyCost > caster.Energy)
            {
                caster.SendLog("Not enough energy to use " + esr.EnumSkillName + ", require " + esr.EnergyCost + ".");
                return;
            }

            // TODO: make this work
            /*
            if ( esr.EnumMobileState > caster.MobileState ) {
                caster.SendLog( "Not while " + caster.MobileState.Name );
            }
            */

            Avatar target;
            switch ((EnumTargetType)esr.EnumTargetTypeID)
            {
                case EnumTargetType.TargetSelf:
                    target = caster;
                    break;
                case EnumTargetType.TargetMobile:
                    if (message.TargetPhysicalObjectIDs.Length == 0)
                    {
                        caster.SendLog("No target specified, this skill may only be used on Mobiles.");
                        return;
                    }
                    target = (Avatar)world.PhysicalObjects[message.TargetPhysicalObjectIDs[0]];
                    if (target == null)
                    {
                        caster.SendLog("Target " + message.TargetPhysicalObjectIDs[0] + " not found.");
                        return;
                    }
                    if ((caster.Position - target.Position).Length > esr.Range)
                    {
                        // target is out of range
                        caster.SendLog(target.Name + " is out of range");
                        return;
                    }
                    break;
                default:
                    caster.SendLog("That skill does not work yet, contact admin.");
                    Log.Error("Unhandled target type " + esr.EnumTargetTypeID + " for skill " + esr.EnumSkillID);
                    return;
            }

            TargetSkill(world, caster, esr, target);
        }

        public static void TargetSkill(World world, Avatar source, Schema.EnumSkillRow esr, Avatar target)
        {
            var skill = (EnumSkill)esr.EnumSkillID;
            // test adeptness
            float competancy = GetCompetancy(source, skill);
            double roll = Global.Rand.NextDouble();
            bool succeeds = competancy < roll * 100;

            // deduct energy regardless
            source = (Avatar)source.WithEnergyChange(-esr.EnergyCost);

            // successful casting affects affinity with the elements
            if (succeeds)
            {
                // TODO: can remove the explicit cast here by using generics
                source = (Avatar)source.WithAffinityChange(
                    esr.AirAffinity / 1000f,
                    esr.EarthAffinity / 1000f,
                    esr.FireAffinity / 1000f,
                    esr.LifeAffinity / 1000f,
                    esr.WaterAffinity / 1000f);

                switch ((EnumActivationType)esr.EnumActivationTypeID)
                {
                    case EnumActivationType.AttackSpell:
                        float damage = esr.EnergyCost;
                        damage += esr.AirAffinity * source.Affinity.Air / target.Affinity.Air;
                        damage += esr.EarthAffinity * source.Affinity.Earth / target.Affinity.Earth;
                        damage += esr.FireAffinity * source.Affinity.Fire / target.Affinity.Fire;
                        damage += esr.LifeAffinity * source.Affinity.Life / target.Affinity.Life;
                        damage += esr.WaterAffinity * source.Affinity.Water / target.Affinity.Water;
                        source.MagicalAttack(target, damage, skill);
                        Log.Info("Attack spell cast!");
                        break;
                    case EnumActivationType.Enchantment:
                        break;
                    case EnumActivationType.Glamour:
                        break;
                    case EnumActivationType.HealingSpell:
                        break;
                    case EnumActivationType.Skill:
                        if (esr.EnumSkillID == (int)EnumSkill.Kill)
                            DoKill(source, target);
                        else if (esr.EnumSkillID == (int)EnumSkill.Kick)
                            DoKick(source, target);
                        else
                            Log.Error("Unhandled SkillID " + esr.EnumSkillID);
                        break;
                    case EnumActivationType.Sorcery:
                        break;
                    default:
                        source.SendLog("That skill does not work yet, contact admin.");
                        Log.Error("Unhandled activation type " + esr.EnumActivationTypeID + " for skill " + esr.EnumSkillID);
                        break;
                }
            }
            world.Apply(new SkillEvent(source, EnumSkill.Kick, target.WithHealthChange(-20), succeeds, "Ninja Kick"));
        }

        public static float GetCompetancy(CombatantModel combatant, EnumSkill skill)
        {
            Schema.MobileHasSkillRow mhs = Global.Schema.MobileHasSkill.FindByTemplateObjectIDEnumSkillID(
                // TODO: do we even want templates?
                //TemplateObjectId,
                combatant.Id,
                (int)skill);
            if (mhs != null)
                return (float)mhs.Rating;
            return 0;
        }

        public static void DoKill(Avatar caster, EntityModel target)
        {
            caster.Attack(target);
        }

        public static void DoKick(Avatar caster, EntityModel target)
        {
            caster.Kick(target);
        }
    }
}
