using System;

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
		public float X, Y, Z;
		public float HeadingX, HeadingY, HeadingZ;

		public PhysicalObject(
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) {
			ObjectInstanceID = instance.ObjectInstanceID;
			ObjectTemplateID = template.ObjectTemplateID;
			ObjectTemplateName = template.ObjectTemplateName;
			X = (float)instance.X;
			Y = (float)instance.Y;
			Z = (float)instance.Z;
			HeadingX= (float)instance.HeadingX;
			HeadingY = (float)instance.HeadingY;
			HeadingZ = (float)instance.HeadingZ;
			ModelID = template.ModelID;
		}
	}
}
