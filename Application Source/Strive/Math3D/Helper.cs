using System;

namespace Strive.Math3D
{
	/// <summary>
	/// Summary description for Helper.
	/// </summary>
	public class Helper
	{
		public static Vector3D GetRotationFromHeading( float x, float y, float z ) {
			// Rotation, convert from heading to Euler angles
			// protect from divide by zero,
			// and convert to degrees, and float.
			if ( x == 0 && y == 0 && z == 0 ) {
				return new Vector3D(0, 0, 0);
			}
			double dFlat = Math.Sqrt( x * x + z * z );
			double xTheta;
			if ( dFlat == 0.0 ) {
				xTheta = y > 0 ? Math.PI/2.0 : -Math.PI/2.0;
			} else {
				xTheta = Math.Atan( y/dFlat );
			}
			double yTheta;
			if ( z == 0.0 ) {
				yTheta = x > 0 ? -Math.PI/2.0 : Math.PI/2.0;	
			} else {
				yTheta = Math.Atan( x/z );
				if ( x == 0 && z < 0 ) {
					yTheta = Math.PI;
				}
			}
			return new Vector3D( (float)(xTheta*180.0/Math.PI), (float)(yTheta*180.0/Math.PI), 0.0F );
		}

		public static Vector3D GetHeadingFromRotation( Vector3D rotation ) {
			return new Vector3D(
				(float)Math.Sin( rotation.Y * Math.PI/180.0 ),
				(float)Math.Sin( rotation.X * Math.PI/180.0 ),	
				(float)Math.Cos( rotation.Y * Math.PI/180.0 )
				);
		}
	}
}
