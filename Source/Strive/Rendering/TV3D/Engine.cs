using System;
using System.Threading;
using System.Windows.Forms;

using Strive.Rendering;
using Strive.Rendering.Controls;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Rendering.TV3D.Models;
using Strive.Math3D;
using TrueVision3D;

namespace Strive.Rendering.TV3D {
	/// <summary>
	/// Creates intances of the other classes in Rendering
	/// </summary>
	public class Engine : IEngine {
		IMouse mouse = new Controls.Mouse();
		IKeyboard keyboard = new Controls.Keyboard();

		static internal TVEngine TV3DEngine;
		static internal TVInputEngine Input;
		static internal TVScene TV3DScene;
		static internal TVTextureFactory TexFactory;
		static internal TVScreen2DImmediate Screen2DImmediate;
		static internal TVScreen2DText Screen2DText;
		static internal TVLightEngine LightEngine;
		static internal TVGlobals Gl;
		static internal TVCamera Camera;
		static internal TVAtmosphere Atmosphere;
		static internal Terrain terrain = new Terrain();

		static internal int FontIndex = -1;

		IWin32Window _renderTarget;

		public Engine() {
		}


		public IScene CreateScene() {
			return new Scene();
		}

		public IViewport CreateViewport( System.Windows.Forms.IWin32Window window, string name ) {
			return new Viewport( window, name );
		}

//		public ITerrain CreateTerrain( string name, ITexture texture, float texture_rotation, float y, float xy, float zy, float xzy ) {
//			return Terrain.CreateTerrain( name, texture, texture_rotation, y, xy, zy, xzy );
//		}
		public ITerrainChunk CreateTerrainChunk( float x, float z, float gap_size, int heights ) {
			return TerrainChunk.CreateTerrainChunk( x, z, gap_size, heights );
		}

		public ITerrain GetTerrain() {
			return terrain;
		}
		public IActor LoadActor(string name, string path, float height) {
			return Actor.LoadActor( name, path, height );
		}
		public IModel LoadStaticModel(string name, string path, float height) {
			return Model.LoadStaticModel( name, path, height );
		}
		public IModel CreateBox( string name, float width, float height, float depth, ITexture texture ) {
			return Model.CreateBox( name, width, height, depth, texture );
		}
		public IModel CreatePlane( string name, ITexture texture, Vector3D p1, Vector3D p2, Vector3D p3, Vector3D p4 ) {
			return Model.CreatePlane( name, texture, p1, p2, p3, p4 );
		}

		public ITexture LoadTexture( string name, string path ) {
			return Textures.Texture.LoadTexture( name, path );
		}

		public void ForceInputUpdate() {
			Input.ForceUpdate();
		}

		public float TimeSinceLastFrame() {
			return TV3DEngine.AccurateTimeElapsed();
		}

		public IMouse Mouse {
			get { return mouse; }
		}
		public IKeyboard Keyboard {
			get { return keyboard; }
		}

		/// <summary>
		/// Initialise the scene
		/// </summary>
		/// <param name="window">The IWin32Window to render to.  System.Windows.Forms.Form implements IWin32Window</param>
		/// <param name="target">The render target</param>
		/// <param name="resolution">The resolution to render in</param>
		public void Initialise(IWin32Window window, EnumRenderTarget target, Resolution resolution) {
			if ( TV3DEngine != null ) {
				Terminate();
			}
			TV3DEngine = new TVEngine();
			try {
				Engine.TV3DEngine.Init3DWindowedMode(window.Handle.ToInt32(), true);
				//TV3DEngine.Init3DFullscreen( 800, 600, 16, true, false, CONST_TV_DEPTHMODE.TV_DEPTHBUFFER_BESTBUFFER, 1, 0 );
				_renderTarget = window;
			}
			catch(Exception e) {
				throw new EngineInitialisationException(e);
			}
			TV3DEngine.SetAngleSystem( CONST_TV_ANGLE.TV_ANGLE_DEGREE );
			// TODO:
			//TV3DEngine.SetVSync( true );
			TV3DEngine.DisplayFPS = true;
			TV3DScene = new TVScene();
			TV3DScene.SetDepthBuffer( CONST_TV_DEPTHBUFFER.TV_WBUFFER );
			//TV3DScene.SetRenderMode( CONST_TV_RENDERMODE.TV_LINE );
			TV3DScene.SetTextureFilter( CONST_TV_TEXTUREFILTER.TV_FILTER_ANISOTROPIC );
			TexFactory = new TVTextureFactory();
			Screen2DImmediate = new TVScreen2DImmediate();
			Screen2DText = new TVScreen2DText();
			LightEngine = new TVLightEngine();
			Gl = new TVGlobals();
			Camera = new TVCamera();
			Atmosphere = new TVAtmosphere();
			Atmosphere.Fog_SetParameters( 100F, 10000F, 0.0005F );
			Atmosphere.Fog_Enable( true );
			Input = new TVInputEngine();
			FontIndex = Screen2DText.TextureFont_Create("font", "Verdana", 20, true, false, false, false);
		}

		bool _fullScreen = false;
		public bool FullScreen {
			get { return _fullScreen; }
			set {
				if ( value ) {
					if ( !TV3DEngine.ResizeFullscreen( 800, 600, 16, CONST_TV_DEPTHBUFFER.TV_WBUFFER ) ) {
						throw new Exception( "Failed to set fullscreen mode" );
					}
				} else {
					if ( !TV3DEngine.ResizeDevice() ) {
						throw new Exception( "Failed to set windowed mode" );
					}
				}
				_fullScreen = value;
			}
		}

		public void Terminate() {
			if(TV3DEngine != null)
				TV3DEngine.ReleaseAll();
			TV3DEngine = null;
			if(TV3DScene != null)
				TV3DScene.DestroyAllMeshes();
			TV3DScene = null;
			if(TexFactory != null)
				TexFactory.DeleteAll();
			TexFactory = null;
			if(Screen2DImmediate != null)
				Screen2DImmediate = null;
			Screen2DText = null;
			if(LightEngine != null)
				LightEngine.DeleteAllLights();
			LightEngine = null;
			Gl = null;
			
			Camera = null;
			if(Atmosphere != null)
				Atmosphere.Unload();
			Atmosphere = null;
			if(Input != null)
				Input.UnloadDevices();
			Input = null;
		}

		public void DisableZ() {
			TV3DScene.SetDepthBuffer( 0 );
		}

		public void EnableZ() {
			TV3DScene.SetDepthBuffer( CONST_TV_DEPTHBUFFER.TV_WBUFFER );
		}

		public IWin32Window RenderTarget {
			get {
				return _renderTarget;
			}
		}
	}
}
