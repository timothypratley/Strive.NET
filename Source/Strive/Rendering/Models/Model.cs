using System;
using Strive.Math3D;

using R3D089_VBasic;
using Strive.Rendering;

namespace Strive.Rendering.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public class Model : IManeuverable {

		#region "Fields"
		public float BoundingSphereRadiusSquared;

		private ModelFormat _format = ModelFormat.Unspecified;
		private string _key;
		private int _id;
		private Vector3D _position;
		private Vector3D _rotation;
		private int frame = 0;
		private int frame_count = 0;
		#endregion

		#region "Constructors"

		#endregion

		#region "Factory Initialisers"
		public static Model CreatePlane( string name, Vector3D point1, Vector3D point2, Vector3D point3, Vector3D point4, string texture, string material ) {
			int id = Interop._instance.Meshbuilder.Mesh_Create( name );
			R3DVector3D p1, p2, p3, p4;
			p1.x = point1.X;
			p1.y = point1.Y;
			p1.z = point1.Z;
			p2.x = point2.X;
			p2.y = point2.Y;
			p2.z = point2.Z;
			p3.x = point3.X;
			p3.y = point3.Y;
			p3.z = point3.Z;
			p4.x = point4.X;
			p4.y = point4.Y;
			p4.z = point4.Z;
			Interop._instance.Meshbuilder.Mesh_AddPlane( ref p1, ref p2, ref p3, ref p4, texture, material, R3DBLENDMODE.R3DBLENDMODE_NONE, true);
			Model created = new Model();
			created._key = name;
			created._id = id;
			created._format = ModelFormat.Mesh;
			// todo: fix this hax,
			// atm set to 0 as this is assumed to be terrain...
			created.BoundingSphereRadiusSquared = 0;
			return created;
		}

		public static Model CreateBox( string name, float w, float h, float d, string texture, string material ) {
			int id = Interop._instance.Meshbuilder.Mesh_Create( name );
			R3DVector3D box = VectorConverter.GetR3DVector3DFromVector3D(new Vector3D(w,h,d));
			Interop._instance.Meshbuilder.Mesh_AddBox(ref box,  texture, material,R3DBLENDMODE.R3DBLENDMODE_NONE, true);
			Model created = new Model();
			created._key = name;
			created._id = id;
			created._format = ModelFormat.Mesh;
			R3DVector3D center = new R3DVector3D();
			bool worldspace = false;
			Interop._instance.Meshbuilder.Mesh_GetBoundingSphere( ref center, ref created.BoundingSphereRadiusSquared, ref worldspace );
			// square it ofc
			created.BoundingSphereRadiusSquared = created.BoundingSphereRadiusSquared * created.BoundingSphereRadiusSquared;
			return created;
		}

		public static Model CreateTerrain( string name, string filename, string texture ) {
			Model created = CreatePlane(name, 
				new Vector3D(0,0,0),
				new Vector3D(100,0,0),
				new Vector3D(100,0,100),
				new Vector3D(0,0,100),
				texture,
				"");
			return created;

			//			Model created = new Model();
			//			created._format = ModelFormat.Scape;
			//			created._key = name;
			//			// TODO TIM: Fix
			//			// P.S.  Why does this method exist anyway?
			//			R3DVector2D size = new R3DVector2D();
			//			size.x = 128;
			//			size.y = 128;
			//			created._id = Interop._instance.PolyVox.Scape_Create(name, ref size, 1);
			//			//created._id = Interop._instance.PolyVox.Scape_Create( name, filename, 500, false, POLYVOXDETAIL.POLYVOXDETAIL_LOW );
			//			R3DVector3D scale = new R3DVector3D();
			//			scale.x = 0.390625F;
			//			scale.y = 0.390625F;
			//			scale.z = 0.390625F;
			//			//Interop._instance.PolyVox.Scape_ApplyHeightMap(
			//			//Interop._instance.PolyVox.Scape_SetScale( ref scale );
			//			// TODO TIM: Fix
			//			Interop._instance.PolyVox.Scape_SetTexture( 0, texture);
			//			return created;
		}

