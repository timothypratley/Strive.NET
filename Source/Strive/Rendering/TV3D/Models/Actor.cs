using System;
using DxVBLibA;
using TrueVision3D;

using Strive.Math3D;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Rendering.TV3D;
using Strive.Logging;

namespace Strive.Rendering.TV3D.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public class Actor : IActor {

		#region "Fields"


		float _RadiusSquared;

		// todo: shouldn't need a show variable, engine should do it for us
		private bool _show = true;
		private string _key;
		private string _label;
		private int _id;
		private Vector3D _offset;
		private Vector3D _position;
		private Vector3D _rotation;
		TVActor2 _model;

		private Vector3D _boxmin;
		private Vector3D _boxmax;
		private float _height;
		#endregion

		#region "Constructors"

		#endregion

		#region "Factory Initialisers"

		/// <summary>
		/// Loads a model
		/// </summary>
		/// <param name="key">The key of the model</param>
		/// <param name="path">The path at which the model can be loaded</param>
		/// <param name="format">The format of the model</param>
		/// <returns>A reference to the loaded Model</returns>
		public static IActor LoadActor(string name, string path, float height ) {
			if(!System.IO.File.Exists(path)) {
				throw new System.IO.FileNotFoundException("Could not load model '" + path + "'", path);
			}
			
			Actor loadedModel = new Actor();

			loadedModel._key = name;

			try {
				//loadedModel._model = Engine.TV3DScene.CreateActorTVM( name );
				loadedModel._model = new TVActor2();
				loadedModel._model.Load( path, name, true, true );
			}
			catch(Exception e) {
				throw new ModelNotLoadedException(path, e);
			}
			DxVBLibA.D3DVECTOR boxmin = new DxVBLibA.D3DVECTOR();
			DxVBLibA.D3DVECTOR boxmax = new DxVBLibA.D3DVECTOR();
			loadedModel._model.GetBoundingBox( ref boxmin, ref boxmax );

			// TODO: get bounding boxes dependent upon the animation sequence

			// EEERRR todo: there is a bug in tv3d mdl bounding boxes,
			// we need to transpose for it
			loadedModel._boxmin = new Vector3D( boxmin.y, boxmin.z, boxmin.x );
			loadedModel._boxmax = new Vector3D( boxmax.y, boxmax.z, boxmax.x );

			// todo: use a bounding cylinder instead, would be bett0r!

			loadedModel._height = height;
			if ( height != 0 ) {
				// scale the model to the correct height and get new bounding box info
				float scale_factor = height / ( loadedModel._boxmax.Y - loadedModel._boxmin.Y );
				loadedModel._model.SetScale( scale_factor, scale_factor, scale_factor );
				loadedModel._boxmin.X *= scale_factor;
				loadedModel._boxmin.Y *= scale_factor;
				loadedModel._boxmin.Z *= scale_factor;
				loadedModel._boxmax.X *= scale_factor;
				loadedModel._boxmax.Y *= scale_factor;
				loadedModel._boxmax.Z *= scale_factor;
			}
			float radius = 0;
			DxVBLibA.D3DVECTOR center = new DxVBLibA.D3DVECTOR();
			loadedModel._model.GetBoundingSphere(ref center, ref radius );
			loadedModel._RadiusSquared = radius * radius;
			loadedModel._position = new Math3D.Vector3D( 0, 0, 0 );
			// TODO: remove hardcoded initial rotation
			//loadedModel._rotation = new Math3D.Vector3D( 0, 90, 0 );

			// todo: _offset is ghey I wish we could permenantly transpose
			// the object.
			// also, bounding box should be normalised around origin?
			loadedModel._offset = new Math3D.Vector3D(
				(loadedModel._boxmax.X + loadedModel._boxmin.X)/2,
				(loadedModel._boxmax.Y + loadedModel._boxmin.Y)/2,
				(loadedModel._boxmax.Z + loadedModel._boxmin.Z)/2
			);
			loadedModel._id = loadedModel._model.GetEntity();
			//loadedModel._model.ShowBoundingBox( true, false );
			loadedModel._model.PlayAnimation( 20 );
			return loadedModel;
		}


