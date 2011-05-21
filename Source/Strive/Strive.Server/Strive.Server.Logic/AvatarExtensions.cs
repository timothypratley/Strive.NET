using System;
using System.Windows.Media.Media3D;
using Strive.Common;
using Strive.Data.Events;
using Strive.Model;


namespace Strive.Server.Logic
{
    public static class AvatarExtensions
    {
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

            // TODO: check for queued skills
            //else if (c.SkillQueue.Length > 0)
            //c.ActivatingSkill = c.SkillQueue.Head;

            if (c.Target != null)
                world.CheckAttack(c);

            if (!world.Possession.ContainsKey(c.Id))
                c = world.BehaviourUpdate(c);
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

        public static CombatantModel BehaviourUpdate(this World world, CombatantModel combatant)
        {
            Quaternion rotation = combatant.Rotation;
            Vector3D position = combatant.Position;
            EnumMobileState mobileState = combatant.MobileState;
            var task = world.DoingTask(combatant);

            // continue doing whatever you were doing
            if (Global.Now - combatant.LastMoveUpdate > TimeSpan.FromSeconds(1))
            {
                if (combatant.MobileState >= EnumMobileState.Standing)
                {
                    if (task != null)
                    {
                        // move toward goal
                        var goalVector = (task.Finish - combatant.Position);
                        rotation.Y = Math.Atan2(goalVector.Y, goalVector.X) * 180 / Math.PI;
                    }
                    else
                    {
                        // move randomly
                        rotation.Y += (float)(Global.Rand.NextDouble() * 40 - 20)
                            * combatant.MoveTurnSpeed;
                        while (combatant.Rotation.Y < 0) rotation.Y += 360;
                        while (combatant.Rotation.Y >= 360) rotation.Y -= 360;
                    }
                }
                Matrix3D m = Matrix3D.Identity;
                m.RotatePrepend(rotation);
                Vector3D velocity = new Vector3D(1, 0, 0) * m;
                switch (combatant.MobileState)
                {
                    case EnumMobileState.Running:
                        // TODO: using timing, not constant values
                        position = combatant.Position + combatant.MoveRunSpeed * velocity / 3;
                        break;
                    case EnumMobileState.Walking:
                        position = combatant.Position + combatant.MoveRunSpeed * velocity / 10;
                        break;
                    default:
                        // do nothing
                        break;
                }
            }
            if (combatant.MobileState > EnumMobileState.Incapacitated
                && Global.Now - combatant.LastMobileStateUpdate > TimeSpan.FromSeconds(3))
            {
                int rand = Global.Rand.Next(5) - 2;
                if (task == null && rand > 1 && combatant.MobileState > EnumMobileState.Sleeping)
                    mobileState = combatant.MobileState - 1;
                else if (rand < -1 && combatant.MobileState < EnumMobileState.Running)
                    mobileState = combatant.MobileState + 1;
            }
            if (rotation != combatant.Rotation || position != combatant.Position || mobileState != combatant.MobileState)
                return (CombatantModel)combatant.Move(mobileState, position, rotation, Global.Now);
            else
                return combatant;
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