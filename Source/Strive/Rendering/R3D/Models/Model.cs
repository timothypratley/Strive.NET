using System;
using Strive.Math3D;

using R3D089_VBasic;
using Strive.Rendering.Models;
using Strive.Rendering.R3D;

namespace Strive.Rendering.R3D.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public class Model : IModel {

		#region "Fields"
		public float BoundingSphereRadiusSquared;

		private string _key;
		private int _id;
		private Vector3D _position;
		private Vector3D _rotation;
		#endregion

		#region "Constructors"

		#endregion

		#region "Factory Initialisers"
		public static IModel Load( string name, string path ) {
			if(!System.IO.File.Exists(path)) {
				throw new System.IO.FileNotFoundException("Could not load model '" + path + "'", path);
			}
			Model loadedModel = new Model();
			loadedModel._key = name;
			// todo: fix bounding radius
			loadedModel.BoundingSphereRadiusSquared = 100;

			try {
				R3D_3DStudio _3dsfile = new R3D_3DStudioClass();
				_3dsfile.File_Open(path);
				_3dsfile.File_ReadMaterials();
				_3dsfile.File_ReadTextures();
				//_3dsfile.File_ReadScene(loadedModel.Key, true, false, true);
				_3dsfile.File_ReadMeshes(name, true, true,true,true,true);	
				_3dsfile.File_Close();

				loadedModel._id = Engine.MeshBuilder.Mesh_Create( name );
				//Engine.Meshbuilder.Mesh_SetCullMode(R3DCULLMODE.R3DCULLMODE_DOUBLESIDED);
				//Engine.Meshbuilder.Mesh_SetLayerConfig(0, R3DLAYERCONFIG.R3DLAYERCONFIG_MONOCHROME);
				//System.Windows.Forms.MessageBox.Show(Engine.TextureLib.Class_GetNumTextures().ToString());
						
				loadedModel.BoundingSphereRadiusSquared = 1000;
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

		public void applyTexture( string texture ) {
			Engine.MeshBuilder.Class_SetPointer(this.Name);
			Engine.MeshBuilder.Mesh_SetTexture( 0, texture );
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
