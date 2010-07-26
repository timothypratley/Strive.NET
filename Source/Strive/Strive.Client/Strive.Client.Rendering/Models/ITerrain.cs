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
		void SetHeight( float x, float z, float altitude );
		void SetTexture( float x, float z, ITexture texture, float rotation );
		float HeightLookup( float x, float z );
		void Clear();
		#endregion
	}
}