		#endregion

		#region "Methods"

		public void Delete() {
			_model.Destroy();
		}

		public void applyTexture( ITexture texture ) {
			_model.SetTexture( texture.ID, 0 );
		}

		// todo: remove me
		public void Render() {
			//if ( _model.IsVisible() ) {
			if ( _show ) {
				_model.Render();
			}
		}

		public void playAnimation() {
			// todo: use a better value than 1
			_model.PlayAnimation( 20 );
		}

		public void stopAnimation() {
			_model.StopAnimation();
		}

		public void GetBoundingBox( Vector3D minbox, Vector3D maxbox ) {
			minbox.Set( _boxmin );
			maxbox.Set( _boxmax );
		}

		public IModel Duplicate( string name, float height ) 
		{
			Actor dup = new Actor();
			dup._model = this._model.Duplicate( name );
			dup._model.SetPosition( 0, 0, 0 );
			dup._model.SetRotation( 0, 0, 0 );
			dup._key = name;

			
			DxVBLibA.D3DVECTOR boxmin = new DxVBLibA.D3DVECTOR();
			DxVBLibA.D3DVECTOR boxmax = new DxVBLibA.D3DVECTOR();
			dup._model.GetBoundingBox( ref boxmin, ref boxmax );
			
			// TODO: get bounding boxes dependent upon the animation sequence

			// EEERRR todo: there is a bug in tv3d mdl bounding boxes,
			// we need to transpose for it
			dup._boxmin = new Vector3D( boxmin.y, boxmin.z, boxmin.x );
			dup._boxmax = new Vector3D( boxmax.y, boxmax.z, boxmax.x );

			// todo: use a bounding cylinder instead, would be bett0r!

			dup._height = height;
			if ( height != 0 ) {
				// scale the model to the correct height and get new bounding box info
				float scale_factor = height / ( dup._boxmax.Y - dup._boxmin.Y );
				dup._model.SetScale( scale_factor, scale_factor, scale_factor );
				dup._boxmin.X *= scale_factor;
				dup._boxmin.Y *= scale_factor;
				dup._boxmin.Z *= scale_factor;
				dup._boxmax.X *= scale_factor;
				dup._boxmax.Y *= scale_factor;
				dup._boxmax.Z *= scale_factor;
			}
			float radius = 0;
			DxVBLibA.D3DVECTOR center = new DxVBLibA.D3DVECTOR();
			dup._model.GetBoundingSphere(ref center, ref radius );
			dup._RadiusSquared = radius * radius;
			dup._position = new Math3D.Vector3D( 0, 0, 0 );
			// TODO: remove hardcoded initial rotation
			//dup._rotation = new Math3D.Vector3D( 0, 90, 0 );

			// todo: _offset is ghey I wish we could permenantly transpose
			// the object.
			// also, bounding box should be normalised around origin?
			dup._offset = new Math3D.Vector3D(
				(dup._boxmax.X + dup._boxmin.X)/2,
				(dup._boxmax.Y + dup._boxmin.Y)/2,
				(dup._boxmax.Z + dup._boxmin.Z)/2
				);


			dup._id = dup._model.GetEntity();
			return dup;
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


		public string AnimationSequence {
			set {
				_model.SetAnimationByName( value );
			}
		}

		public string Label {
			get { return _label; }
			set { _label = value; }
		}

		public bool Visible {
			get { return _show; }
			set { _show = value; _model.Enable(value); }
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
			_model.SetPosition( _position.X - _offset.X, _position.Y - _offset.Y, _position.Z - _offset.Z );
			// TODO: Implement success 
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
			_model.SetRotation( _rotation.X, _rotation.Y, _rotation.Z );
			// TODO: Implement success
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
				_model.SetPosition( _position.X - _offset.X, _position.Y - _offset.Y, _position.Z - _offset.Z );
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

				//TODO: remove this hack to rotate the character,
				// instead normalise the model.
				_model.SetRotation( _rotation.Z, _rotation.Y+90, _rotation.X );
			}
		}
		#endregion
	}

}
