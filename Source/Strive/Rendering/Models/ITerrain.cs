using System;
using Strive.Math3D;

using Strive.Rendering;
using Strive.Rendering.Textures;

namespace Strive.Rendering.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public interface ITerrain : IModel {
		#region "Methods"
		void applyTexture( ITexture texture );
		float HeightLookup( float x, float z );
		#endregion
	}
}
