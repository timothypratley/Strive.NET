using System;
using System.Threading;
using System.Windows.Forms;

using Strive.Rendering;
using Strive.Rendering.Cameras;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Rendering.TV3D.Models;
using Strive.Math3D;
using TrueVision3D;

namespace Strive.Rendering.TV3D
{

	/// <summary>
	/// Represent a Scene.  A scene contains everything that will be rendered.
	/// </summary>
	public class Scene : IScene
	{
		#region "Private fields"		
		// Singleton support
		private static bool _constructed = false;
		private bool _isRendering = false;
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
			Scene._constructed = false;
		}
		#endregion

		#region "Methods"
		public void DropAll() {
			_models = new ModelCollection();
			_views = new Cameras.CameraCollection();
		}

		public void SetSky( string name, ITexture texture ) {
			Engine.Atmosphere.SkySphere_SetTexture( texture.ID );
			Engine.Atmosphere.SkySphere_Enable( true );
		}

		public void SetLighting( short level ) {
		}

		public void SetFog( float level ) {
		}

		public void DrawText( Vector2D location, string message ) {
			/*
			Engine.Screen2DText.TextureFont_DrawBillboardText( message, location.X, location.Y, location.Z, 0, 0, 1, 1 );
			*/

			/* need a render surface :(
			Engine.Screen2DText.ACTION_BeginText();
			Engine.Screen2DText.NormalFont_DrawText( message, location.X, location.Y, Engine.Gl.RGBA(1f, 0f, 1f, 1f), "TV" );
			Engine.Screen2DText.ACTION_EndText();
			*/
		}

		/// <summary>
		/// Public rendering routine
		/// </summary>
		/// <remarks>This method renders the scene into video memory</remarks>
		public void Render()
		{
			try
			{
				Engine.TV3DEngine.Clear( false );
			}
			catch(Exception e)
			{
				throw new RenderingException("Call to 'Clear()' failed", e);
			}
			try {
				Engine.Atmosphere.Atmosphere_Render();
				Engine.TV3DScene.RenderAllMeshes( false );
				foreach( IModel m in _models.Values ) {
					if ( m is Actor ) {
						((Actor)m).Render();
					}
				}
			}
			catch(Exception e) {
				throw new RenderingException("Call to 'Render()' failed with '" + e.ToString() + "'", e);
			}

//#if DEBUG
			//R3DColor black = new R3DColor();
			//black.b = 255;
			//black.r = 255;
			//black.g = 255;
			//EEERRR setting the draw color fails to write text in 89
			//Engine.Interface5D.Primitive_SetDrawColor(ref black);
	        //ngine.Screen2D.DrawText(ref zero, "Fp/S: " + Engine.PowerMonitor.lGetFramesPerSecond().ToString() +
//                                                            ", Vertices: " + Engine.PowerMonitor.lGetNumVerticesPerSinceLastFrame().ToString() + 
                                                            //", Verts/Sec:  " + (Engine.PowerMonitor.lGetFramesPerSecond() * Engine.PowerMonitor.lGetNumVerticesPerSinceLastFrame()).ToString() );
//			Engine.Interface2D.Primitive_DrawText(0,0, (Engine.PowerMonitor.lGetFramesPerSecond()).ToString());
//#endif

			/*
			try
			{
				Engine.Pipeline.Renderer_Display();
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
			try
			{
				Engine.TV3DEngine.RenderToScreen();
			}
			catch(Exception e)
			{
				throw new RenderingException("Call to 'Display()' failed", e);
			}
		}

		public int RayCollision(
			Vector3D start_point, Vector3D end_point, int collision_type
		) {
			return 1;
		}

		public ICameraCollection CameraCollection {
			get { return _views; }
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
				return _isRendering;
			}
			set
			{
				_isRendering = value;
			}
		}

		/// <summary>
		/// Model collection
		/// </summary>
		public IModelCollection Models
		{
			get
			{
				return _models;
			}
		}

		/// <summary>
		/// Returns the View of the current scene.
		/// </summary>
		public ICameraCollection Views
		{
			get
			{
				return _views;
			}
		}

		/// <summary>
		/// Returns the default view
		/// </summary>
		public ICamera View
		{
			get
			{
				if(!Views.Contains(EnumCommonCameraView.Default.ToString()))
				{
					return Views.CreateCamera( EnumCommonCameraView.Default );
				} else {
					return (ICamera)this.Views[EnumCommonCameraView.Default.ToString()];
				}
			}
		}
		

		#endregion


	}
}
