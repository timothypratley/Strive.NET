using System;

namespace Strive.Math3D
{
	/// <summary>
	/// Summary description for Vector2d.
	/// </summary>
	public struct Vector2D
	{
		public float X;
		public float Y;

		public Vector2D( float x, float y ) {
			X = x;
			Y = y;
		}

		public void Set( float x, float y ) {
			X = x;
			Y = y;
		}
	}
}
