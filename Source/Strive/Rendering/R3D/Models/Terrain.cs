using System;
using Strive.Math3D;

using R3D089_VBasic;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Rendering.R3D;

namespace Strive.Rendering.R3D.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public class Terrain : ITerrain {

		#region "Fields"
		public float _BoundingSphereRadiusSquared;

		private string _key;
		private int _id;
		private Vector3D _position;
		private Vector3D _rotation;
		#endregion

		#region "Constructors"

		#endregion

		#region "Factory Initialisers"
		public static Terrain CreateTerrain( string name, ITexture texture ) {
			Terrain created = new Terrain();
			created._id = Engine.MeshBuilder.Mesh_Create( name );
			created._key = name;
			R3DVector3D p1, p2, p3, p4;
			p1.x = 0;
			p1.y = 0;
			p1.z = 0;
			p2.x = 0;
			p2.y = 0;
			p2.z = 100;
			p3.x = 100;
			p3.y = 0;
			p3.z = 100;
			p4.x = 100;
			p4.y = 0;
			p4.z = 0;
			Engine.MeshBuilder.Mesh_AddPlane( ref p1, ref p2, ref p3, ref p4, texture.Name, "", R3DBLENDMODE.R3DBLENDMODE_NONE, true);
			// todo: fix this hax,
			// atm set to 0 as this is assumed to be terrain...
			created._BoundingSphereRadiusSquared = 0;
			return created;
		}


		#endregion


		#region "Methods"

		public void Delete() {
				Engine.MeshBuilder.Class_SetPointer(this.Name);
				Engine.MeshBuilder.Mesh_Release();
		}

		public void Hide() {
				Engine.MeshBuilder.Class_SetPointer(this.Name);
				Engine.MeshBuilder.Mesh_SetActivate( false );
		}

		public void Show() {
				Engine.MeshBuilder.Class_SetPointer(this.Name);
				Engine.MeshBuilder.Mesh_SetActivate( true );
		}

		public void Normalise( float height ) {
		}

		public void applyTexture( ITexture texture ) {
			Engine.MeshBuilder.Class_SetPointer(this.Name);
			Engine.MeshBuilder.Mesh_SetTexture( 0, texture.Name );
		}

		public float HeightLookup( float x, float z ) {
			// todo: implement
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

			Engine.MeshBuilder.Class_SetPointer(this.Name);

					try
					{
						R3DVector3D vector = VectorConverter.GetR3DVector3DFromVector3D(newPosition);
						Engine.MeshBuilder.Mesh_SetPosition(ref vector);
					}
					catch(Exception e)
					{
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
		public bool Rotate(Vector3D rotation)
		{
			// Calculate absolute rotation
			Vector3D newRotation = _rotation + rotation;

			Engine.MeshBuilder.Class_SetPointer(this.Name);

					try
					{
						R3DVector3D vector = VectorConverter.GetR3DVector3DFromVector3D(newRotation);
						Engine.MeshBuilder.Mesh_SetRotation(ref vector);
					}
					catch(Exception e)
					{
						throw new ModelException("Could not set rotation '" + newRotation.X + "' '" + newRotation.Y + "' '" + newRotation.Z + "' for model '" + this.Name + "'", e);
					}
			_rotation = newRotation;
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
				R3DVector3D vector = VectorConverter.GetR3DVector3DFromVector3D(value);
				Engine.MeshBuilder.Class_SetPointer(this.Name);
					try {
		
							Engine.MeshBuilder.Mesh_SetPosition(ref vector);
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
		public Vector3D Rotation
		{
			get
			{
				return _rotation;
			}
			set {
				R3DVector3D vector = VectorConverter.GetR3DVector3DFromVector3D(value);
				Engine.MeshBuilder.Class_SetPointer(this.Name);

						Engine.MeshBuilder.Mesh_SetRotation(ref vector );
				_rotation = value;
			}
		}
		#endregion
	}

}
