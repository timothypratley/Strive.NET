using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for PhysicalObject.
	/// </summary>
	public abstract class PhysicalObject
	{
		public Schema.ObjectInstanceRow instance;
		public Schema.ObjectTemplateRow template;

		public PhysicalObject(
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) {
			this.instance = instance;
			this.template = template;
		}
	}
}
