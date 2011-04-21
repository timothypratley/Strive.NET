using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using Common.Logging;
using Strive.Common;
using Strive.Data.Events;
using Strive.Model;
using Strive.Network.Messages.ToServer;
using Strive.Network.Messaging;
using Strive.Server.DB;
using ToClient = Strive.Network.Messages.ToClient;


namespace Strive.Server.Logic
{
    /// <summary>
    /// A server side Avatar is a Mobile that
    /// optionally has a client associated with it.
    /// The server world contains only Avatars;
    /// no Mobiles, as all Mobiles are possesable and hence are Avatars.
    /// If a client is associated to an Avatar,
    /// that client is controlling the Mobile in question.
    /// </summary>
    public class Avatar : CombatantModel
    {
        public ClientConnection Client;
        public World World;
        public DateTime LastAttackUpdate;
        public DateTime LastHealUpdate;
        public DateTime LastBehaviourUpdate;
        public DateTime LastMoveUpdate;

        // if fighting someone or something
        public EntityModel Target;

        // if in a party
        public Party Party;
        public Party InvitedToParty;

        // currently invoking a skill
        public UseSkill ActivatingSkill;
        public DateTime ActivatingSkillTimestamp;
        public TimeSpan ActivatingSkillLeadTime;

        // any queued up skills to be executed after the current one
        public Queue<UseSkill> SkillQueue = new Queue<UseSkill>();

        // TODO: put these in the database schema
        public float AffinityAir;
        public float AffinityEarth;
        public float AffinityFire;
        public float AffinityLife;
        public float AffinityWater;

        readonly ILog _log = LogManager.GetCurrentClassLogger();

        public Avatar(
            World world,
            int id,
            string name,
            string modelId,
            Vector3D position,
            Quaternion rotation,
            float health,
            float energy,
            EnumMobileState mobileState,
            float height,
            int constitution,
            int dexterity,
            int willpower,
            int cognition,
            int strength
        )
            : base(id, name, modelId, position, rotation, health, energy, mobileState, height, constitution, dexterity, willpower, cognition, strength)
        {
            World = world;
        }

        public Avatar(World world, Schema.ObjectInstanceRow instance, Schema.TemplateObjectRow template, Schema.TemplateMobileRow mobile)
            : base(instance.ObjectInstanceID, template.TemplateObjectName, template.TemplateObjectName,
                new Vector3D(instance.X, instance.Y, instance.Z),
                new Quaternion(instance.RotationX, instance.RotationY, instance.RotationZ, instance.RotationW),
                (float)instance.HealthCurrent, (float)instance.EnergyCurrent,
                (EnumMobileState)mobile.EnumMobileStateID, template.Height,
                mobile.Constitution, mobile.Dexterity, mobile.Willpower, mobile.Cognition, mobile.Strength)
        {
            World = world;
        }

        public void SetMobileState(EnumMobileState ms)
        {
            if (MobileState == ms)
                return;

            World.Add(this.WithState(ms));
        }


        public void SendLog(string message)
        {
            if (Client == null)
            {
                _log.Warn(Id + ", no client: " + message);
            }
            else
            {
                Client.LogMessage(message);
            }
        }

        public void SendPartyTalk(string message)
        {
            Party.SendPartyTalk(Name, message);
        }

        public void Update()
        {
            // check for activating skills
            if (ActivatingSkill != null)
            {
                if (ActivatingSkillTimestamp + ActivatingSkillLeadTime <= Global.Now)
                {
                    SkillCommandProcessor.UseSkillNow(World, this, ActivatingSkill);
                    ActivatingSkill = null;
                }
            }
            // check for queued skills
            else if (SkillQueue.Count > 0)
                ActivatingSkill = SkillQueue.Dequeue();

            if (Target != null)
                CombatUpdate();
            else if (Client == null)
                BehaviourUpdate();
            else
            {
                if (Global.Now - LastMoveUpdate > TimeSpan.FromSeconds(1)
                    && (MobileState == EnumMobileState.Running
                        || MobileState == EnumMobileState.Walking))
                    SetMobileState(EnumMobileState.Standing);
            }
            HealUpdate();
        }

        public void CombatUpdate()
        {
            if (Global.Now - LastAttackUpdate > TimeSpan.FromSeconds(3))
            {
                // combat
                LastAttackUpdate = Global.Now;
                PhysicalAttack(Target, EnumSkill.Kill);
            }
        }

        public void BehaviourUpdate()
        {
            Quaternion rotation = Rotation;
            Vector3D position = Position;
            EnumMobileState mobileState = MobileState;
            // continue doing whatever you were doing
            if (Global.Now - LastMoveUpdate > TimeSpan.FromSeconds(1))
            {
                if (MobileState >= EnumMobileState.Standing)
                {
                    rotation.Y += (float)(Global.Rand.NextDouble() * 40 - 20);
                    while (Rotation.Y < 0) rotation.Y += 360;
                    while (Rotation.Y >= 360) rotation.Y -= 360;
                }
                Matrix3D m = Matrix3D.Identity;
                m.RotatePrepend(rotation);
                Vector3D velocity = new Vector3D(1, 0, 0) * m;
                switch (MobileState)
                {
                    case EnumMobileState.Running:
                        // TODO: using timing, not constant values
                        position = Position + 3 * velocity / 10;
                        break;
                    case EnumMobileState.Walking:
                        position = Position + velocity / 10;
                        break;
                    default:
                        // do nothing
                        break;
                }
            }
            if (Global.Now - LastBehaviourUpdate > TimeSpan.FromSeconds(3))
            {
                // change behavior?
                LastBehaviourUpdate = Global.Now;
                if (MobileState > EnumMobileState.Incapacitated)
                {
                    int rand = Global.Rand.Next(5) - 2;
                    if (rand > 1 && MobileState > EnumMobileState.Sleeping)
                        mobileState = MobileState - 1;
                    else if (rand < -1 && MobileState < EnumMobileState.Running)
                        mobileState = MobileState + 1;
                }
            }
            if (rotation != Rotation || position != Position || mobileState != MobileState)
                World.Relocate(this, position, rotation, mobileState);
        }

