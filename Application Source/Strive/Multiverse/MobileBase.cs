using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Mobile.
	/// </summary>
	public class MobileBase : PhysicalObject
	{
		public Schema.TemplateMobileRow mobile;

		public MobileBase (
			Schema.TemplateMobileRow mobile,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base ( template, instance ) {
			this.mobile = mobile;
		}
	}
}
