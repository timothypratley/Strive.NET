using System;

namespace Strive.Server.DB
{
    /// <summary>
    /// Summary description for Item.
    /// </summary>
    public abstract class Item : PhysicalObject
    {
        public int Value;
        public int Weight;

        public Item() { }
        public Item(
            Schema.TemplateItemRow item,
            Schema.TemplateObjectRow template,
            Schema.ObjectInstanceRow instance
        )
            : base(template, instance)
        {
            Value = item.Value;
            Weight = item.Weight;
            Energy = (float)instance.EnergyCurrent;
            MaxHealth = (int)Math.Pow(2, item.EnumItemDurabilityID);
            MaxEnergy = (int)Math.Pow(2, item.EnumItemDurabilityID);
            Health = (float)instance.HealthCurrent;
        }
    }
}
