using System;
using System.Threading;
using System.Windows.Forms;

using Strive.Rendering.Models;
using Strive.Math3D;
using Revolution3D8087b;

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
		// This, combined with the inner class, is an attempt to implement
		// an interesting design pattern
		private Scene.Camera _view = Scene.Camera._instance;
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
				Interop._instance.Pipeline.SetBackColor(0,0,0);
				Interop._instance.Pipeline.SetDithering(true);
				Interop._instance.Pipeline.SetFillMode(Revolution3D8087b.R3DFILLMODE.R3DFILLMODE_SOLID);
				Interop._instance.Pipeline.SetAmbientLight(255,255,255);
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

		public void LoadTexture( string name, string filename ) {
			if ( Interop._instance.TextureLib.Class_SetPointer( name ) < 0 ) {
				R3DCOLORKEY colorkey = R3DCOLORKEY.R3DCOLORKEY_BLACK;
				Interop._instance.TextureLib.Texture_Load( name, filename, ref colorkey );
			} else {
				// already added
			}
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
				throw new RenderingException("Call to 'Render()' failed", e);
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
		) {
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
		public Camera View
		{
			get
			{
				checkInitialised();
				return _view;
			}
		}

		#endregion

		#region "Internal classes"

		/// <summary>
		/// Represents the current view of the scene
		/// </summary>
		public class Camera : IManeuverable
		{
			#region "Private fields"
			/// <summary>
			/// Enforces access to only one Camera object (singleton pattern)
			/// </summary>
			/// <remarks>This may be extended in the future for multiple cameras/split screen views such as 'crystal ball'</remarks>
			public static readonly Camera _instance = new Camera();
			private Vector3D _position;
			private Vector3D _rotation;
			private float _fieldOfView;
			private float _viewDistance;
			#endregion

			#region "Constructors"
			/// <summary>
			/// Private constructor for singleton pattern
			/// </summary>
			private Camera()
			{
			}
			#endregion

			#region "Operators

			#endregion

			#region "Properties"
			/// <summary>
			/// The depth of field (width of vision) for the camera
			/// </summary>
			public float FieldOfView
			{
				get
				{
					return _fieldOfView;
				}
				set
				{
					try
					{
						Interop._instance.Engine.Inf_SetFieldOfView(value);
					}
					catch(Exception e)
					{
						throw new SceneException("Could not set field of view", e);
					}
					_fieldOfView = value;
				
				}
			}
			/// <summary>
			/// The view distance for the camera
			/// </summary>
			public float ViewDistance
			{
				get
				{
					return _viewDistance;
				}
				set
				{
					try
					{
						Interop._instance.Engine.Inf_SetViewDistance(value);
					}
					catch(Exception e)
					{
						throw new SceneException("Could not set view distance", e);
					}
					_viewDistance = value;
				}
			}
			#endregion

			#region "Implementation of IManeuverable"
			/// <summary>
			/// Moves the camera
			/// </summary>
			/// <param name="movement">The amount to move the camera</param>
			/// <returns>Indicates whether the Move was successful</returns>
			public bool Move(Vector3D movement)
			{
				Vector3D newPosition = _position + movement;
				try
				{
					Interop._instance.Camera.SetPosition(newPosition.X, newPosition.Y, newPosition.Z);
				}
				catch(Exception e)
				{
					throw new RenderingException("Could not set position '" + newPosition.X + "' '" + newPosition.Y + "' '" + newPosition.Z + "' for camera.", e);
				}
				_position = newPosition;
				// TODO: Implement success correctly - may not be needed for a camera
				return true;
			}

			/// <summary>
			/// Rotates the camera
			/// </summary>
			/// <param name="rotation">The amount to rotate the camera</param>
			/// <returns>Indicates whether the rotation was successful</returns>
			public bool Rotate(Vector3D rotation)
			{
				Vector3D newRotation = _rotation + rotation;
				try
				{
					Interop._instance.Camera.SetRotation(newRotation.X, newRotation.Y, newRotation.Z);
				}
				catch(Exception e)
				{
					throw new RenderingException("Could not set rotation '" + newRotation.X + "' '" + newRotation.Y + "' '" + newRotation.Z + "' for camera.", e);
				}
				_rotation = newRotation;
				// TODO: Implement success if needed
				return true;
			}

			/// <summary>
			/// The position of the camera
			/// </summary>
			public Vector3D Position
			{
				get
				{
					return _position;
				}
				set {
					try {
						Interop._instance.Camera.SetPosition(value.X, value.Y, value.Z);
					}
					catch(Exception e) {
						throw new RenderingException("Could not set position '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for camera.", e);
					}
					_position = value;				
				}
			}

			/// <summary>
			/// The rotation of the camera
			/// </summary>
			public Vector3D Rotation
			{
				get
				{
					return _rotation;
				}
				set {
					try {
						Interop._instance.Camera.SetRotation(value.X, value.Y, value.Z);
					}
					catch(Exception e) {
						throw new RenderingException("Could not set rotation '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for camera.", e);
					}
					_rotation = value;
				}
			}
			#endregion
		}
		
		#endregion

	}
}
