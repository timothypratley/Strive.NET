using System;

namespace Strive.Server.Model
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
			Schema.TemplateItemEquipableRow equipable,
			Schema.TemplateItemRow item,
			Schema.TemplateObjectRow template,
			Schema.ObjectInstanceRow instance
		) : base( item,	template, instance ) {
			ArmourClass = equipable.ArmourClass;
			WearLocationID = (EnumWearLocation)equipable.EnumWearLocationID;
		}
	}
}
