using System;
using System.Threading;
using System.Windows.Forms;

using Strive.Rendering;
using Strive.Rendering.Cameras;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Rendering.R3D.Models;
using Strive.Math3D;
using R3D089_VBasic;

namespace Strive.Rendering.R3D
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
			if ( Engine.Skydome.Class_SetPointer( name ) < 0 ) {
				Engine.Skydome.Item_Create( name, R3DSKYDOMEITEM.R3DSKYDOMEITEM_SPHERE, 2000 );
			}
			Engine.Skydome.Item_SetTexture( 0, texture.Name );
		}

		public void SetLighting( short level ) {
			R3DColor color;
			color.r = level;
			color.g = level;
			color.b = level;
			color.a = level;
			//Engine.Pipeline.SetAmbientLight( level, level, level );
			Engine.Pipeline.SetGammaLevel( ref color );
		}

		public void SetFog( float level ) {
			R3DColor FogColor;
			FogColor.r = (short)(level);
			FogColor.g = (short)(level);
			FogColor.b = (short)(level);
			FogColor.a = (short)(level);
			Engine.Pipeline.SetFog( ref FogColor, R3DFOGTYPE.R3DFOGTYPE_PIXELTABLE, R3DFOGMODE.R3DFOGMODE_LINEAR, 0, 200, level );
		}

		public void DrawText( Vector2D location, string message ) {
			R3DVector2D nameLoc = new R3DVector2D();
			nameLoc.x = location.X;
			nameLoc.y = location.Y;
			Engine.Interface5D.Primitive_DrawText( ref nameLoc, message );
		}

		/// <summary>
		/// Public rendering routine
		/// </summary>
		/// <remarks>This method renders the scene into video memory</remarks>
		public void Render()
		{
			try
			{
				Engine.Pipeline.Renderer_Clear();
			}
			catch(Exception e)
			{
				throw new RenderingException("Call to 'Clear()' failed", e);
			}
			try {
				Engine.Pipeline.Renderer_Render();
			}
			catch(Exception e) {
				throw new RenderingException("Call to 'Render()' failed with '" + e.ToString() + "'", e);
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
			//Engine.Interface5D.Primitive_SetDrawColor(ref black);
	        Engine.Interface5D.Primitive_DrawText(ref zero, "Fp/S: " + Engine.PowerMonitor.lGetFramesPerSecond().ToString() +
                                                            ", Vertices: " + Engine.PowerMonitor.lGetNumVerticesPerSinceLastFrame().ToString() + 
                                                            ", Verts/Sec:  " + (Engine.PowerMonitor.lGetFramesPerSecond() * Engine.PowerMonitor.lGetNumVerticesPerSinceLastFrame()).ToString() );
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
				Engine.Pipeline.Renderer_Display();
			}
			catch(Exception e)
			{
				throw new RenderingException("Call to 'Display()' failed", e);
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
//			Engine.Meshbuilder.Class_RayCollision(`(
//				ref vStart, ref vEnd, ct
//				);

			return 1;
		}

		public IModel MousePick( int x, int y ) {

			return null;
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
