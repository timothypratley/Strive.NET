using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using Common.Logging;
using Strive.Common;
using Strive.Model;
using Strive.Network.Messages;
using Strive.Network.Messages.ToServer;
using Strive.Network.Messaging;
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
            EnumMobileState mobileState,
            float height,
            int constitution,
            int dexterity,
            int willpower,
            int strength
        )
            : base(id, name, modelId, position, rotation, health, mobileState, height, constitution, dexterity, willpower, strength)
        {
            World = world;
        }

        /**
        public Avatar(World world)
        {
            World = world;
        }
         */

        public void SetMobileState(EnumMobileState ms)
        {
            if (MobileState == ms)
                return;

            MobileState = ms;
            // TODO: this will probably change if we use anything more
            // advance than stick to ground.
            // changing state may have moved the mobile.
            double? altitude = World.AltitudeAt(Position.X, Position.Z) + CurrentHeight / 2;
            if (altitude.HasValue)
                Position.Y = altitude.Value;

            // MobileState message has position info
            // as it is likely that this will have changed
            World.InformNearby(this, new ToClient.MobileState(this));
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
                PhysicalAttack(Target);
            }
        }

        public void BehaviourUpdate()
        {
            // continue doing whatever you were doing
            if (Global.Now - LastMoveUpdate > TimeSpan.FromSeconds(1))
            {
                if (MobileState >= EnumMobileState.Standing)
                {
                    Rotation.Y += (float)(Global.Rand.NextDouble() * 40 - 20);
                    while (Rotation.Y < 0) Rotation.Y += 360;
                    while (Rotation.Y >= 360) Rotation.Y -= 360;
                }
                Matrix3D m = Matrix3D.Identity;
                m.RotatePrepend(Rotation);
                Vector3D velocity = new Vector3D(1, 0, 0) * m;
                switch (MobileState)
                {
                    case EnumMobileState.Running:
                        // TODO: using timing, not constant values
                        World.Relocate(this, (Position + 3 * velocity / 10), Rotation);
                        break;
                    case EnumMobileState.Walking:
                        World.Relocate(this, (Position + velocity / 10), Rotation);
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
                    {
                        SetMobileState(MobileState - 1);
                        //Log.Info( TemplateObjectName + " changed behavior from " + (MobileState+1) + " to " + MobileState + "." );
                    }
                    else if (rand < -1 && MobileState < EnumMobileState.Running)
                    {
                        SetMobileState(MobileState + 1);
                        //Log.Info( TemplateObjectName + " changed behavior from " + (MobileState-1) + " to " + MobileState + "." );
                    }
                }
            }
        }

        public void HealUpdate()
        {
            if (Global.Now - LastHealUpdate > TimeSpan.FromSeconds(1))
            {
                LastHealUpdate = Global.Now;
                switch (MobileState)
                {
                    case EnumMobileState.Incapacitated:
                        HitPoints -= 0.5F;
                        Energy -= 0.5F;
                        break;
                    case EnumMobileState.Sleeping:
                        HitPoints += Constitution / 10.0F;
                        Energy += Constitution / 10.0F;
                        break;
                    case EnumMobileState.Resting:
                        HitPoints += Constitution / 40.0F;
                        Energy += Constitution / 40.0F;
                        break;
                }
            }

            // we might have died, or recovered
            UpdateState();
        }

        public void Damage(float damage)
        {
            HitPoints -= damage;
            UpdateState();
        }

        public void Attack(EntityModel target)
        {
            Target = target;
            MobileState = EnumMobileState.Fighting;
            World.InformNearby(
                this,
                new ToClient.CombatReport(this, target, EnumCombatEvent.Attacks, 0));
        }

        public void Kick(EntityModel target)
        {
            // TODO: would be nice to have a base class physical object with damage function,
            // but needs multiple inheritance
            target.HitPoints -= 20;
            World.InformNearby(
                this,
                new ToClient.CombatReport(this, target, EnumCombatEvent.Hits, 20));
            ((Avatar)target).UpdateState();
        }

        // TODO: use dynamic instead
        public void PhysicalAttack(EntityModel e)
        {
            // TODO use the real range of kill
            if ((Position - e.Position).Length > 100)
            {
                // target is out of range
                SendLog(Target.Name + " is out of range.");
                return;
            }
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
                        new ToClient.CombatReport(this, Target, EnumCombatEvent.Avoids, 0));
                    return;
                }

                // hit phase: hit-roll determines if you miss, hit armor, or bypass armor
                int hitroll = 80;
                int attackroll = Global.Rand.Next(hitroll);

                if (attackroll < 20)
                {
                    // %20 chance to miss for weapon with 100 hit-roll
                    World.InformNearby(
                        this,
                        new ToClient.CombatReport(this, Target, EnumCombatEvent.Misses, 0));
                }

                // damage phase: weapon damage + bonuses
                int damage = 10;
                // TODO: use armor rating
                if (attackroll < 50)
                    damage -= 8; // opponent.ArmorRating
                if (damage < 0)
                    damage = 0;

                damage *= Strength / opponent.Constitution;
                opponent.HitPoints -= damage;
                opponent.MobileState = EnumMobileState.Fighting;
                opponent.UpdateState();
                World.InformNearby(
                    this,
                    new ToClient.CombatReport(this, Target, EnumCombatEvent.Hits, damage));
            }
            else if (e is Item)
            {
                // attacking object
                var item = Target as Item;
                int damage = 10;
                item.HitPoints -= damage;
                World.InformNearby(
                    this,
                    new ToClient.CombatReport(this, Target, EnumCombatEvent.Hits, damage));

                // You destroyed the item
                if (item.HitPoints <= 0)
                    World.Remove(item);
            }
            else
                throw new Exception("ERROR: attacking a " + e.GetType() + " " + e);
        }

        // TODO: use dynamic instead
        public void MagicalAttack(EntityModel po, float damage)
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
                    World.InformNearby(
                        this,
                        new ToClient.CombatReport(this, Target, EnumCombatEvent.Avoids, 0));
                    return;
                }

                // damage phase
                opponent.HitPoints -= damage * Cognition / opponent.Willpower;
                opponent.UpdateState();
                World.InformNearby(
                    this,
                    new ToClient.CombatReport(this, Target, EnumCombatEvent.Hits, damage));
            }
            else if (po is Item)
            {
                // attacking object
                var item = (Item)Target;
                item.HitPoints -= damage;
                World.InformNearby(
                    this,
                    new ToClient.CombatReport(this, Target, EnumCombatEvent.Hits, damage));

                // You destroyed the item
                if (item.HitPoints <= 0)
                    World.Remove(item);
            }
            else
                throw new Exception("ERROR: attacking a " + po.GetType() + " " + po);
        }

        public void UpdateState()
        {
            if (MobileState >= EnumMobileState.Sleeping)
            {
                if (HitPoints <= 0)
                {
                    HitPoints = 0;
                    SetMobileState(EnumMobileState.Incapacitated);
                }
            }
            else if (MobileState == EnumMobileState.Incapacitated)
            {
                if (HitPoints <= -50)
                    Death();
                else if (HitPoints > 0)
                    SetMobileState(EnumMobileState.Resting);
            }
        }

        public void Death()
        {
            // RIP
            SetMobileState(EnumMobileState.Dead);

            /*
            if (Client != null)
            {
                // re-spawn!
                HitPoints = MaxHitPoints;

                // TODO: where should we re-spawn?
                World.Relocate(this, new Vector3D(0, 0, 0), Quaternion.Identity);

                // set resting in new location to let everyone know
                SetMobileState(EnumMobileState.Resting);
            }
            else
            {
                // TODO: should probably stay around as a corpse
                // world.Remove( this );
                // but we need some decay/re-pop code
            }
             */
        }

        public float CurrentHeight { get { return MobileState <= EnumMobileState.Resting ? 0 : Height; } }

        public float GetCompetancy(EnumSkill skill)
        {
            Schema.MobileHasSkillRow mhs = Global.ModelSchema.MobileHasSkill.FindByTemplateObjectIDEnumSkillID(
                TemplateObjectId, (int)skill);
            if (mhs != null)
                return (float)mhs.Rating;
            return 0;
        }

        public override string ToString()
        {
            return "Avatar '" + Name + "' (" + Id + ")";
        }
    }
}
