using System;
using Strive.Math3D;

using TrueVision3D;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Rendering.TV3D;

namespace Strive.Rendering.TV3D.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public class Actor : IActor {

		#region "Fields"
		public float BoundingSphereRadiusSquared;

		// todo: shouldn't need a show variable, engine should do it for us
		private bool _show = true;
		private string _key;
		private int _id;
		private Vector3D _position;
		private Vector3D _rotation;
		TVActor2 _model;
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
		public static IActor LoadActor(string name, string path ) {
			if(!System.IO.File.Exists(path)) {
				throw new System.IO.FileNotFoundException("Could not load model '" + path + "'", path);
			}
			
			Actor loadedModel = new Actor();

			loadedModel._key = name;
			loadedModel.BoundingSphereRadiusSquared = 100;

			try {
				loadedModel._model = Engine.TV3DScene.CreateActorTVM( name );
				loadedModel._model.Load( path, name, true, true );
				loadedModel.BoundingSphereRadiusSquared = 1000;
			}
			catch(Exception e) {
				throw new ModelNotLoadedException(path, e);
			}
			loadedModel.Position = Vector3D.Origin;
			loadedModel._id = loadedModel._model.GetEntity();
			return loadedModel;
		}


		#endregion

		#region "Methods"

		public void Delete() {
			_model.Destroy();
		}

		public void Hide() {
			_show = false;
			// todo: why doesn't this work?
			//_model.Enable( false );
		}

		public void Show() {
			_show = true;
			//_model.Enable( true );
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
			_model.PlayAnimation( 1 );
		}

		public void stopAnimation() {
			_model.StopAnimation();
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


		public int AnimationSequence {
			set {
				string sequence;		
				switch( value ) {
					case 1:
						sequence = "stand";
						break;
					case 2:
						sequence = "run";
						break;
					case 3:
						sequence = "stand";
						break;
					case 4:
						sequence = "run";
						break;
					case 5:
						sequence = "stand";
						break;
					case 6:
						sequence = "run";
						break;
					case 7:
						sequence = "stand";
						break;
					case 8:
						sequence = "run";
						break;
					default:
						throw new Exception( "Unknown sequence" );
				}
				// nb: could set by number also
				_model.SetAnimationByName( sequence );
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
			_model.SetPosition( _position.X, _position.Y, _position.Z );
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
				_model.SetPosition( _position.X, _position.Y, _position.Z );
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
				_model.SetRotation( _rotation.X, _rotation.Y, _rotation.Z );
			}
		}
		#endregion
	}

}
