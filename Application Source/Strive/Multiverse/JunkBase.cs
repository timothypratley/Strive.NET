using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Junk.
	/// </summary>
	public class JunkBase : Item {
		public Schema.ItemJunkRow junk;

		public JunkBase(
			Schema.ItemJunkRow junk,
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( item, template, instance ) {
			this.junk = junk;
		}
	}
}
