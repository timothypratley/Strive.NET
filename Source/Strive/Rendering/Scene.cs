using System;
using System.Threading;
using System.Windows.Forms;

using Strive.Rendering.Models;
using Strive.Math3D;
using Revolution3D8088c;

namespace Strive.Rendering
{

	/// <summary>
	/// Represent a Scene.  A scene contains everything that will be rendered.
	/// </summary>
	public class Scene
	{
		#region "Private fields"		
		// Singleton support
		private static bool _constructed = false;
		private bool _initialised;
		private bool _isRendering;
		private ModelCollection _models = new ModelCollection();
		private Cameras.CameraCollection _views = new Cameras.CameraCollection();
		#endregion

		#region "Constructors"
		/// <summary>
		/// Creates a new Scene
		/// </summary>
		public Scene()
		{
			// Singleton support
			if(Scene._constructed)
			{
				throw new SceneAlreadyExistsException();
			}			

			Scene._constructed = true;

		}
		/// <summary>
		/// Destuctor for Scene.  Unsets static _constructed field
		/// </summary>
		~Scene()
		{
			if(_initialised)
			{
				Interop._instance.Engine.TerminateMe();
			}
			Scene._constructed = false;
		}
		#endregion

		#region "Methods"
		/// <summary>
		/// Initialise the scene
		/// </summary>
		/// <param name="window">The IWin32Window to render to.  System.Windows.Forms.Form implements IWin32Window</param>
		/// <param name="target">The render target</param>
		/// <param name="resolution">The resolution to render in</param>
		public void Initialise(IWin32Window window, RenderTarget target, Resolution resolution)
		{
			try
			{
				R3DRENDERTARGET r3dtarget = Interop._instance[target];
				Interop._instance.Engine.Inf_SetRenderTarget(window.Handle.ToInt32(), ref r3dtarget);
				if(resolution != Resolution.Automatic)
				{
					Interop._instance.Engine.Inf_ForceResolution(resolution.Width, resolution.Height, resolution.ColourDepth);
				}

				Interop._instance.Engine.InitializeMe(true);
				Interop._instance.Pipeline.SetBackColor(30,30,140);
				Interop._instance.Pipeline.SetDithering(true);
				Interop._instance.Pipeline.SetFillMode(Revolution3D8088c.R3DFILLMODE.R3DFILLMODE_SOLID);
				Interop._instance.Pipeline.SetAmbientLight(0,0,0);
				Interop._instance.Pipeline.SetColorKeying(true);
				Interop._instance.Pipeline.SetSpecular(true);
				Interop._instance.Pipeline.SetMipMapping(false);
				Interop._instance.Pipeline.SetShadeMode(R3DSHADEMODE.R3DSHADEMODE_GOURAUD);
				Interop._instance.Pipeline.SetTextureFilter(R3DTEXTUREFILTER.R3DTEXTUREFILTER_LINEARFILTER);
			}
			catch(Exception e)
			{
				throw new EngineInitialisationException(e);
			}
			_initialised = true;
		}

		public void DropAll() {
			_models = new ModelCollection();
			_views = new Cameras.CameraCollection();
			if(_initialised) {
				Interop._instance.Engine.TerminateMe();
			}
			_initialised = false;
		}

		public void SetSky( string name, string texture ) {
			if ( Interop._instance.Skydome.Class_SetPointer( name ) < 0 ) {
				Interop._instance.Skydome.Item_Create( name, R3DSKYDOMEITEM.R3DSKYDOMEITEM_SPHERE, 2000 );
			}
			Interop._instance.Skydome.Item_SetTexture( 0, texture );
		}

		public void SetLighting( short level ) {
			R3DColor color;
			color.r = level;
			color.g = level;
			color.b = level;
			color.a = level;
			//Interop._instance.Pipeline.SetAmbientLight( level, level, level );
			Interop._instance.Pipeline.SetGammaLevel( ref color );
		}

