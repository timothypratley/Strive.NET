using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Readable.
	/// </summary>
	public class ReadableBase : Item
	{
		public Schema.ItemReadableRow readable;

		public ReadableBase(
			Schema.ItemReadableRow readable,
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( item, template, instance ) {
			this.readable = readable;
		}
	}
}
