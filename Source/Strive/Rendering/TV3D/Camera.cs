using System;
using Strive.Math3D;

using TrueVision3D;

namespace Strive.Rendering.TV3D
{
	/// <summary>
	/// Represents the current view of the scene
	/// </summary>
	public class Camera : ICamera
	{
		#region "Private fields"

		public TVCamera _tvcamera;
		// initialised to origin
		Vector3D _position = Vector3D.Origin;
		Vector3D _rotation = Vector3D.Origin;
		float _fieldOfView = 60;
		float _viewDistance = 100;
		float _nearPlane = 0.1F;

		#endregion

		#region "Constructors"
		public  Camera( TVCamera camera ) {
			_tvcamera = camera;
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
				//Engine.TV3DScene.SetViewFrustum( _fieldOfView, _viewDistance );
				_tvcamera.SetViewFrustum( _fieldOfView, _viewDistance, _nearPlane);
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
				_viewDistance = value;
				//Engine.TV3DScene.SetViewFrustum( _fieldOfView, _viewDistance );
				_tvcamera.SetViewFrustum(_fieldOfView, _viewDistance, _nearPlane);
			}
		}

		/*
		public void SetHeading( Vector3D heading ) {
			setPointer;
			R3DPoint3D p;
			p.x = heading.X + _position.X;
			p.y = heading.Y + _position.Y;
			p.z = heading.Z + _position.Z;
			_tvcameras.Camera_LookAt( ref p );
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
				_tvcamera.SetPosition(newPosition.X, newPosition.Y, newPosition.Z);
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
				_tvcamera.SetRotation( newRotation.X, newRotation.Y, newRotation.Z );
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
					_tvcamera.SetPosition(value.X, value.Y, value.Z);
				}
				catch(Exception e) 
				{
					throw new RenderingException("Could not set position '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for camera.", e);
				}
				_position = value.Clone();
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
					_tvcamera.SetRotation( value.X, value.Y, value.Z );
				}
				catch(Exception e) 
				{
					throw new RenderingException("Could not set rotation '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for camera.", e);
				}
				_rotation = value.Clone();
			}
		}
		#endregion
	}

}
