using System;

namespace Strive.Server.Schema
{
	/// <summary>
	/// Summary description for Quaffable.
	/// </summary>
	public class Quaffable : Item
	{
		public Quaffable(){}
		public Quaffable(
			Schema.TemplateItemQuaffableRow quaffable,
			Schema.TemplateItemRow item,
			Schema.TemplateObjectRow template,
			Schema.ObjectInstanceRow instance
		) : base( item, template, instance ) {
		}
	}
}