        public void HealUpdate()
        {
            if (Global.Now - LastHealUpdate > TimeSpan.FromSeconds(1))
            {
                LastHealUpdate = Global.Now;

                switch (MobileState)
                {
                    case EnumMobileState.Incapacitated:
                        World.Add(this.WithHealthChange(-0.5f).WithEnergyChange(-0.5f));
                        break;
                    case EnumMobileState.Sleeping:
                        World.Add(this.WithHealthChange(Constitution / 10.0F).WithEnergyChange(Constitution / 10.0F));
                        break;
                    case EnumMobileState.Resting:
                        World.Add(this.WithHealthChange(Constitution / 40.0F).WithEnergyChange(Constitution / 40.0F));
                        break;
                }
            }
        }

        public void Attack(EntityModel target)
        {
            Target = target;
            World.Add(this.WithState(EnumMobileState.Fighting));
            World.InformNearby(
                this,
                new ToClient.CombatReport(this, EnumSkill.Kill, target, 0));
        }

        public void Kick(EntityModel target)
        {
            World.Apply(new SkillEvent(this, EnumSkill.Kick, target.WithHealthChange(-20), true, "Ninja Kick"));
        }

        // TODO: use dynamic instead
        public void PhysicalAttack(EntityModel e, EnumSkill skill)
        {
            int damage;

            // TODO use the real range of kill
            if ((Position - e.Position).Length > 100)
            {
                // target is out of range
                SendLog(Target.Name + " is out of range.");
                return;
            }

            // TODO: use overloading???
            if (e is Avatar)
            {
                var opponent = (Avatar)e;

                // if not already in a fight, your opponent automatically
                // fights back
                if (opponent.Target == null)
                    opponent.Target = this;

                // avoidance phase: ratio of Dexterity
                if (Dexterity == 0 || Global.Rand.Next(100) <= opponent.Dexterity / Dexterity * 20)
                {
                    // 20% chance for equal dexterity player to avoid
                    World.InformNearby(
                        this,
                        new ToClient.CombatReport(this, skill, Target, 0));
                    return;
                }

                // hit phase: hit-roll determines if you miss, hit armor, or bypass armor
                int hitroll = 80;
                int attackroll = Global.Rand.Next(hitroll);

                if (attackroll < 20)
                {
                    // %20 chance to miss for weapon with 100 hit-roll
                    // TODO: differentiate between miss and avoid
                    World.InformNearby(
                        this,
                        new ToClient.CombatReport(this, skill, Target, 0));
                    return;
                }

                // damage phase: weapon damage + bonuses
                damage = 10;
                // TODO: use armor rating
                if (attackroll < 50)
                    damage -= 8; // opponent.ArmorRating
                if (damage < 0)
                    damage = 0;

                damage *= Strength / opponent.Constitution;
                opponent = (Avatar)opponent.WithHealthChange(-damage);
                if (opponent.MobileState != EnumMobileState.Dead && opponent.MobileState != EnumMobileState.Incapacitated)
                    opponent = (Avatar)opponent.WithState(EnumMobileState.Fighting);
                World.Add(opponent);
            }
            else
            {
                damage = 10;
                World.Add(e.WithHealthChange(-damage));

                // You destroyed the item
                if (e.Health <= 0)
                    World.Remove(e);
            }
            World.InformNearby(this, new ToClient.CombatReport(this, skill, Target, damage));
        }

        // TODO: use dynamic instead
        public void MagicalAttack(EntityModel po, float damage, EnumSkill skill)
        {
            if (po is Avatar)
            {
                var opponent = (Avatar)po;

                // if not already in a fight, your opponent automatically
                // fights back
                if (opponent.Target == null)
                    opponent.Target = this;

                // avoidance phase: Dexterity
                if (Global.Rand.Next(100) <= opponent.Dexterity)
                {
                    World.InformNearby(this, new ToClient.CombatReport(this, skill, po, 0));
                    return;
                }

                World.Add(opponent.WithHealthChange(-damage * Cognition / opponent.Willpower));
            }
            else
            {
                // attacking object
                World.Add(po.WithHealthChange(-damage));

                // You destroyed the item
                if (po.Health <= 0)
                    World.Remove(po);
            }

            // TODO: move all the inform stuff into event propigation
            World.InformNearby(this, new ToClient.CombatReport(this, skill, po, damage));
        }

        public float CurrentHeight { get { return MobileState <= EnumMobileState.Resting ? 0.3f : Height; } }

        public override string ToString()
        {
            return "Avatar '" + Name + "' (" + Id + ")";
        }
    }
}
