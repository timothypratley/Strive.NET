using System;
using Strive.Math3D;
using Strive.Rendering;
using Revolution3D8088c;

namespace Strive.Rendering
{
	/// <summary>
	/// Contains Vector Conversion routines
	/// </summary>
	public class VectorConverter
	{

		public static R3DPoint3D GetR3DPoint3DFromVector3D(Vector3D vector)
		{
			R3DPoint3D r;
			r.x = vector.X;
			r.y = vector.Y;
			r.z = vector.Z;
			return r;
		}

		public static R3DVector3D GetR3DVector3DFromVector3D(Vector3D vector)
		{
			R3DVector3D r;
			r.x = vector.X;
			r.y = vector.Y;
			r.z = vector.Z;
			return r;
		}

	}
}
