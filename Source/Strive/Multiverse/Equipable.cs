using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Equipable.
	/// </summary>
	public class Equipable : Item
	{
		public EnumWearLocation WearLocationID;
		public int ArmourClass;

		public Equipable(){}
		public Equipable(
			Schema.ItemEquipableRow equipable,
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( item,	template, instance ) {
			ArmourClass = equipable.ArmourClass;
			WearLocationID = (EnumWearLocation)equipable.EnumWearLocationID;
		}
	}
}
