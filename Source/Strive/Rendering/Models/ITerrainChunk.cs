using System;
using Strive.Math3D;

using Strive.Rendering;
using Strive.Rendering.Textures;

namespace Strive.Rendering.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public interface ITerrainChunk : IModel {
		void SetHeight( float x, float z, float altitude );
		float GetHeight( float x, float z );
		void DrawTexture( ITexture t, float x, float z, float rotation );
		void Clear( float x, float z );
		void SetTexture( ITexture t );
		void SetDetailTexture( ITexture t );
		void SetClouds( ITexture t );
		void Update();
		void Render();
	}
}
