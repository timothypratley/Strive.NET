using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Equipable.
	/// </summary>
	public class EquipableBase : Item
	{
		public Schema.ItemEquipableRow equipable;

		public EquipableBase(
			Schema.ItemEquipableRow equipable,
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( item,	template, instance ) {
			this.equipable = equipable;
		}
	}
}
