using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Junk.
	/// </summary>
	public class Junk : Item {
		public Junk(){}
		public Junk(
			Schema.TemplateItemJunkRow junk,
			Schema.TemplateItemRow item,
			Schema.TemplateObjectRow template,
			Schema.ObjectInstanceRow instance
		) : base( item, template, instance ) {
		}
	}
}
