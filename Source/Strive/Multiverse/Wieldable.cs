using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Wieldable.
	/// </summary>
	public class Wieldable : Item
	{
		public Schema.ItemWieldableRow wieldable;

		public Wieldable(){}
		public Wieldable(
			Schema.ItemWieldableRow wieldable,
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( item, template, instance ) {
			this.wieldable = wieldable;
		}
	}
}
