using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Junk.
	/// </summary>
	public class Junk : Item {
		public Junk(
			Schema.ItemJunkRow junk,
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( item, template, instance ) {
		}
	}
}
