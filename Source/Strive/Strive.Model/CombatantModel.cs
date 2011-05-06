using System;
using System.Windows.Media.Media3D;
using Microsoft.FSharp.Collections;
using Strive.Common;

namespace Strive.Model
{
    public class CombatantModel : EntityModel
    {
        public CombatantModel(int id, string name, string modelId, Vector3D position, Quaternion rotation,
            float health, float energy, EnumMobileState mobileState, float height,
            int constitution, int dexterity, int willpower, int cognition, int strength)
            : base(id, name, modelId, position, rotation, health, energy, mobileState, height)
        {
            Constitution = constitution;
            Dexterity = dexterity;
            Willpower = willpower;
            Cognition = cognition;
            Strength = strength;

            ActivatingSkill = EnumSkill.None;
            SkillQueue = ListModule.Empty<Tuple<EnumSkill, EntityModel>>();
        }

        public int Party { get; private set; }
        public int InvitedToParty { get; private set; }
        public int Constitution { get; private set; }
        public int Dexterity { get; private set; }
        public int Willpower { get; private set; }
        public int Cognition { get; private set; }
        public int Strength { get; private set; }
        public DateTime LastAttackUpdate { get; private set; }
        public DateTime LastHealUpdate { get; private set; }
        public DateTime LastBehaviourUpdate { get; private set; }
        public EnumSkill ActivatingSkill { get; private set; }
        // TODO: Skill target and current target can be different
        public EntityModel Target { get; private set; }
        public DateTime ActivatingSkillTimestamp { get; private set; }
        public TimeSpan ActivatingSkillLeadTime { get; private set; }
        public FSharpList<Tuple<EnumSkill, EntityModel>> SkillQueue { get; private set; }

        public CombatantModel StartSkill(EnumSkill skill, EntityModel target, DateTime when, TimeSpan leadTime)
        {
            var r = (CombatantModel)MemberwiseClone();
            r.ActivatingSkill = skill;
            r.Target = target;
            r.ActivatingSkillTimestamp = when;
            r.ActivatingSkillLeadTime = leadTime;
            return r;
        }

        public CombatantModel EnqueueSkill(EnumSkill skill, EntityModel target)
        {
            var r = (CombatantModel)MemberwiseClone();
            // TODO: zomg surely there is a simpler expression
            r.SkillQueue = ListModule.Append(
                SkillQueue,
                new FSharpList<Tuple<EnumSkill, EntityModel>>(new Tuple<EnumSkill, EntityModel>(skill, target),
                    ListModule.Empty<Tuple<EnumSkill, EntityModel>>()));
            return r;
        }

        public CombatantModel DequeueSkill()
        {
            var r = (CombatantModel)MemberwiseClone();
            r.SkillQueue = ListModule.Tail(SkillQueue);
            return r;
        }

        public CombatantModel EmptyQueue()
        {
            var r = (CombatantModel)MemberwiseClone();
            // TODO: zomg surely there is a simpler expression
            r.SkillQueue = ListModule.Empty<Tuple<EnumSkill, EntityModel>>();
            return r;
        }

        public CombatantModel WithTarget(CombatantModel target)
        {
            var r = (CombatantModel)MemberwiseClone();
            r.Target = target;
            return r;
        }

        public CombatantModel WithHealUpdate(float healthChange, float energyChange, DateTime when)
        {
            var r = (CombatantModel)MemberwiseClone();
            r.Health += healthChange;
            r.Energy += energyChange;
            r.LastHealUpdate = when;
            return r;
        }
    }
}
