using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Item.
	/// </summary>
	public abstract class Item : PhysicalObject
	{
		public int Value;
		public int Weight;

		public Item(){}
		public Item(
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( template, instance ) {
			Value = item.Value;
			Weight = item.Weight;
			Energy = (float)instance.EnergyCurrent;
			MaxHitPoints = (int)Math.Pow( 2, item.EnumItemDurabilityID );
			MaxEnergy = (int)Math.Pow( 2, item.EnumItemDurabilityID );
			HitPoints = (float)instance.HitpointsCurrent;
		}
	}
}
