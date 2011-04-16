
namespace Strive.Server.Schema
{
    public class Wieldable : Item
    {
        public Wieldable() { }
        public Wieldable(
            Schema.TemplateItemWieldableRow wieldable,
            Schema.TemplateItemRow item,
            Schema.TemplateObjectRow template,
            Schema.ObjectInstanceRow instance)
            : base(item, template, instance)
        {
        }
    }
}
