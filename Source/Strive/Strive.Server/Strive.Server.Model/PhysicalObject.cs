using System;
using System.Windows.Media.Media3D;


namespace Strive.Server.Model
{
	public abstract class PhysicalObject
	{
		public int ObjectInstanceId;
		public int TemplateObjectId;
		public string TemplateObjectName = "";
		public int ResourceId;
		public float Height;
        public Vector3D Position = new Vector3D(0, 0, 0);
		public Quaternion Rotation = Quaternion.Identity;
		public float HitPoints;
		public int MaxHitPoints;
		public float Energy;
		public int MaxEnergy;
		public float BoundingSphereRadiusSquared;

		public PhysicalObject() {}

		public PhysicalObject(
			Schema.TemplateObjectRow template,
			Schema.ObjectInstanceRow instance
		) {
			ObjectInstanceId = instance.ObjectInstanceID;
			TemplateObjectId = template.TemplateObjectID;
			TemplateObjectName = template.TemplateObjectName;
            Position = new Vector3D(instance.X, instance.Y, instance.Z);
			Rotation = new Quaternion(instance.RotationX,instance.RotationY,instance.RotationZ, instance.RotationW);
			ResourceId = template.ResourceID;
			Height = template.Height;
			// can we get r^2 from ResourceID?
			// TODO: make this the proper 3d radius^2
			// store it in the database
			BoundingSphereRadiusSquared = 1;
		}
	}
}
