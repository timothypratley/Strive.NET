using System;
using System.Windows.Media.Media3D;
using Strive.Common;
using Strive.Data.Events;
using Strive.Model;


namespace Strive.Server.Logic
{
    public static class AvatarExtensions
    {
        public static void UpdateEntity(this World world, EntityModel entity)
        {
            if (!world.Possession.ContainsKey(entity.Id))
                entity = world.BehaviourUpdate(entity);

            var c = entity as CombatantModel;
            if (c != null)
                entity = UpdateCombatant(world, c);

            world.Apply(new EntityUpdateEvent(entity, "Update"));
        }

        private static CombatantModel UpdateCombatant(World world, CombatantModel c)
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

            else if (Global.Now - c.LastMoveUpdate > TimeSpan.FromSeconds(1)
                && (c.MobileState == EnumMobileState.Running
                || c.MobileState == EnumMobileState.Walking))
                // TODO: where to check if changed?, also can remove cast with generics
                c = (CombatantModel)c.WithState(EnumMobileState.Standing);

            c = c.HealUpdate();
            return c;
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

        public static EntityModel BehaviourUpdate(this World world, EntityModel entity)
        {
            Quaternion rotation = entity.Rotation;
            Vector3D position = entity.Position;
            EnumMobileState mobileState = entity.MobileState;
            var task = world.DoingTask(entity);

            // continue doing whatever you were doing
            if (Global.Now - entity.LastMoveUpdate > TimeSpan.FromSeconds(1))
            {
                if (entity.MobileState >= EnumMobileState.Standing)
                {
                    if (task != null)
                    {
                        // move toward goal
                        var goalVector = (task.Finish - entity.Position);
                        if (goalVector.LengthSquared > 0)
                            rotation.Y = Math.Atan2(goalVector.Y, goalVector.X) * 180 / Math.PI;
                    }
                    else
                    {
                        // move randomly
                        rotation.Y += (float)(Global.Rand.NextDouble() * 10 - 5)
                            * entity.MoveTurnSpeed;
                        while (rotation.Y < 0) rotation.Y += 360;
                        while (rotation.Y >= 360) rotation.Y -= 360;
                    }
                }
                Matrix3D m = Matrix3D.Identity;
                m.RotatePrepend(rotation);
                Vector3D velocity = new Vector3D(1, 0, 0) * m;
                switch (entity.MobileState)
                {
                    case EnumMobileState.Running:
                        // TODO: using timing, not constant values
                        position = entity.Position + entity.MoveRunSpeed * velocity / 30;
                        break;
                    case EnumMobileState.Walking:
                        position = entity.Position + entity.MoveRunSpeed * velocity / 100;
                        break;
                    default:
                        // do nothing
                        break;
                }
            }
            if (entity.MobileState > EnumMobileState.Incapacitated
                && Global.Now - entity.LastMobileStateUpdate > TimeSpan.FromSeconds(3))
            {
                int rand = Global.Rand.Next(5) - 2;
                if (task == null && rand > 1 && entity.MobileState > EnumMobileState.Sleeping)
                    mobileState = entity.MobileState - 1;
                else if (rand < -1 && entity.MobileState < EnumMobileState.Running)
                    mobileState = entity.MobileState + 1;
            }
            if (rotation != entity.Rotation || position != entity.Position || mobileState != entity.MobileState)
                return entity.Move(mobileState, position, rotation, Global.Now);
            else
                return entity;
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