		/// <summary>
		/// Loads a model
		/// </summary>
		/// <param name="key">The key of the model</param>
		/// <param name="path">The path at which the model can be loaded</param>
		/// <param name="format">The format of the model</param>
		/// <returns>A reference to the loaded Model</returns>
		public static Model Load(string key, string path, ModelFormat format) {
			
			if(!System.IO.File.Exists(path)) {
				throw new System.IO.FileNotFoundException("Could not load model '" + path + "'", path);
			}
			
			// 1.0  Initialise object
			
			// 1.1 Load return type
			Model loadedModel = new Model();


			// 1.2 Initialise fields
			loadedModel._format = format;
			loadedModel._key = key;
			// todo: fix bounding radius
			loadedModel.BoundingSphereRadiusSquared = 100;

			// 2.0 Create appropriate model in interop layer
			switch(format) {
				case ModelFormat.MD2: {
					try {
						Interop._instance.MD2System.Model_Load(path, key);
					}
					catch(Exception e) {
						throw new ModelNotLoadedException(path, format, e);
					}
					break;
				}
				case ModelFormat.Mesh: {
					try {
						// Changed for 8088c
						R3D_3DStudio _3dsfile = new R3D_3DStudioClass();
						_3dsfile.File_Open(path);
						_3dsfile.File_ReadMaterials();
						_3dsfile.File_ReadTextures();
						//_3dsfile.File_ReadScene(loadedModel.Key, true, false, true);
						_3dsfile.File_ReadMeshes(key, true, true,true,true,true);	
						_3dsfile.File_Close();
						
						Interop._instance.Meshbuilder.Class_SetPointer(key);
						//Interop._instance.Meshbuilder.Mesh_SetCullMode(R3DCULLMODE.R3DCULLMODE_DOUBLESIDED);
						//Interop._instance.Meshbuilder.Mesh_SetLayerConfig(0, R3DLAYERCONFIG.R3DLAYERCONFIG_MONOCHROME);
						//System.Windows.Forms.MessageBox.Show(Interop._instance.TextureLib.Class_GetNumTextures().ToString());
						
						loadedModel.BoundingSphereRadiusSquared = 1000;
					}
					catch(Exception e) {
						throw new ModelNotLoadedException(path, format, e);
					}
					break;
				}

				default: {
					// Unhandled model format encountered
					throw new ModelFormatUnknownException(path, format);
				}
			}
			loadedModel.Position = Vector3D.Origin;
			return loadedModel;
		}
		#endregion

		#region "Operators"
		/// <summary>
		/// Sets the internal pointer
		/// </summary>
		protected void setPointer() {
			try {
				if ( _format == ModelFormat.MD2 ) {
					Interop._instance.MD2System.Class_SetPointer(this.Key);
				} else if ( _format == ModelFormat.Mesh )  {
					Interop._instance.Meshbuilder.Class_SetPointer(this.Key);
				} else if ( _format == ModelFormat.Scape ) {
					Interop._instance.PolyVox.Class_SetPointer( this.Key );
				} else {
					throw new Exception( "n0rty n0rty, unknown modelformat" );
				}
			}
			catch(Exception e) {
				throw new ModelException("Could not set pointer to '" + this.Key + "'.", e);
			}
		}


		#endregion

		#region "Methods"

		public void Delete() {
			if ( _format == ModelFormat.MD2 ) {
				Interop._instance.MD2System.Class_SetPointer(this.Key);
				Interop._instance.MD2System.Model_Release();
			} else if ( _format == ModelFormat.Scape ) {
				Interop._instance.PolyVox.Class_SetPointer(this.Key);
				Interop._instance.PolyVox.Scape_Release();
			} else if ( _format == ModelFormat.Mesh ) {
				Interop._instance.Meshbuilder.Class_SetPointer(this.Key);
				Interop._instance.Meshbuilder.Mesh_Release();
			} else {
				throw new Exception( "n0rty n0rty, unknown format for delete" );
			}
		}

		public void Hide() {
			if ( _format == ModelFormat.MD2 ) {
				Interop._instance.MD2System.Class_SetPointer(this.Key);
				Interop._instance.MD2System.Model_SetActivate( false );
			} else if ( _format == ModelFormat.Scape ) {
				// no equivalent
			} else if ( _format == ModelFormat.Mesh ) {
				Interop._instance.Meshbuilder.Class_SetPointer(this.Key);
				Interop._instance.Meshbuilder.Mesh_SetActivate( false );
			} else {
				throw new Exception( "n0rty n0rty, unknown format for hide" );
			}
		}

		public void Show() {
			if ( _format == ModelFormat.MD2 ) {
				Interop._instance.MD2System.Class_SetPointer(this.Key);
				Interop._instance.MD2System.Model_SetActivate( true );
			} else if ( _format == ModelFormat.Scape ) {
				// no equivalent
			} else if ( _format == ModelFormat.Mesh ) {
				Interop._instance.Meshbuilder.Class_SetPointer(this.Key);
				Interop._instance.Meshbuilder.Mesh_SetActivate( true );
			} else {
				throw new Exception( "n0rty n0rty, unknown format for show" );
			}
		}

