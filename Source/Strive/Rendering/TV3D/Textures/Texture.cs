using System;
using TrueVision3D;
using Strive.Rendering.Textures;

namespace Strive.Rendering.TV3D.Textures
{
	/// <summary>
	/// Summary description for Texture.
	/// </summary>
	public class Texture : ITexture
	{

		string _name;
		int _id;
		public static ITexture LoadTexture( string name, string filename ) {
            Texture t = new Texture();
			// TODO: should be more involved in tracking/unloading textures
			t._id = Engine.Gl.GetTex( name );
			if ( t._id == 0 ) {
				t._id = Engine.TexFactory.LoadTexture( filename, name, 256, 256, CONST_TV_COLORKEY.TV_COLORKEY_NO, true, true );
			}
			t._name = name;
			return t;
		}
		public string Name {
			get { return _name; }
		}
		public int ID {
			get { return _id; }
		}
	}
}
