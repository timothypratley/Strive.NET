using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Quaffable.
	/// </summary>
	public class Quaffable : Item
	{
		public Quaffable(){}
		public Quaffable(
			Schema.ItemQuaffableRow quaffable,
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( item, template, instance ) {
		}
	}
}
