using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Quaffable.
	/// </summary>
	public class QuaffableBase : Item
	{
		public Schema.ItemQuaffableRow quaffable;

		public QuaffableBase(
			Schema.ItemQuaffableRow quaffable,
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( item, template, instance ) {
			this.quaffable = quaffable;
		}
	}
}