		public void SetFog( float level ) {
			R3DColor FogColor;
			FogColor.r = (short)(level);
			FogColor.g = (short)(level);
			FogColor.b = (short)(level);
			FogColor.a = (short)(level);
			Interop._instance.Pipeline.SetFog( ref FogColor, R3DFOGTYPE.R3DFOGTYPE_PIXELTABLE, R3DFOGMODE.R3DFOGMODE_LINEAR, 0, 200, level );
		}

		/// <summary>
		/// Public rendering routine
		/// </summary>
		/// <remarks>This method renders the scene into video memory</remarks>
		public void Render()
		{
			checkInitialised();
			try
			{
				Interop._instance.Pipeline.Renderer_Clear();
			}
			catch(Exception e)
			{
				throw new RenderingException("Call to 'Clear()' failed", e);
			}
#if DEBUG
			Interop._instance.Interface2D.Primitive_DrawText(0,0, (Interop._instance.PowerMonitor.GetFrameTime() / Interop._instance.PowerMonitor.GetElapsedFrames() ).ToString());
#endif

			try
			{
				Interop._instance.Pipeline.Renderer_Render();
			}
			catch(Exception e)
			{
				throw new RenderingException("Call to 'Render()' failed with '" + e.ToString() + "'", e);
			}
		}

		/// <summary>
		/// Displays the rendered screen
		/// </summary>
		public void Display()
		{
			checkInitialised();

			try
			{
				Interop._instance.Pipeline.Renderer_Display();
			}
			catch(Exception e)
			{
				throw new RenderingException("Call to 'Display()' failed", e);
			}
		}

		private void checkInitialised()
		{
			if(!_initialised)
			{
				throw new SceneNotInitialisedException();
			}
		}

		public int RayCollision(
			Vector3D start_point, Vector3D end_point, int collision_type
			) 
		{
			// map collision_type to r3d collision type omg :(
			R3DCOLLISIONTYPE ct = (R3DCOLLISIONTYPE) collision_type;
			R3DVector3D vStart;
			vStart.x = start_point.X;
			vStart.y = start_point.Y;
			vStart.z = start_point.Z;
			R3DVector3D vEnd;
			vEnd.x = end_point.X;
			vEnd.y = end_point.Y;
			vEnd.z = end_point.Z;
			// if (
			Interop._instance.Meshbuilder.Class_RayCollision(
				ref vStart, ref vEnd, ct
				);

			return 1;
		}

		#endregion
 
		public void addsky()
		{
			R3DCOLORKEY r = R3DCOLORKEY.R3DCOLORKEY_NONE;
			Interop._instance.TextureLib.Texture_Load("sky_panaroma", @"C:\projects\Revolution.NET\Tutorial17\Media\sky_panorama.bmp", ref r );

			Interop._instance.Skydome.Item_Create("sky_panaromasphere", R3DSKYDOMEITEM.R3DSKYDOMEITEM_SPHERE, 200);
 
			Interop._instance.Skydome.Item_SetTexture(0, "sky_panaroma");



		}
	
		#region "Properties"
		/// <summary>
		/// Indiactes whether the scene is being rendered
		/// </summary>
		public bool IsRendering
		{
			get
			{
				checkInitialised();
				return _isRendering;
			}
			set
			{
				checkInitialised();
				_isRendering = value;
			}
		}

		/// <summary>
		/// Model collection
		/// </summary>
		public ModelCollection Models
		{
			get
			{
				return _models;
			}
		}

		/// <summary>
		/// Returns the View of the current scene.
		/// </summary>
		public Cameras.CameraCollection Views
		{
			get
			{
				checkInitialised();
				return _views;
			}
			set
			{
				checkInitialised();
				_views = value;
			}

		}

		/// <summary>
		/// Returns the default view
		/// </summary>
		public Cameras.Camera View
		{
			get
			{
				checkInitialised();
				if(!Views.Contains("DefaultView"))
				{
					return Cameras.Camera.CreateCamera("DefaultView", this.Views);
					}
				else
				{
					return this.Views["DefaultView"];
				}
			}
		}

		#endregion

	}
}