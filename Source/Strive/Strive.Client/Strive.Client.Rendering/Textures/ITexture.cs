using System;

namespace Strive.Rendering.Textures
{
	/// <summary>
	/// Summary description for Texture.
	/// </summary>
	public interface ITexture
	{
		string Name { get; }
		int ID { get; }
		int Width { get; }
		int Height { get; }
		void Draw( ITexture t, float x, float y, float rotation, float scale );
		void Clear( float x, float y, float width, float height );
	}
}
