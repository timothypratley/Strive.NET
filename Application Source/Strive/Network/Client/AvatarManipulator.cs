using System;

namespace Strive.Client
{
	public class Avatar
	{
		public Avatar()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void MoveForward( float distance ) {
			x += velocity_x*distance;
			y += velocity_y*distance;
			z += velocity_z*distance;
		}

		public void Straffe( float distance ) {
			x += velocity_z*distance;
			z -= velocity_x*distance;
		}

		public void TurnDegrees( float degrees ) {
			Turn( degrees/180.0f * (float)System.Math.PI );
		}

		public void Turn( float radians ) {
			heading += radians;
			velocity_x = (float)System.Math.Sin( heading );
			velocity_z = (float)System.Math.Cos( heading );
		}

		public void SetHeadingDegrees( float degrees ) {
			SetHeading( degrees/180.0f * (float)System.Math.PI );
		}

		public float GetHeadingDegrees()
		{
			return heading*180.0f / (float)System.Math.PI;
		}

		public void SetHeading( float radians ) {
			heading = radians;
			velocity_x = (float)System.Math.Sin( heading );
			velocity_z = (float)System.Math.Cos( heading );
		}

		public void SetPosition( float new_x, float new_y, float new_z ) {
			x = new_x;
			y = new_y;
			z = new_z;
		}

		public string name;

		public float velocity_x = 0;
		public float velocity_y = 0;
		public float velocity_z = 0;

		public float heading = 0;

		public float x = 0;
		public float y = 0;
		public float z = 0;
	}
}
