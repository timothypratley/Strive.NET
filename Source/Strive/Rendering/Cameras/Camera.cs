using System;
using Strive.Math3D;

using R3D089_VBasic;
using Strive.Rendering;

namespace Strive.Rendering.Cameras
{
	/// <summary>
	/// Represents the current view of the scene
	/// </summary>
	public class Camera : IManeuverable
	{
		#region "Private fields"
		private Vector3D _position;
		private Vector3D _rotation;
		private float _fieldOfView;
		private float _viewDistance;
		private string _key;

		#endregion

		#region "Constructors"
		/// <summary>
		/// Private to support the factory idiom
		/// </summary>
		private Camera()
		{
		}
		#endregion

		#region "Factory Loader"
		/// <summary>
		/// Loads a new camera and adds it the collection
		/// </summary>
		/// <param name="cameraKey">The specified Key for the camera</param>
		/// <param name="cameras">The collection to add the camera too</param>
		/// <returns>The newly created camera</returns>
		public static Camera CreateCamera(string cameraKey, CameraCollection cameras)
		{
			Camera c = new Camera();
			
			c._key = cameraKey;
			c.Position = Vector3D.Origin;
			//c.Rotation = Vector3D;
			cameras.Add(cameraKey, c);
			Interop._instance.Cameras.Camera_Create(cameraKey);
			Interop._instance.Cameras.Class_SetPointer(cameraKey);
			
			return c;
		}

		#endregion

		#region "Operators

		#endregion

		#region "Properties"
		/// <summary>
		/// Revolution specific camera attributes
		/// </summary>
		protected R3DCameraAttributes Attributes
		{
			get
			{
				R3DCameraAttributes _attributes = Interop._instance.Cameras.Camera_GetAttributes();
				return _attributes;
			}
		}

		protected void initialisePointer()
		{
			Interop._instance.Cameras.Class_SetPointer(_key);
		}


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
					initialisePointer();
					R3DCameraAttributes attributes = this.Attributes;
					Interop._instance.Cameras.Camera_SetAttributes(ref attributes);

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

					Interop._instance.Cameras.Class_SetPointer(_key);
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
			Interop._instance.Cameras.Camera_LookAt( ref p );
		}
		*/
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
				initialisePointer();
				R3DVector3D r = VectorConverter.GetR3DVector3DFromVector3D(newPosition);
				Interop._instance.Cameras.Camera_SetPosition(ref r);
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
				initialisePointer();
				R3DVector3D r = VectorConverter.GetR3DVector3DFromVector3D(newRotation);
				Interop._instance.Cameras.Camera_SetRotation(ref r);
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
				try {
					initialisePointer();
					R3DVector3D r = VectorConverter.GetR3DVector3DFromVector3D(value);
					Interop._instance.Cameras.Camera_SetPosition(ref r);
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
			set 
			{
				try 
				{
					initialisePointer();
					R3DVector3D r = VectorConverter.GetR3DVector3DFromVector3D(value);
					r.x = -r.x;
					r.y = -r.y;
					r.z = -r.z;
					Interop._instance.Cameras.Camera_SetRotation(ref r);
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
