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
				t._id = Engine.TexFactory.LoadTexture( filename, name, 256, 256, CONST_TV_COLORKEY.TV_COLORKEY_BLACK, false, true );
			}
			t._name = name;
			return t;
		}
		public static Texture invisible;
		public static ITexture GetInvisible() {
			if ( invisible != null ) return invisible;
			invisible = new Texture();
			// TODO: what dimensions?
			int fire_width = 2;
			int fire_height = 2;
			TVRenderSurface rs = Engine.TV3DScene.CreateRenderSurface(fire_width,fire_height,false,0,0);
			rs.StartRender( true );
			Engine.Screen2DImmediate.ACTION_Begin2D();
			int i, j, cc;
			for ( i=1; i<fire_width-1; i++ ) {
				for ( j=fire_height-2; j>=0; j-- ) {
					cc = Engine.Gl.RGBA(0,0,0,0);
					Engine.Screen2DImmediate.DRAW_Point( i, j, cc );
				}
			}
			Engine.Screen2DImmediate.ACTION_End2D();
			rs.EndRender();
			invisible._id = rs.GetTexture();
			invisible._name = "invisible";
			rs.Destroy();
			return invisible;
		}

		public string Name {
			get { return _name; }
		}
		public int ID {
			get { return _id; }
		}
	}
}
