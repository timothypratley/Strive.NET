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
			t._id = Engine.TexFactory.LoadTexture( filename, name, 256, 256, CONST_TV_COLORKEY.TV_COLORKEY_NO, false, false );
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
