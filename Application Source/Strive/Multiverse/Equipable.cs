using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Equipable.
	/// </summary>
	public class Equipable : Item
	{
		//EnumWearLocation WearLocationID;
		int ArmourClass;

		public Equipable(
			Schema.ItemEquipableRow equipable,
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( item,	template, instance ) {
			ArmourClass = equipable.ArmourClass;
			//EnumWearLocationID = equipable.EnumWearLocationID;
		}
	}
}
