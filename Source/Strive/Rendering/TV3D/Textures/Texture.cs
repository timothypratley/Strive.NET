using System;
using TrueVision3D;
using Strive.Rendering.Textures;
using Strive.Common;

namespace Strive.Rendering.TV3D.Textures
{
	/// <summary>
	/// Summary description for Texture.
	/// </summary>
	public class Texture : ITexture
	{

		string _name;
		int _id;
		int _width;
		int _height;
		TVRenderSurface _render_surface = null;

		public static ITexture LoadTexture( string name, string filename ) {
            Texture t = new Texture();
			// TODO: should be more involved in tracking/unloading textures
			t._id = Engine.Gl.GetTex( name );
			t._width = Constants.terrainPieceTextureWidth;
			t._height = Constants.terrainPieceTextureWidth;
			if ( t._id == 0 ) {
				t._id = Engine.TexFactory.LoadTexture( filename, name, 256, 256, CONST_TV_COLORKEY.TV_COLORKEY_BLACK, false, true );
			}
			t._name = name;
			return t;
		}

		public static ITexture CreateTexture( string name, int width, int height ) {
			Texture t = new Texture();
			t._name = name;
			t._render_surface = Engine.TV3DScene.CreateAlphaRenderSurface( width, height, false );
			t._width = width;
			t._height = height;
			t._id = t._render_surface.GetTexture();
			return t;
		}

		public void Draw( ITexture t, float x, float y, float rotation, float scale ) {
			_render_surface.StartRender(false);
			Engine.Screen2DImmediate.ACTION_Begin2D();
			int white = Engine.Gl.RGBA( 1, 1, 1, 1 );
// T00 slow!
//			Engine.Screen2DImmediate.DRAW_TextureRotated( t.ID, x, y, t.Width*scale, t.Height*scale, rotation, 0, 0, 1, 1, white, white, white, white );
			Engine.Screen2DImmediate.DRAW_Texture( t.ID, x, y, x+Constants.terrainPieceTextureWidth, y+Constants.terrainPieceTextureWidth, white, white, white, white, 0, 0, 1, 1 );
			Engine.Screen2DImmediate.ACTION_End2D();
			_render_surface.EndRender();
		}

		public void Clear( float x, float y, float width, float height ) {
			_render_surface.StartRender(false);
			Engine.Screen2DImmediate.ACTION_Begin2D();
			Engine.Screen2DImmediate.SETTINGS_SetBlendingMode( DxVBLibA.CONST_D3DBLEND.D3DBLEND_SRCALPHA, DxVBLibA.CONST_D3DBLEND.D3DBLEND_ZERO, true );
			int invis = Engine.Gl.RGBA( 0, 0, 0, 0 );
			Engine.Screen2DImmediate.DRAW_FilledBox( x, y, x+width, y+height, invis, invis, invis, invis );
			Engine.Screen2DImmediate.SETTINGS_SetBlendingMode( DxVBLibA.CONST_D3DBLEND.D3DBLEND_SRCALPHA, DxVBLibA.CONST_D3DBLEND.D3DBLEND_INVSRCALPHA, true );
			Engine.Screen2DImmediate.ACTION_End2D();
			_render_surface.EndRender();
		}

		public static Texture invisible;
		static int fire_width = 32;
		static int fire_height = 32;
		static TVRenderSurface rs = Engine.TV3DScene.CreateRenderSurface(fire_width,fire_height,false,0,0);
		public static ITexture GetInvisible() {
			if ( invisible != null ) return invisible;
			invisible = new Texture();
			// TODO: what dimensions?
			rs.StartRender( true );
			int i, j, cc;
			cc = Engine.Gl.RGBA(0,0,0,0);
			Engine.Screen2DImmediate.ACTION_Begin2D();
			for ( i=0; i<fire_width; i++ ) {
				for ( j=0; j<fire_height; j++ ) {
					Engine.Screen2DImmediate.DRAW_Point( i, j, cc );
				}
			}
			Engine.Screen2DImmediate.ACTION_End2D();
			rs.EndRender();
			invisible._id = rs.GetTexture();
			invisible._name = "invisible";
			//rs.Destroy();
			return invisible;
		}

		public string Name {
			get { return _name; }
		}
		public int ID {
			get { return _id; }
		}
		public int Width {
			get { return _width; }
		}
		public int Height {
			get { return _height; }
		}
	}
}
