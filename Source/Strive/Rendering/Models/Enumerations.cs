using System;

namespace Strive.Rendering.Models
{
	/// <summary>
	/// Model formats
	/// </summary>
	public enum ModelFormat
	{
		/// <summary>
		/// Unspecified model format.  
		/// </summary>
		Unspecified,
		/// <summary>
		/// MDL (half-life) format
		/// </summary>
		MDL,
		/// <summary>
		/// 3D studio format
		/// </summary>
		_3DS,
		/// <summary>
		/// a mesh of terrain, don't collision detect v1.
		/// </summary>
		Terrain
	}



}
