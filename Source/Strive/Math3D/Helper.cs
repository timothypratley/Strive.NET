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
			if ( x == 0.0 && y == 0.0 && z == 0.0 ) {
				return new Vector3D(0, 0, 0);
			}
			double dFlat = Math.Sqrt( x * x + z * z );
			double yTheta;
			if ( z == 0.0 ) {
				yTheta = x > 0.0 ? -Math.PI/2.0 : Math.PI/2.0;	
			} else if ( z > 0.0 ) {
				yTheta = Math.Atan( -x/z );
			} else {
				yTheta = Math.Atan( -x/z ) - Math.PI;
			}
			return new Vector3D( 0, (float)(yTheta*180.0/Math.PI), 0 );
		}

		public static Vector3D GetHeadingFromRotation( Vector3D rotation ) {
			return new Vector3D(
				-(float)Math.Sin( rotation.Y * Math.PI/180.0 ),
				0,	
				(float)Math.Cos( rotation.Y * Math.PI/180.0 )
			);
		}
	}
}
