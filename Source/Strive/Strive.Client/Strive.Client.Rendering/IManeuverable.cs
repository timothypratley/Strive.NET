using System;
using Strive.Math3D;

namespace Strive.Rendering
{
	/// <summary>
	/// Implement this interface to introduce an object that can be moved programtically
	/// </summary>
	public interface IManeuverable
	{
		/// <summary>
		/// The position of the object
		/// </summary>
		Vector3D Position {get; set;}
		/// <summary>
		/// The rotation of the object
		/// </summary>
		Vector3D Rotation {get; set;}
		/// <summary>
		/// Moves the object
		/// </summary>
		/// <param name="movement">The amount to move</param>
		/// <returns>A boolean indicating whether the move was successful</returns>
		bool Move(Vector3D movement);
		/// <summary>
		/// Rotates the object
		/// </summary>
		/// <param name="rotation">The amount to rotate</param>
		/// <returns>A boolean indicating whether the rotation was successful</returns>
		bool Rotate(Vector3D rotation);
	}
}
