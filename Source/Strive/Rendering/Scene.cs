using System;
using System.Threading;
using System.Windows.Forms;

using Strive.Rendering.Models;
using Strive.Math3D;
using R3D089_VBasic;

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
		private IWin32Window _renderTarget;
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
				R3DColor color = new R3DColor();
				color.r = 30;
				color.g = 30;
				color.b = 140;
				Interop._instance.Pipeline.SetBackColor(ref color);
				Interop._instance.Pipeline.SetDithering(true);
				Interop._instance.Pipeline.SetFillMode(R3D089_VBasic.R3DFILLMODE.R3DFILLMODE_SOLID);
				R3DColor white = new R3DColor();
				white.r = 0;
				white.b = 0;
				white.g = 0;
				Interop._instance.Pipeline.SetAmbientLight(ref white);
				Interop._instance.Pipeline.SetColorKeying(true);
				Interop._instance.Pipeline.SetSpecular(true);
				Interop._instance.Pipeline.SetMipMapping(false);
				Interop._instance.Pipeline.SetShadeMode(R3DSHADEMODE.R3DSHADEMODE_GOURAUD);
				Interop._instance.Pipeline.SetTextureFilter(R3DTEXTUREFILTER.R3DTEXTUREFILTER_LINEARFILTER);
				_renderTarget = window;
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
			try {
				Interop._instance.Pipeline.Renderer_Render();
			}
			catch(Exception e) {
				throw new RenderingException("Call to 'Render()' failed with '" + e.ToString() + "'", e);
			}
			R3DVector3D namePos = new R3DVector3D();
			R3DVector2D nameLoc = new R3DVector2D();
			foreach ( Model m in Models.Values ) {
				if ( m.ShowLabel ) {
					namePos.x = m.Position.X;
					namePos.y = m.Position.Y+5;
					namePos.z = m.Position.Z;
					nameLoc = Interop._instance.Cameras.Camera_ProjectPoint( ref namePos ) ;
					// nameDist = namePos - camPos; -> set font size
					Interop._instance.Interface5D.Primitive_DrawText( ref nameLoc, m.Key );
				}
			}

//#if DEBUG
			R3DVector2D zero = new R3DVector2D();
			zero.x = 20;
			zero.y = 20;

			//R3DColor black = new R3DColor();
			//black.b = 255;
			//black.r = 255;
			//black.g = 255;
			//EEERRR setting the draw color fails to write text in 89
			//Interop._instance.Interface5D.Primitive_SetDrawColor(ref black);
	        Interop._instance.Interface5D.Primitive_DrawText(ref zero, "Fp/S: " + Interop._instance.PowerMonitor.lGetFramesPerSecond().ToString() +
                                                            ", Vertices: " + Interop._instance.PowerMonitor.lGetNumVerticesPerSinceLastFrame().ToString() + 
                                                            ", Verts/Sec:  " + (Interop._instance.PowerMonitor.lGetFramesPerSecond() * Interop._instance.PowerMonitor.lGetNumVerticesPerSinceLastFrame()).ToString() );
//			Interop._instance.Interface2D.Primitive_DrawText(0,0, (Interop._instance.PowerMonitor.lGetFramesPerSecond()).ToString());
//#endif

			/*
			try
			{
				Interop._instance.Pipeline.Renderer_Display();
			}
			catch(Exception e)
			{
				throw new RenderingException("Call to 'Display()' failed with '" + e.ToString() + "'", e);
			}
			*/
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
//			Interop._instance.Meshbuilder.Class_RayCollision(`(
//				ref vStart, ref vEnd, ct
//				);

			return 1;
		}

		#endregion
 
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
				if(!Views.Contains(Cameras.CommonCameraView.Default.ToString()))
				{
					return Cameras.Camera.CreateCamera(Cameras.CommonCameraView.Default.ToString(), this.Views);
				} else {
					return this.Views[Cameras.CommonCameraView.Default.ToString()];
				}
			}
		}
		
		public IWin32Window RenderTarget
		{
			get
			{
				return _renderTarget;
			}
		}

		#endregion

	}
}
