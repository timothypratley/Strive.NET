using System;
using Revolution3D8088c;

namespace Strive.Rendering.Textures
{
	/// <summary>
	/// Summary description for Texture.
	/// </summary>
	public class Texture
	{
		public static void LoadTexture( string name, string filename ) {
			if ( Interop._instance.TextureLib.Class_SetPointer( name ) < 0 ) {
				R3DCOLORKEY colorkey = R3DCOLORKEY.R3DCOLORKEY_NONE;
				Interop._instance.TextureLib.Texture_Load( name, filename, ref colorkey );
			} 
			else {
				// already added
			}
		}
	}
}
