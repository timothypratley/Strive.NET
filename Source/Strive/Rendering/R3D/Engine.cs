using System;
using System.Threading;
using System.Windows.Forms;

using Strive.Rendering;
using Strive.Rendering.Controls;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Rendering.R3D.Models;
using Strive.Math3D;
using R3D089_VBasic;

namespace Strive.Rendering.R3D {
	/// <summary>
	/// Creates intances of the other classes in Rendering
	/// </summary>
	public class Engine : IEngine {
		IMouse mouse = new Controls.Mouse();
		IKeyboard keyboard = new Controls.Keyboard();

		static internal R3D_Engine R3DEngine = new R3D_Engine();
		static internal R3D_Control Control = new R3D_Control();
		static internal R3D_Pipeline Pipeline = new R3D_Pipeline();
		static internal R3D_MeshBuilder2 MeshBuilder = new R3D_MeshBuilder2();
		static internal R3D_Cameras Cameras = new R3D_Cameras();
		static internal R3D_MD2System MD2System = new R3D_MD2System();
		static internal R3D_SkyDome2 Skydome = new R3D_SkyDome2();
		static internal R3D_TextureLib TextureLib = new R3D_TextureLib();
		static internal R3D_MaterialLib MaterialLib = new R3D_MaterialLib();
		static internal R3D_PolyVox3 PolyVox = new R3D_PolyVox3();
		static internal R3D_Interface5D Interface5D = new R3D_Interface5D();
		static internal R3D_PowerMonitor PowerMonitor = new R3D_PowerMonitor();

		IWin32Window _renderTarget;

		public Engine() {
		}
		~Engine() {
			Terminate();
		}


		public IScene CreateScene() {
			return new Scene();
		}

		public ITerrain CreateTerrain( string name, ITexture texture, float y, float xy, float zy, float xzy ) {
			return Terrain.CreateTerrain( name, texture );
		}
		public IActor LoadActor(string key, string path, float height) {
			return Actor.Load( key, path, height );
		}
		public IModel LoadStaticModel(string key, string path, float height) {
			return Model.Load( key, path, height );
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
			try {
				R3DRENDERTARGET r3dtarget = convertRenderTarget( target );
				Engine.R3DEngine.Inf_SetRenderTarget(window.Handle.ToInt32(), ref r3dtarget);
				if(resolution != Resolution.Automatic) {
					Engine.R3DEngine.Inf_ForceResolution(resolution.Width, resolution.Height, resolution.ColourDepth);
				}
				Engine.R3DEngine.InitializeMe( false );

				R3DColor color = new R3DColor();
				color.r = 30;
				color.g = 30;
				color.b = 140;
				Engine.Pipeline.SetBackColor(ref color);
				Engine.Pipeline.SetDithering(true);
				Engine.Pipeline.SetFillMode(R3D089_VBasic.R3DFILLMODE.R3DFILLMODE_SOLID);
				R3DColor white = new R3DColor();
				white.r = 128;
				white.b = 128;
				white.g = 128;
				Engine.Pipeline.SetAmbientLight(ref white);
				Engine.Pipeline.SetColorKeying(true);
				Engine.Pipeline.SetSpecular(true);
				Engine.Pipeline.SetMipMapping(false);
				Engine.Pipeline.SetShadeMode(R3DSHADEMODE.R3DSHADEMODE_GOURAUD);
				Engine.Pipeline.SetTextureFilter(R3DTEXTUREFILTER.R3DTEXTUREFILTER_LINEARFILTER);
				_renderTarget = window;
			}
			catch(Exception e) {
				throw new EngineInitialisationException(e);
			}
		}
		public void Terminate() {
			Engine.R3DEngine.TerminateMe();
		}
		public IWin32Window RenderTarget {
			get {
				return _renderTarget;
			}
		}
		/// <summary>
		/// Converts Strive.Rendering.R3D.RenderTarget instances to the appropriate underlying instance
		/// </summary>
		internal R3DRENDERTARGET convertRenderTarget( EnumRenderTarget target ) {
			switch(target) {
				case EnumRenderTarget.Window: {
					return R3DRENDERTARGET.R3DRENDERTARGET_WINDOW;
				}
				case EnumRenderTarget.FullScreen: {
					return R3DRENDERTARGET.R3DRENDERTARGET_FULLSCREEN;
				}
				case EnumRenderTarget.PictureBox: {
					return R3DRENDERTARGET.R3DRENDERTARGET_PICTUREBOX;
				}
			}
			return new R3DRENDERTARGET();
		}

	}
}
