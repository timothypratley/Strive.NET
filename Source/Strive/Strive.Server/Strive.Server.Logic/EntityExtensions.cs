using System;
using System.Windows.Media.Media3D;
using Strive.Common;
using Strive.Data.Events;
using Strive.Model;


namespace Strive.Server.Logic
{
    public static class EntityExtensions
    {
        public static void UpdateEntity(this EntityModel entity, World world, DateTime when)
        {
            if (!world.Possession.ContainsKey(entity.Id)
                && entity.MobileState > EnumMobileState.Incapacitated
                && (when - entity.LastMoveUpdate).TotalSeconds > 1)
                entity = entity.BehaviourUpdate(world, when);

            var c = entity as CombatantModel;
            if (c != null)
                entity = c.Update(world, when);

            world.Apply(new EntityUpdateEvent(entity, "Update"));
        }

        private static CombatantModel Update(this CombatantModel combatant, World world, DateTime when)
        {
            // check for activating skills
            if (combatant.ActivatingSkill != EnumSkill.None
                && combatant.ActivatingSkillTimestamp + combatant.ActivatingSkillLeadTime <= when)
                world.UseSkillNow(
                    combatant,
                    // TODO: avoid looking it up?
                    Global.Schema.EnumSkill.FindByEnumSkillID((int)combatant.ActivatingSkill),
                    combatant.Target);

            // TODO: check for queued skills
            //else if (c.SkillQueue.Length > 0)
            //c.ActivatingSkill = c.SkillQueue.Head;

            if (combatant.Target != null)
                combatant.CheckAttack(world, when);
            else if ((when - combatant.LastMoveUpdate).TotalSeconds > 1
                && (combatant.MobileState == EnumMobileState.Running
                    || combatant.MobileState == EnumMobileState.Walking))
                // TODO: where to check if changed?, also can remove cast with generics
                combatant = (CombatantModel)combatant.WithState(EnumMobileState.Standing);

            combatant = combatant.HealUpdate(when);
            return combatant;
        }

        public static void CheckAttack(this CombatantModel combatant, World world, DateTime when)
        {
            if ((when - combatant.LastAttackUpdate).TotalSeconds > 3)
            {
                // TODO: don't look it up every time
                var esr = Global.Schema.EnumSkill.FindByEnumSkillID((int)EnumSkill.Kill);
                world.UseSkillNow(combatant, esr, combatant.Target);
            }
        }

        public static EntityModel BehaviourUpdate(this EntityModel entity, World world, DateTime when)
        {
            Quaternion rotation = entity.Rotation;
            Vector3D position = entity.Position;
            EnumMobileState mobileState = entity.MobileState;
            var task = world.DoingTask(entity);
            var sinceSeconds = Math.Min(2, (when - entity.LastMobileStateUpdate).TotalSeconds);

            // Mobile state
            if (task == null)
            {
                if (sinceSeconds > 3)
                {
                    int rand = Global.Rand.Next(5) - 2;
                    if (rand > 1 && mobileState > EnumMobileState.Sleeping)
                        mobileState = mobileState - 1;
                    else if (rand < -1 && mobileState < EnumMobileState.Running)
                        mobileState = mobileState + 1;
                }
            }
            else if (mobileState != EnumMobileState.Fighting)
            {
                var goalVector = (task.Finish - entity.Position);
                if (goalVector.Length < entity.MoveRunSpeed * 2)
                    mobileState = EnumMobileState.Walking;
                else
                    mobileState = EnumMobileState.Running;
            }

            // Movement
            if (mobileState >= EnumMobileState.Standing)
            {
                if (task == null)
                {
                    // move randomly
                    rotation *= new Quaternion(Global.Up, (Global.Rand.NextDouble() * 40 - 20) * entity.MoveTurnSpeed);
                }
                else
                {
                    // move toward goal
                    var forward = new Vector3D(1, 0, 0);
                    var goalVector = (task.Finish - entity.Position);
                    if (goalVector.LengthSquared > 0)
                    {
                        goalVector.Normalize();
                        Vector3D axis = Vector3D.CrossProduct(goalVector, forward);
                        double angle = Vector3D.AngleBetween(goalVector, forward);
                        rotation = new Quaternion(axis, -angle);
                    }
                }
            }
            Matrix3D m = Matrix3D.Identity;
            m.RotatePrepend(rotation);
            Vector3D velocity = new Vector3D(1, 0, 0) * m;
            switch (mobileState)
            {
                case EnumMobileState.Running:
                    position = entity.Position + entity.MoveRunSpeed * velocity * sinceSeconds;
                    break;
                case EnumMobileState.Walking:
                    position = entity.Position + entity.MoveRunSpeed * velocity * sinceSeconds / 3;
                    break;
                default:
                    // do nothing
                    break;
            }

            if (rotation != entity.Rotation || position != entity.Position || mobileState != entity.MobileState)
                return entity.Move(mobileState, position, rotation, when);
            else
                return entity;
        }

        public static CombatantModel HealUpdate(this CombatantModel combatant, DateTime when)
        {
            if ((when - combatant.LastHealUpdate).TotalSeconds > 5)
            {
                switch (combatant.MobileState)
                {
                    case EnumMobileState.Incapacitated:
                        return combatant.WithHealUpdate(-0.5f, -0.5f, when);
                    case EnumMobileState.Sleeping:
                        return combatant.WithHealUpdate(combatant.Constitution / 10.0f, combatant.Constitution / 10.0f, when);
                    case EnumMobileState.Resting:
                        return combatant.WithHealUpdate(combatant.Constitution / 40.0f, combatant.Constitution / 40.0f, when);
                }
            }
            return combatant;
        }
    }
}