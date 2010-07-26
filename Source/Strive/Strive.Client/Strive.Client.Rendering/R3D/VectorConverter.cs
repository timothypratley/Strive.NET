using System;
using Strive.Math3D;
using Strive.Rendering.R3D;
using R3D089_VBasic;

namespace Strive.Rendering.R3D
{
	/// <summary>
	/// Contains Vector Conversion routines
	/// </summary>
	public class VectorConverter
	{



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
