using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Wieldable.
	/// </summary>
	public class WieldableBase : Item
	{
		public Schema.ItemWieldableRow wieldable;

		public WieldableBase(
			Schema.ItemWieldableRow wieldable,
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( item, template, instance ) {
			this.wieldable = wieldable;
		}
	}
}
