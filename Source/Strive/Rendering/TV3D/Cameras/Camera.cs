using System;
using Strive.Math3D;

using TrueVision3D;
using Strive.Rendering.Cameras;

namespace Strive.Rendering.TV3D.Cameras
{
	/// <summary>
	/// Represents the current view of the scene
	/// </summary>
	public class Camera : ICamera
	{
		#region "Private fields"
		// initialised to origin
		private Vector3D _position = Vector3D.Origin;
		private Vector3D _rotation = Vector3D.Origin;
		private float _fieldOfView;
		private float _viewDistance;
		private string _key;

		static Camera thisCamera = new Camera();

		#endregion

		#region "Constructors"
		/// <summary>
		/// Private to support the factory idiom
		/// </summary>
		private Camera()
		{
		}
		#endregion

		#region "Factory initializer"
		public static Camera CreateCamera(string cameraKey) {
			// handle the default view
			// there is only one camera in TV3D
			Engine.Camera.SetCamera(0f, 0f, 0f, 0f, 0f, 0f);
			return thisCamera;
		}
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

					//Engine.Cameras.Class_SetPointer(_key);
				}
				catch(Exception e)
				{
					throw new SceneException("Could not set view distance", e);
				}
				_viewDistance = value;
			}
		}

		/*
		public void SetHeading( Vector3D heading ) {
			setPointer;
			R3DPoint3D p;
			p.x = heading.X + _position.X;
			p.y = heading.Y + _position.Y;
			p.z = heading.Z + _position.Z;
			Engine.Cameras.Camera_LookAt( ref p );
		}
		*/
		#endregion

		#region "Methods"
		public Vector2D ProjectPoint( Vector3D point ) {
			return new Vector2D( 0, 0 );
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
				Engine.Camera.SetPosition(newPosition.X, newPosition.Y, newPosition.Z);
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
				Engine.Camera.SetRotation( newRotation.X, newRotation.Y, newRotation.Z );
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
			set 
			{
				try 
				{
					Engine.Camera.SetPosition(value.X, value.Y, value.Z);
				}
				catch(Exception e) 
				{
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
			set 
			{
				try 
				{
					Engine.Camera.SetRotation( value.X, value.Y, value.Z );
				}
				catch(Exception e) 
				{
					throw new RenderingException("Could not set rotation '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for camera.", e);
				}
				_rotation = value;
			}
		}
		#endregion
	}

}
