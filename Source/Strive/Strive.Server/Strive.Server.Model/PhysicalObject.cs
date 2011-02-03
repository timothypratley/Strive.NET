using System;
using System.Windows.Media.Media3D;


namespace Strive.Server.Model
{
	/// <summary>
	/// Maybe there should be two different multiverses
	/// that are client/server specific... as they
	/// both store slightly different things.
	/// </summary>
	public abstract class PhysicalObject
	{
		public int ObjectInstanceID;
		public int TemplateObjectID;
		public string TemplateObjectName = "";
		public int ResourceID;
		public float Height;
        public Vector3D Position = new Vector3D(0, 0, 0);
		public Quaternion Rotation = Quaternion.Identity;
		public float HitPoints = 0;
		public int MaxHitPoints = 0;
		public float Energy = 0;
		public int MaxEnergy = 0;
		public float BoundingSphereRadiusSquared;

		public PhysicalObject() {}

		public PhysicalObject(
			Schema.TemplateObjectRow template,
			Schema.ObjectInstanceRow instance
		) {
			ObjectInstanceID = instance.ObjectInstanceID;
			TemplateObjectID = template.TemplateObjectID;
			TemplateObjectName = template.TemplateObjectName;
            Position = new Vector3D(instance.X, instance.Y, instance.Z);
			Rotation = new Quaternion(instance.RotationX,instance.RotationY,instance.RotationZ, instance.RotationW);
			ResourceID = template.ResourceID;
			Height = template.Height;
			// can we get r^2 from ResourceID?
			// TODO: make this the proper 3d radius^2
			// store it in the database
			BoundingSphereRadiusSquared = 1;
		}
	}
}
