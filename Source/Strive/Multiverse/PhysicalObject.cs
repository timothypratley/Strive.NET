using System;

using Strive.Math3D;

namespace Strive.Multiverse
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
		public Vector3D Position = new Vector3D(0,0,0);
		public Vector3D Rotation = new Vector3D(0,0,0);
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
			Position = new Vector3D(
				(float)instance.X,
				(float)instance.Y,
				(float)instance.Z
			);
			// todo: fix meh umg!!!
			// Shall we use teh ghey euler angles in database?
			// 1337 quarterions?
			// whatever the case, heading probabbly is not a good one.
			Rotation = new Vector3D(
				(float)instance.RotationX,
				(float)instance.RotationY,
				(float)instance.RotationZ
			);
			ResourceID = template.ResourceID;
			Height = (float)template.Height;
			// can we get r^2 from ResourceID?
			// TODO: make this the proper 3d radius^2
			// store it in the database
			BoundingSphereRadiusSquared = 100;
		}
	}
}
