using System;
using Strive.Math3D;

using R3D089_VBasic;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Rendering.R3D;
using Strive.Rendering.R3D.Textures;

namespace Strive.Rendering.R3D.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public class Actor : IActor {

		#region "Fields"
		float _RadiusSquared;

		private string _key;
		private int _id;
		private Vector3D _position;
		private Vector3D _rotation;
		#endregion

		#region "Constructors"

		#endregion

		#region "Factory Initialisers"
		public static IActor Load( string name, string path, float height ) {		
			if(!System.IO.File.Exists(path)) {
				throw new System.IO.FileNotFoundException("Could not load model '" + path + "'", path);
			}
			Actor loadedModel = new Actor();
			loadedModel._key = name;
			// todo: fix bounding radius
			loadedModel._RadiusSquared = 100;

			try {
				loadedModel._id = Engine.MD2System.Model_Load(path, name);
			}
			catch(Exception e) {
				throw new ModelNotLoadedException(path, e);
			}
			loadedModel.Position = Vector3D.Origin;
			return loadedModel;
		}
		#endregion

		#region "Methods"

		public void Delete() {
			Engine.MD2System.Class_SetPointer(this.Name);
			Engine.MD2System.Model_Release();
		}

		public void Hide() {
			Engine.MD2System.Class_SetPointer(this.Name);
			Engine.MD2System.Model_SetActivate( false );
		}

		public void Show() {
			Engine.MD2System.Class_SetPointer(this.Name);
			Engine.MD2System.Model_SetActivate( true );
		}

		public void applyTexture( ITexture texture ) {
			Engine.MD2System.Class_SetPointer(this.Name);
			Engine.MD2System.Model_SetTexture( 0, texture.Name );
		}

		// todo: remove me!
		public void Render() {

		}

		public void playAnimation() {
			Engine.MD2System.Class_SetPointer(this.Name);
			bool relative = true;
			Engine.MD2System.Model_Animate( R3DLERPANIMTYPE.R3DLERPANIMTYPE_NODECROSSOVER, R3DCYCLEMODE.R3DCYCLE_LOOP, 0.1f, ref relative );
		}

		public void stopAnimation() {

		}

		public void GetBoundingBox( Vector3D minbox, Vector3D maxbox ) {

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
				Engine.MD2System.Class_SetPointer(this.Name);
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
				// todo: fix meh!
				//Engine.MD2System.Model_Animate();
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
		public bool Move(Vector3D movement) {
			// Calculate new absolute vector:
			Vector3D newPosition = _position + movement;

			Engine.MD2System.Class_SetPointer(this.Name);
			try {
				R3DVector3D vector = VectorConverter.GetR3DVector3DFromVector3D(newPosition);
				Engine.MD2System.Model_SetPosition(ref vector);
			}
			catch(Exception e) {
				throw new ModelException("Could not set position '" + newPosition.X + "' '" + newPosition.Y + "' '" + newPosition.Z + "' for model '" + this.Name + "'", e);
			}
			_position = newPosition;
			// TODO: Implement success 
			return true;
		}


		/// <summary>
		/// Rotates the model
		/// </summary>
		/// <param name="rotation">The amount to rotate</param>
		/// <returns>Indicates whether the rotation was successful</returns>
		public bool Rotate(Vector3D rotation) {
			// Calculate absolute rotation
			Vector3D newRotation = _rotation + rotation;

			Engine.MD2System.Class_SetPointer(this.Name);
			try {
				R3DVector3D vector = VectorConverter.GetR3DVector3DFromVector3D(newRotation);
				Engine.MD2System.Model_SetRotation(ref vector);
			}
			catch(Exception e) {
				throw new ModelException("Could not set rotation '" + newRotation.X + "' '" + newRotation.Y + "' '" + newRotation.Z + "' for model '" + this.Name + "'", e);
			}
			_rotation = newRotation;
			// TODO: Implement success
			return true;
		}

		/// <summary>
		/// The position of the model 
		/// </summary>
		public Vector3D Position {
			get {
				return _position;
			}
			set {
				R3DVector3D vector = VectorConverter.GetR3DVector3DFromVector3D(value);
				Engine.MD2System.Class_SetPointer(this.Name);
				try {

					Engine.MD2System.Model_SetPosition(ref vector);
				}
				catch(Exception e) {
					throw new ModelException("Could not set position '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for model '" + this.Name + "'", e);
				}
				_position = value;
			}
		}

		/// <summary>
		/// The rotation of the model
		/// </summary>
		public Vector3D Rotation {
			get {
				return _rotation;
			}
			set {
				R3DVector3D vector = VectorConverter.GetR3DVector3DFromVector3D(value);
				Engine.MD2System.Class_SetPointer(this.Name);
				try {
					// todo: normalise MD2s so we don't need a 90 degree offset?
					// todo: our mobs are at different angles to players :(
					Engine.MD2System.Model_SetRotation(ref vector);
				}
				catch(Exception e) {
					throw new ModelException("Could not set rotation '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for model '" + this.Name + "'", e);
				}
				_rotation = value;
			}
		}
		#endregion
	}

}
