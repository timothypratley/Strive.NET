using System;
using Strive.Math3D;

using TrueVision3D;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Rendering.TV3D;
using Strive.Rendering.TV3D.Textures;

namespace Strive.Rendering.TV3D.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public class Terrain : ITerrain {

		#region "Fields"
		public float BoundingSphereRadiusSquared;

		private string _key;
		private int _id;
		private Vector3D _position;
		private Vector3D _rotation;
		TVMesh _mesh;
		#endregion

		#region "Constructors"

		#endregion

		#region "Factory Initialisers"
		public static ITerrain CreateTerrain( string name, ITexture texture, float y, float xy, float zy, float xzy ) {
			Terrain t = new Terrain();
			t._mesh = Engine.TV3DScene.CreateMeshBuilder( name );
			t._mesh.AddTriangle( texture.ID, 0, zy, 100, 100, xzy, 100, 0, y, 0, 1, 1, true, false );
			t._mesh.AddTriangle( texture.ID, 0, y, 0, 100, xy, 0, 100, xzy, 100, 1, 1, true, false );
			return t;
		}


		#endregion

		#region "Methods"

		public void Delete() {
		}

		public void Hide() {
		}

		public void Show() {
		}

		public void applyTexture( ITexture texture ) {
		}

		public float HeightLookup( float x, float z ) {
			return 0;
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
			_mesh.SetPosition( newPosition.X, newPosition.Y, newPosition.Z );
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
			_mesh.SetRotation( newRotation.X, newRotation.Y, newRotation.Z );
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
				_mesh.SetPosition( value.X, value.Y, value.Z );
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
				_mesh.SetRotation( value.X, value.Y, value.Z );
			}
		}
		#endregion
	}

}
