using System;

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
		Vector3D _position = new Vector3D( 0, 0, 0 );
		Vector3D _rotation = new Vector3D( 0, 0, 0 );
		TVMesh _mesh;
		float _BoundingSphereRadiusSquared;
		#endregion

		#region "Constructors"

		#endregion

		#region "Factory Initialisers"

		public static Model LoadStaticModel( string name, string path ) {
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
			float radius = 0;
			DxVBLibA.D3DVECTOR center = new DxVBLibA.D3DVECTOR();
			loadedModel._mesh.GetBoundingSphere( ref center, ref radius, false );
			loadedModel._BoundingSphereRadiusSquared = radius * radius;
			if ( center.x != 0 || center.y != 0 || center.z != 0 ) {
				Log.WarningMessage( "Model " + name + " " + path + " is centered at " + center + " not at (0,0,0) where it should be." );
			}
			loadedModel.Position = Vector3D.Origin;
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

		public void Normalise( float height ) {
			// TODO: should also translate, not just scale
			DxVBLibA.D3DVECTOR boxmin = new DxVBLibA.D3DVECTOR();
			DxVBLibA.D3DVECTOR boxmax = new DxVBLibA.D3DVECTOR();
			_mesh.GetBoundingBox( ref boxmin, ref boxmax, false );
			float scale_factor = height / ( boxmax.y - boxmin.y );
			_mesh.ScaleMesh( scale_factor, scale_factor, scale_factor );
		}

		public void applyTexture( string texture ) {
		}

		public void nextFrame() {
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

		public float BoundingSphereRadiusSquared {
			get {
				return _BoundingSphereRadiusSquared;
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
			_mesh.SetPosition( _position.X, _position.Y, _position.Z );
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
				_mesh.SetPosition( _position.X, _position.Y, _position.Z );
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
