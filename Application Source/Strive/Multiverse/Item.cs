using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Item.
	/// </summary>
	public abstract class Item : PhysicalObject
	{
		Schema.TemplateItemRow item;

		public Item(
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( template, instance ) {
			this.item = item;
		}
	}
}
