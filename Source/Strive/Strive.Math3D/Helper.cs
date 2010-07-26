using System;

namespace Strive.Math3D
{
	/// <summary>
	/// Summary description for Helper.
	/// </summary>
	public class Helper
	{
		public static Vector3D GetHeadingFromRotation( Vector3D rotation ) {
			/*
			return new Vector3D(
				-(float)Math.Sin( rotation.Y * Math.PI/180.0 ),
				0,	
				(float)Math.Cos( rotation.Y * Math.PI/180.0 )
			);
			*/

			float A = (float)Math.Cos(rotation.X * Math.PI/180);
			float B = (float)Math.Sin(rotation.X * Math.PI/180);
			float C = (float)Math.Cos(rotation.Y * Math.PI/180);
			float D = (float)Math.Sin(rotation.Y * Math.PI/180);
			float E = (float)Math.Cos(rotation.Z * Math.PI/180);
			float F = (float)Math.Sin(rotation.Z * Math.PI/180);

			return new Vector3D(
				D, B*C, A*C
			);
		}

		static public int DivTruncate( int x, int y ) {
			return (x/y - ((x<0&&(x%y!=0))?1:0));
		}

	}
}
