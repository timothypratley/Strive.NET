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
		public int AreaID;
		public Vector3D Position;
		public Vector3D Heading;
		public float HitPoints = 0;
		public int MaxHitPoints = 0;
		public float BoundingSphereRadiusSquared;

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
			// can we get r^2 from modelID?
			// todo: make this the proper 3d radius^2
			BoundingSphereRadiusSquared = 10;
			AreaID = template.AreaID;
		}
	}
}
