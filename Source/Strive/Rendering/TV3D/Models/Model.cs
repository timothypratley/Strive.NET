using System;
using DxVBLibA;

using TrueVision3D;
using Strive.Rendering.Models;
using Strive.Rendering.TV3D;
using Strive.Rendering.Textures;
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
		public string _label;
		Vector3D _position;
		Vector3D _offset;
		Vector3D _rotation;
		TVMesh _mesh;
		float _RadiusSquared;
		Vector3D _boxmin;
		Vector3D _boxmax;
		bool _show = true;
		float _height;
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
		public static IModel LoadStaticModel( string name, string path, float height ) {
			if(!System.IO.File.Exists(path)) {
				throw new System.IO.FileNotFoundException("Could not load model '" + path + "'", path);
			}

			Model loadedModel = new Model();
			loadedModel._key = name;
			try {
				loadedModel._mesh = Engine.TV3DScene.CreateMeshBuilder( name );
				loadedModel._mesh.Load3DSMesh( path, true, true, false, true, true );
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

			loadedModel._height = height;
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
			//loadedModel._mesh.InitLOD(0);
			return loadedModel;
		}


		public IModel Duplicate( string name, float height ) {
			Model dup = new Model();
			dup._key = name;
			dup._mesh = this._mesh.DuplicateMesh( name );
			dup._mesh.SetPosition( 0, 0, 0 );
			dup._mesh.SetRotation( 0, 0, 0 );

			//loadedModel._mesh.ShowBoundingBox( true );
			D3DVECTOR boxmin = new D3DVECTOR();
			D3DVECTOR boxmax = new D3DVECTOR();
			dup._mesh.GetBoundingBox( ref boxmin, ref boxmax, true );
			dup._boxmin = new Vector3D( boxmin.x, boxmin.y, boxmin.z );
			dup._boxmax = new Vector3D( boxmax.x, boxmax.y, boxmax.z );

			dup._height = height;
			if ( height != 0 ) {
				// scale the model to the correct height and get new bounding box info
				float scale_factor = height / ( boxmax.y - boxmin.y );
				dup._mesh.ScaleMesh( scale_factor, scale_factor, scale_factor );
				dup._boxmin.X *= scale_factor;
				dup._boxmin.Y *= scale_factor;
				dup._boxmin.Z *= scale_factor;
				dup._boxmax.X *= scale_factor;
				dup._boxmax.Y *= scale_factor;
				dup._boxmax.Z *= scale_factor;
			}
			float radius = 0;
			DxVBLibA.D3DVECTOR center = new DxVBLibA.D3DVECTOR();
			dup._mesh.GetBoundingSphere(ref center, ref radius, true );
			dup._RadiusSquared = radius * radius;
			dup._position = new Math3D.Vector3D( 0, 0, 0 );
			dup._rotation = new Math3D.Vector3D( 0, 0, 0 );
			// TODO: could get rid of 'offset' if we prenormalize the models
			// outside
			dup._offset = new Math3D.Vector3D(
				(boxmax.x + boxmin.x)/2,
				(boxmax.y + boxmin.y)/2,
				(boxmax.z + boxmin.z)/2
				);
			dup._id = dup._mesh.GetMeshIndex();
			return dup;
		}


		public static Model CreateBox( string name, float width, float height, float depth, ITexture texture ) {
			Model loadedModel = new Model();
			loadedModel._key = name;
			loadedModel._mesh = Engine.TV3DScene.CreateMeshBuilder( name );
			loadedModel._mesh.CreateBox( width, height, depth, false );
			if ( texture != null ) loadedModel._mesh.SetTexture( texture.ID, 0 );
			loadedModel._RadiusSquared = 0;
			loadedModel._position = new Math3D.Vector3D( 0, 0, 0 );
			loadedModel._rotation = new Math3D.Vector3D( 0, 0, 0 );
			// TODO: could get rid of 'offset' if we prenormalize the models
			// outside
			loadedModel._offset = new Math3D.Vector3D( 0,0,0 );
			loadedModel._id = loadedModel._mesh.GetMeshIndex();
			return loadedModel;
		}


		public static Model CreatePlane( string name, ITexture texture, Vector3D p1, Vector3D p2, Vector3D p3, Vector3D p4 ) {
			Model t = new Model();
			t._mesh = Engine.TV3DScene.CreateMeshBuilder( name );
			// NB: note the point order gets changed here,
			// because I think it makes more sense this way
			t._mesh.AddFaceFromPoint( texture.ID, p1.X, p1.Y, p1.Z, p2.X, p2.Y, p2.Z, p4.X, p4.Y, p4.Z, p3.X, p3.Y, p3.Z, 1, 1, true, false );
			t._mesh.Optimize();
			t._mesh.ComputeBoundingVolumes();
			t._offset = new Math3D.Vector3D( 0,0,0 );

			// TODO: bumpmapping would be cool :)
			//t._mesh.SetBumpMapping( true, texture.ID, -1, -1, 10 );
			t._key = name;
			t._id = t._mesh.GetMeshIndex();
			t._RadiusSquared = 0;
			return t;
		}
		#endregion

		#region "Methods"

		public void Delete() {
			Engine.TV3DScene.DestroyMesh( ref _mesh );
			_mesh = null;
		}

		public void applyTexture( ITexture texture ) {
		}

		public void nextFrame() {
		}

		public void SetLOD( EnumLOD lod ) {
			//_mesh.SetLODIndex( (float)lod );
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

		public string Label {
			get { return _label; }
			set { _label = value; }
		}

		public bool Visible {
			get { return _show; }
			set { _show = value; _mesh.Enable( value ); }
		}

		public float Height {
			get { return _height; }
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
