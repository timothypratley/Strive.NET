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
		static internal TVLandscape Land;
		static internal TVTextureFactory TexFactory;
		static internal TVScreen2DImmediate Screen2DImmediate;
		static internal TVScreen2DText Screen2DText;
		static internal TVLightEngine LightEngine;
		static internal TVGlobals Gl;
		static internal TVCamera Camera;
		static internal TVAtmosphere Atmosphere;

		static internal int FontIndex = -1;

		IWin32Window _renderTarget;

		public Engine() {
		}


		public IScene CreateScene() {
			return new Scene();
		}

		public ITerrain CreateTerrain( string name, ITexture texture, float y, float xy, float zy, float xzy ) {
			return Terrain.CreateTerrain( name, texture, y, xy, zy, xzy );
		}
		public IActor LoadActor(string name, string path, float height) {
			return Actor.LoadActor( name, path, height );
		}
		public IModel LoadStaticModel(string name, string path, float height) {
			return Model.LoadStaticModel( name, path, height );
		}

		public ITexture LoadTexture( string name, string path ) {
			return Textures.Texture.LoadTexture( name, path );
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
				_renderTarget = window;
			}
			catch(Exception e) {
				throw new EngineInitialisationException(e);
			}
			TV3DEngine.SetAngleSystem( CONST_TV_ANGLE.TV_ANGLE_DEGREE );
			TV3DEngine.SetVSync( true );
			TV3DScene = new TVScene();
			Land = new TVLandscape();
			TexFactory = new TVTextureFactory();
			Screen2DImmediate = new TVScreen2DImmediate();
			Screen2DText = new TVScreen2DText();
			LightEngine = new TVLightEngine();
			Gl = new TVGlobals();
			Camera = new TVCamera();
			Atmosphere = new TVAtmosphere();
			Input = new TVInputEngine();
			FontIndex = Screen2DText.TextureFont_Create("font", "Verdana", 20, true, false, false, false);
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
			if(Land != null)
				Land.DeleteAll();
			Land = null;
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

		public IWin32Window RenderTarget {
			get {
				return _renderTarget;
			}
		}

	}
}
