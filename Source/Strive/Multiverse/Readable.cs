using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Readable.
	/// </summary>
	public class Readable : Item
	{
		public Readable(){}
		public Readable(
			Schema.ItemReadableRow readable,
			Schema.TemplateItemRow item,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base( item, template, instance ) {
		}
	}
}
