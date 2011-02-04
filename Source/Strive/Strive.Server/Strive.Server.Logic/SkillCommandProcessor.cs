using System;
using System.Collections;

using Common.Logging;
using Strive.Network.Server;
using Strive.Server.Model;
using Strive.Common;


namespace Strive.Server.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public class SkillCommandProcessor
    {
        static ILog Log = LogManager.GetCurrentClassLogger();

        public static void ProcessUseSkill(Client client, Strive.Network.Messages.ToServer.UseSkill message)
        {
            MobileAvatar avatar = client.Avatar as MobileAvatar;
            if (avatar == null)
            {
                client.SendLog("Requested a skill, but doesn't have an avatar.");
                return;
            }
            Schema.EnumSkillRow esr = Global.ModelSchema.EnumSkill.FindByEnumSkillID((int)message.SkillID);
            if (esr == null)
            {
                client.SendLog("Requested an invalid skill " + message.SkillID);
                return;
            }

            // If already performing a skill invokation, just queue the request
            // for later.
            if (avatar.activatingSkill != null)
            {
                avatar.skillQueue.Enqueue(message);
                return;
            }

            if (esr.LeadTime <= 0)
            {
                // process it now
                UseSkillNow(avatar, message);
            }
            else
            {
                // process it later, after leadtime is elapsed
                MobileAvatar ma = client.Avatar as MobileAvatar;
                ma.activatingSkill = message;
                ma.activatingSkillTimestamp = Global.Now;
                ma.activatingSkillLeadTime = TimeSpan.FromSeconds(esr.LeadTime);
            }
        }

        public static void ProcessCancelSkill(Client client, Strive.Network.Messages.ToServer.CancelSkill message)
        {
            MobileAvatar avatar = client.Avatar as MobileAvatar;
            if (avatar == null)
            {
                client.SendLog("Canceled a skill invokation, but don't have an avatar.");
                return;
            }

            // If already performing invokation, just cancel it
            bool found = false;
            if (avatar.activatingSkill != null && avatar.activatingSkill.InvokationID == message.InvokationID)
            {
                avatar.activatingSkill = null;
                found = true;
            }
            else
            {
                // search for it in queued skill invokations
                // just generate a new new with the invokation missing
                Queue newQueue = new Queue();
                foreach (Strive.Network.Messages.ToServer.UseSkill m in avatar.skillQueue)
                {
                    if (m.InvokationID == message.InvokationID)
                    {
                        // don't add it
                        found = true;
                    }
                    else
                    {
                        newQueue.Enqueue(m);
                    }
                }
                avatar.skillQueue = newQueue;
            }
            if (found)
            {
                client.SendLog("Successfully canceled invokation " + message.InvokationID);
            }
            else
            {
                client.SendLog("Failed to cancel invokation " + message.InvokationID);
            }
        }

        public static void UseSkillNow(MobileAvatar caster, Strive.Network.Messages.ToServer.UseSkill message)
        {
            Schema.EnumSkillRow esr = Global.ModelSchema.EnumSkill.FindByEnumSkillID((int)message.SkillID);
            if (esr == null)
            {
                caster.SendLog("Requested an invalid skill " + message.SkillID);
                return;
            }

            if (esr.EnergyCost > caster.Energy)
            {
                caster.SendLog("Not enough energy to use " + esr.EnumSkillName + ", require " + esr.EnergyCost + ".");
                return;
            }

            // TODO: mek this wurk
            /*
            if ( esr.EnumMobileState > caster.MobileState ) {
                caster.SendLog( "Not while " + caster.MobileState.Name );
            }
            */

            MobileAvatar target = null;
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
                    target = (MobileAvatar)Global.World.physicalObjects[message.TargetPhysicalObjectIDs[0]];
                    if (target == null)
                    {
                        caster.SendLog("Target " + message.TargetPhysicalObjectIDs[0] + " not found.");
                        return;
                    }
                    if ((caster.Position - target.Position).Length > esr.Range)
                    {
                        // target is out of range
                        caster.SendLog(target.TemplateObjectName + " is out of range");
                        return;
                    }
                    break;
                default:
                    caster.SendLog("That skill does not work yet, contact admin.");
                    Log.Error("Unhandled targettype " + esr.EnumTargetTypeID + " for skill " + esr.EnumSkillID);
                    return;
            }

            if (TargetSkill(caster, target, esr))
            {
                // successful casting affects affinity with the elements
                caster.AffinityAir += esr.AirAffinity / 1000;
                caster.AffinityEarth += esr.EarthAffinity / 1000;
                caster.AffinityFire += esr.FireAffinity / 1000;
                caster.AffinityLife += esr.LifeAffinity / 1000;
                caster.AffinityWater += esr.WaterAffinity / 1000;
            }

            // deduct energy regardless
            caster.Energy -= esr.EnergyCost;
        }

        /// <summary>
        ///  returns true on success, false on failure
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        /// <param name="esr"></param>
        /// <returns></returns>
        public static bool TargetSkill(MobileAvatar caster, MobileAvatar target, Schema.EnumSkillRow esr)
        {
            // test adeptness
            float competancy = caster.GetCompetancy((EnumSkill)esr.EnumSkillID);
            double roll = Global.Rand.NextDouble();
            if (competancy < roll * 100)
            {
                // failed
                caster.SendLog("You failed " + esr.EnumSkillName + ".");
                return false;
            }

            switch ((EnumActivationType)esr.EnumActivationTypeID)
            {
                case EnumActivationType.AttackSpell:
                    float damage = esr.EnergyCost;
                    damage += esr.AirAffinity * caster.AffinityAir / target.AffinityAir;
                    damage += esr.EarthAffinity * caster.AffinityEarth / target.AffinityEarth;
                    damage += esr.FireAffinity * caster.AffinityFire / target.AffinityFire;
                    damage += esr.LifeAffinity * caster.AffinityLife / target.AffinityLife;
                    damage += esr.WaterAffinity * caster.AffinityWater / target.AffinityWater;
                    caster.MagicalAttack(target, damage);
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
                    {
                        DoKill(caster, target);
                    }
                    else if (esr.EnumSkillID == (int)EnumSkill.Kick)
                    {
                        DoKick(caster, target);
                    }
                    else
                    {
                        Log.Warn("Unhandled SkillID " + esr.EnumSkillID);
                    }
                    break;
                case EnumActivationType.Sorcery:
                    break;
                default:
                    caster.SendLog("That skill does not work yet, contact admin.");
                    Log.Error("Unhandled activation type " + esr.EnumActivationTypeID + " for skill " + esr.EnumSkillID);
                    return false;
            }

            // if we made it this far, the skill was successful :)
            return true;
        }

        public static void DoKill(MobileAvatar caster, PhysicalObject target)
        {
            caster.Attack(target);
        }

        public static void DoKick(MobileAvatar caster, PhysicalObject target)
        {
            caster.Kick(target);
        }
    }
}
