using System;
using DxVBLibA;

using TrueVision3D;
using Strive.Rendering.Models;
using Strive.Rendering.TV3D;
using Strive.Math3D;
using Strive.Logging;

namespace Strive.Rendering.TV3D.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public class Model : IModel {

		#region "Fields"

		string _key;
		int _id;
		Vector3D _position;
		Vector3D _offset;
		Vector3D _rotation;
		TVMesh _mesh;
		float _RadiusSquared;
		Vector3D _boxmin;
		Vector3D _boxmax;
		#endregion

		#region "Constructors"

		#endregion

		#region "Factory Initialisers"

		/// <summary>
		/// Load a model from file
		/// </summary>
		/// <param name="name"></param>
		/// <param name="path"></param>
		/// <param name="height">pass zero to retain original height</param>
		/// <returns></returns>
		public static Model LoadStaticModel( string name, string path, float height ) {
			if(!System.IO.File.Exists(path)) {
				throw new System.IO.FileNotFoundException("Could not load model '" + path + "'", path);
			}

			Model loadedModel = new Model();
			loadedModel._key = name;
			try {
				loadedModel._mesh = Engine.TV3DScene.CreateMeshBuilder( name );
				loadedModel._mesh.Load3DSMesh( path, true, true, true );
			}
			catch(Exception e) {
				throw new ModelNotLoadedException(path, e);
			}
			//loadedModel._mesh.ShowBoundingBox( true );
			D3DVECTOR boxmin = new D3DVECTOR();
			D3DVECTOR boxmax = new D3DVECTOR();
			loadedModel._mesh.GetBoundingBox( ref boxmin, ref boxmax, true );
			loadedModel._boxmin = new Vector3D( boxmin.x, boxmin.y, boxmin.z );
			loadedModel._boxmax = new Vector3D( boxmax.x, boxmax.y, boxmax.z );

			if ( height != 0 ) {
				// scale the model to the correct height and get new bounding box info
				float scale_factor = height / ( boxmax.y - boxmin.y );
				loadedModel._mesh.ScaleMesh( scale_factor, scale_factor, scale_factor );
				loadedModel._boxmin.X *= scale_factor;
				loadedModel._boxmin.Y *= scale_factor;
				loadedModel._boxmin.Z *= scale_factor;
				loadedModel._boxmax.X *= scale_factor;
				loadedModel._boxmax.Y *= scale_factor;
				loadedModel._boxmax.Z *= scale_factor;
			}
			float radius = 0;
			DxVBLibA.D3DVECTOR center = new DxVBLibA.D3DVECTOR();
			loadedModel._mesh.GetBoundingSphere(ref center, ref radius, true );
			loadedModel._RadiusSquared = radius * radius;
			loadedModel._position = new Math3D.Vector3D( 0, 0, 0 );
			loadedModel._rotation = new Math3D.Vector3D( 0, 0, 0 );
			// TODO: could get rid of 'offset' if we prenormalize the models
			// outside
			loadedModel._offset = new Math3D.Vector3D(
				(boxmax.x + boxmin.x)/2,
				(boxmax.y + boxmin.y)/2,
				(boxmax.z + boxmin.z)/2
			);
			loadedModel._id = loadedModel._mesh.GetMeshIndex();
			return loadedModel;
		}
		#endregion

		#region "Methods"

		public void Delete() {
			Engine.TV3DScene.DestroyMesh( ref _mesh );
			_mesh = null;
		}

		public void Hide() {
		}

		public void Show() {
		}

		public void applyTexture( string texture ) {
		}

		public void nextFrame() {
		}

		public void GetBoundingBox( Vector3D minbox, Vector3D maxbox ) {
			minbox.Set( _boxmin );
			maxbox.Set( _boxmax );
		}

		#endregion

		#region "Properties"

		/// <summary>
		/// The key of the underlying model
		/// </summary>
		public string Name {
			get {
				return _key;
			}
		}

		public int ID {
			get {
				return _id;
			}
		}

		public float RadiusSquared {
			get {
				return _RadiusSquared;
			}
		}
		#endregion

		#region "Implementation of IManeuverable"
		/// <summary>
		/// Moves the model
		/// </summary>
		/// <param name="movement">The amount to move (relative)</param>
		/// <returns>Indicates whether the move was successful</returns>
		public bool Move(Vector3D movement)
		{
			// Calculate new absolute vector:
			Vector3D newPosition = _position + movement;
			_position = newPosition;
			_mesh.SetPosition( _position.X - _offset.X, _position.Y - _offset.Y, _position.Z - _offset.Z );
			return true;
		}


		/// <summary>
		/// Rotates the model
		/// </summary>
		/// <param name="rotation">The amount to rotate</param>
		/// <returns>Indicates whether the rotation was successful</returns>
		public bool Rotate(Vector3D rotation)
		{
			// Calculate absolute rotation
			Vector3D newRotation = _rotation + rotation;
			_rotation = newRotation;
			_mesh.SetRotation( _rotation.X, _rotation.Y, _rotation.Z );
			return true;
		}

		/// <summary>
		/// The position of the model 
		/// </summary>
		public Vector3D Position
		{
			get
			{
				return _position;
			}
			set {
				_position = value;
				_mesh.SetPosition( _position.X - _offset.X, _position.Y - _offset.Y, _position.Z - _offset.Z );
			}
		}

		/// <summary>
		/// The rotation of the model
		/// </summary>
		public Vector3D Rotation
		{
			get
			{
				return _rotation;
			}
			set {
				_rotation = value;
				_mesh.SetRotation( _rotation.X, _rotation.Y, _rotation.Z );
			}
		}
		#endregion
	}

}
