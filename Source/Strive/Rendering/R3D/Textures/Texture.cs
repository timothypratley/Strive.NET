using System;
using R3D089_VBasic;
using Strive.Rendering.Textures;

namespace Strive.Rendering.R3D.Textures
{
	/// <summary>
	/// Summary description for Texture.
	/// </summary>
	public class Texture : ITexture
	{
		string name;
		int id;

		public static ITexture LoadTexture( string name, string filename ) {
			Texture t = new Texture();
			if ( Engine.TextureLib.Class_SetPointer( name ) < 0 ) {
				R3DCOLORKEY colorkey = R3DCOLORKEY.R3DCOLORKEY_NONE;
				t.id = (short)Engine.TextureLib.Texture_Load( name, filename, ref colorkey );
			} 
			else {
				// already added
			}
			t.name = name;
			return t;
		}

		public string Name {
			get { return name; }
		}
		public int ID {
			get { return id; }
		}
	}
}
