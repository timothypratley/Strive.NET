using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using Strive.Common;
using Strive.Data.Events;
using Strive.Model;
using Strive.Network.Messaging;


namespace Strive.Server.Logic
{
    public static class AvatarExtensions
    {
        public static Dictionary<EntityModel, ClientConnection> Clients;

        public static void LogMessage(this EntityModel entity, string message)
        {
            var client = Clients[entity];
            if (client != null)
                client.LogMessage(message);
        }

        public static void UpdateCombatant(this World world, CombatantModel c)
        {
            // check for activating skills
            if (c.ActivatingSkill != EnumSkill.None
                && c.ActivatingSkillTimestamp + c.ActivatingSkillLeadTime <= Global.Now)
                world.UseSkillNow(
                    c,
                    // TODO: avoid looking it up?
                    Global.Schema.EnumSkill.FindByEnumSkillID((int)c.ActivatingSkill),
                    c.Target);

            // TODO:
            // check for queued skills
            //else if (c.SkillQueue.Length > 0)
            //c.ActivatingSkill = c.SkillQueue.Head;

            if (c.Target != null)
                world.CheckAttack(c);
            // TODO: enable behavior updates
            //else if (listener.Client[c] == null)
            //c = world.BehaviourUpdate(c);
            else if (Global.Now - c.LastMoveUpdate > TimeSpan.FromSeconds(1)
                && (c.MobileState == EnumMobileState.Running
                || c.MobileState == EnumMobileState.Walking))
                // TODO: where to check if changed?, also can remove cast with generics
                c = (CombatantModel)c.WithState(EnumMobileState.Standing);

            c = c.HealUpdate();

            world.Apply(new EntityUpdateEvent(c, "Update"));
        }

        public static void CheckAttack(this World world, CombatantModel me)
        {

            if (Global.Now - me.LastAttackUpdate > TimeSpan.FromSeconds(3))
            {
                // TODO: don't look it up every time
                var esr = Global.Schema.EnumSkill.FindByEnumSkillID((int)EnumSkill.Kill);
                world.UseSkillNow(me, esr, me.Target);
            }
        }

        public static void BehaviourUpdate(this World world, CombatantModel combatant)
        {
            Quaternion rotation = combatant.Rotation;
            Vector3D position = combatant.Position;
            EnumMobileState mobileState = combatant.MobileState;
            // continue doing whatever you were doing
            if (Global.Now - combatant.LastMoveUpdate > TimeSpan.FromSeconds(1))
            {
                if (combatant.MobileState >= EnumMobileState.Standing)
                {
                    rotation.Y += (float)(Global.Rand.NextDouble() * 40 - 20);
                    while (combatant.Rotation.Y < 0) rotation.Y += 360;
                    while (combatant.Rotation.Y >= 360) rotation.Y -= 360;
                }
                Matrix3D m = Matrix3D.Identity;
                m.RotatePrepend(rotation);
                Vector3D velocity = new Vector3D(1, 0, 0) * m;
                switch (combatant.MobileState)
                {
                    case EnumMobileState.Running:
                        // TODO: using timing, not constant values
                        position = combatant.Position + 3 * velocity / 10;
                        break;
                    case EnumMobileState.Walking:
                        position = combatant.Position + velocity / 10;
                        break;
                    default:
                        // do nothing
                        break;
                }
            }
            if (Global.Now - combatant.LastBehaviourUpdate > TimeSpan.FromSeconds(3))
            {
                // change behavior?
                // TODO: record behavior change time
                //combatant.LastBehaviourUpdate = Global.Now;
                if (combatant.MobileState > EnumMobileState.Incapacitated)
                {
                    int rand = Global.Rand.Next(5) - 2;
                    if (rand > 1 && combatant.MobileState > EnumMobileState.Sleeping)
                        mobileState = combatant.MobileState - 1;
                    else if (rand < -1 && combatant.MobileState < EnumMobileState.Running)
                        mobileState = combatant.MobileState + 1;
                }
            }
            if (rotation != combatant.Rotation || position != combatant.Position || mobileState != combatant.MobileState)
                world.Apply(new EntityUpdateEvent(
                    combatant.Move(mobileState, position, rotation, Global.Now),
                    "AI"));
        }

        public static CombatantModel HealUpdate(this CombatantModel combatant)
        {
            if (Global.Now - combatant.LastHealUpdate > TimeSpan.FromSeconds(5))
            {
                switch (combatant.MobileState)
                {
                    case EnumMobileState.Incapacitated:
                        return combatant.WithHealUpdate(-0.5f, -0.5f, Global.Now);
                    case EnumMobileState.Sleeping:
                        return combatant.WithHealUpdate(combatant.Constitution / 10.0f, combatant.Constitution / 10.0f, Global.Now);
                    case EnumMobileState.Resting:
                        return combatant.WithHealUpdate(combatant.Constitution / 40.0f, combatant.Constitution / 40.0f, Global.Now);
                }
            }
            return combatant;
        }
    }
}