using System;
using Strive.Math3D;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for PhysicalObject.
	/// </summary>
	public abstract class PhysicalObject
	{
		public int ObjectInstanceID;
		public int ObjectTemplateID;
		public string ObjectTemplateName;
		public int ModelID;
		public Vector3D Position;
		public Vector3D Heading;

		public PhysicalObject(
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) {
			ObjectInstanceID = instance.ObjectInstanceID;
			ObjectTemplateID = template.ObjectTemplateID;
			ObjectTemplateName = template.ObjectTemplateName;
			Position = new Vector3D(
				(float)instance.X,
				(float)instance.Y,
				(float)instance.Z
			);
			Heading = new Vector3D(
				(float)instance.HeadingX,
				(float)instance.HeadingY,
				(float)instance.HeadingZ
			);
			ModelID = template.ModelID;
		}
	}
}
