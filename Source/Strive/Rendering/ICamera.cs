using System;
using Strive.Math3D;

namespace Strive.Rendering
{
	/// <summary>
	/// Represents the current view of the scene
	/// </summary>
	public interface ICamera : IManeuverable
	{

		#region "Operators

		#endregion

		#region "Properties"
		/// <summary>
		/// The depth of field (width of vision) for the camera
		/// </summary>
		float FieldOfView {	get; set; }

		/// <summary>
		/// The view distance for the camera
		/// </summary>
		float ViewDistance { get; set; }

		#endregion

		#region "Methods"
		Vector2D ProjectPoint( Vector3D point );

		#endregion
	}
}
