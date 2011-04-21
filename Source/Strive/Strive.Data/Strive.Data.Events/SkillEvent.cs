using Strive.Common;
using Strive.Model;

namespace Strive.Data.Events
{
    public class SkillEvent : Event
    {
        public SkillEvent(EntityModel source, EnumSkill skill, EntityModel target, bool succeeds, string description)
        {
            Source = source;
            Skill = skill;
            Target = target;
            Succeeds = succeeds;
            Description = description;
        }

        public EntityModel Source { get; set; }
        public EnumSkill Skill { get; set; }
        public EntityModel Target { get; set; }
        public bool Succeeds { get; set; }
    }
}