		public void applyTexture( string texture ) {
			if ( _format == ModelFormat.Mesh ) {
				setPointer();
				Interop._instance.Meshbuilder.Mesh_SetTexture( 0, texture );
			} else if ( _format == ModelFormat.MD2 ) {
				setPointer();
				Interop._instance.MD2System.Model_SetTexture( 0, texture );
			} else {
				throw new Exception( "n0rty n0rty, trying to applytexture to non-mesh" );
			}
		}

		public void nextFrame() {
			setPointer();
			if ( frame_count > 0 ) {
				frame = (frame+1)%frame_count;
				Interop._instance.MD2System.Model_SetFrameEx(R3DLERPNODETYPE.R3DLERPNODETYPE_START,  (byte)frame );
			}
		}

		#endregion

		#region "Properties"
		/// <summary>
		/// The format of the underlying model
		/// </summary>
		public ModelFormat ModelFormat {
			get {
				return _format;
			}
		}
		/// <summary>
		/// The key of the underlying model
		/// </summary>
		public string Key {
			get {
				return _key;
			}
		}


		public int AnimationSequence {
			set {
				if ( _format != ModelFormat.MD2 ) {
					throw new Exception( "n0rty n0rty there is a mobile with a non md2 model in the database" );
				}
				setPointer();
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
				//Interop._instance.MD2System.Model_Animate();
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

			switch(_format)
			{
				case ModelFormat.MD2:
				{
					setPointer();
					try
					{
						R3DVector3D vector = VectorConverter.GetR3DVector3DFromVector3D(newPosition);
						Interop._instance.MD2System.Model_SetPosition(ref vector);
					}
					catch(Exception e)
					{
						throw new ModelException("Could not set position '" + newPosition.X + "' '" + newPosition.Y + "' '" + newPosition.Z + "' for model '" + this.Key + "'", e);
					}
					break;
				}
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

			switch(_format)
			{
				case ModelFormat.MD2:
				{
					setPointer();
					try
					{
						R3DVector3D vector = VectorConverter.GetR3DVector3DFromVector3D(newRotation);
						Interop._instance.MD2System.Model_SetRotation(ref vector);
					}
					catch(Exception e)
					{
						throw new ModelException("Could not set rotation '" + newRotation.X + "' '" + newRotation.Y + "' '" + newRotation.Z + "' for model '" + this.Key + "'", e);
					}
					break;
				}

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
				switch(_format) {
					case ModelFormat.MD2: {
						setPointer();
						try {

							Interop._instance.MD2System.Model_SetPosition(ref vector);
						}
						catch(Exception e) {
							throw new ModelException("Could not set position '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for model '" + this.Key + "'", e);
						}
						break;
					}
					case ModelFormat.Mesh: {
						setPointer();
						try {
		
							Interop._instance.Meshbuilder.Mesh_SetPosition(ref vector);
						}
						catch(Exception e) {
							throw new ModelException("Could not set position '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for model '" + this.Key + "'", e);
						}
						break;
					}
					case ModelFormat.Scape: {
						setPointer();
						try {
							Interop._instance.PolyVox.Scape_SetPosition( ref vector);
						}
						catch(Exception e) {
							throw new ModelException("Could not set position '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for model '" + this.Key + "'", e);
						}
						break;
					}
					default:
						throw new Exception( "n0rty norty, bad modelformat" );
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
				switch(_format) {
					case ModelFormat.MD2: {
						setPointer();
						try {
							// todo: normalise MD2s so we don't need a 90 degree offset?
							// todo: our mobs are at different angles to players :(
							Interop._instance.MD2System.Model_SetRotation(ref vector);
						}
						catch(Exception e) {
							throw new ModelException("Could not set rotation '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for model '" + this.Key + "'", e);
						}
						break;
					}
					case ModelFormat.Mesh:
					{
						setPointer();
						Interop._instance.Meshbuilder.Mesh_SetRotation(ref vector );
						break;
					}
					case ModelFormat.Scape: {
						setPointer();
						R3DVector3D rotation = new R3DVector3D();
						rotation.x = -value.X;
						rotation.y = -value.Y;
						rotation.z = -value.Z;
						Interop._instance.PolyVox.Scape_SetRotation( ref rotation );
						break;					
					}
					default:
						throw new Exception( "fall through" );
				
				}
				_rotation = value;
			}
		}
		#endregion
	}

}
