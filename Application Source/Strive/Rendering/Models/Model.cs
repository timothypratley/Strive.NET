using System;
using Strive.Math3D;

using Revolution3D8088c;
using Strive.Rendering;

namespace Strive.Rendering.Models
{
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public class Model : IManeuverable {

		#region "Fields"
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
			R3DPoint3D p1, p2, p3, p4;
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
			Interop._instance.Meshbuilder.Mesh_AddPlane( ref p1, ref p2, ref p3, ref p4, texture, material, R3DBLENDMODE.R3DBLENDMODE_NONE, R3DCULLMODE.R3DCULLMODE_DOUBLESIDED );
			Model created = new Model();
			created._key = name;
			created._id = id;
			created._format = ModelFormat._3DS;
			return created;
		}

		public static Model CreateBox( string name, float w, float h, float d, string texture, string material ) {
			int id = Interop._instance.Meshbuilder.Mesh_Create( name );
			Interop._instance.Meshbuilder.Mesh_AddBox( texture, material, w, h, d, R3DBLENDMODE.R3DBLENDMODE_NONE, R3DCULLMODE.R3DCULLMODE_DOUBLESIDED );
			Model created = new Model();
			created._key = name;
			created._id = id;
			return created;
		}

		/// <summary>
		/// Loads a model
		/// </summary>
		/// <param name="key">The key of the model</param>
		/// <param name="path">The path at which the model can be loaded</param>
		/// <param name="format">The format of the model</param>
		/// <returns>A reference to the loaded Model</returns>
		public static Model Load(string key, string path, ModelFormat format) {
			
			if(!System.IO.File.Exists(path))
			{
				throw new System.IO.FileNotFoundException("Could not load model '" + path + "'", path);
			}
			
			// 1.0  Initialise object
			
			// 1.1 Load return type
			Model loadedModel = new Model();

			// 1.2 Initialise fields
			loadedModel._format = format;
			loadedModel._key = key;

			// 2.0 Create appropriate model in interop layer
			switch(format) {
				case ModelFormat.MDL: {
					try {
						Interop._instance.MdlSystem.MDL_Load(key, path);
						Interop._instance.MdlSystem.MDL_SetRotationAxis( R3DROTATIONAXIS.R3DAXIS_RELATIVE );
					}
					catch(Exception e) {
						throw new ModelNotLoadedException(path, format, e);
					}
					break;
				}
				case ModelFormat._3DS:
				{
					try
					{
						// Changed for 8088c
						R3D_3DSFile _3dsfile = new R3D_3DSFileClass();
						_3dsfile.File_Open(path);
											
						_3dsfile.File_Close();

						Interop._instance.Meshbuilder.Mesh_Create(key);
						Interop._instance.Meshbuilder.Mesh_Add3DS( path, true, true, true, true);
						Interop._instance.Meshbuilder.Mesh_SetRotationAxis( R3DROTATIONAXIS.R3DAXIS_RELATIVE );
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
		/// Sets the internal MDL pointer
		/// </summary>
		protected void setPointer() {
			try {
				if ( _format == ModelFormat.MDL ) {
					Interop._instance.MdlSystem.MDL_SetPointer(this.Key);
				} else  {
					Interop._instance.Meshbuilder.Mesh_SetPointer(this.Key);
				}
			}
			catch(Exception e) {
				throw new ModelException("Could not set pointer to '" + this.Key + "'.", e);
			}
		}


		#endregion

		#region "Methods"

		public void applyTexture( string texture ) {
			setPointer();
			Interop._instance.Meshbuilder.Mesh_SetTexture( 0, texture );
		}

		public void nextFrame() {
			setPointer();
			if ( frame_count > 0 ) {
				frame = (frame+1)%frame_count;
				Interop._instance.MdlSystem.MDL_SequenceSetFrame( (float)frame );
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

		public float BoundingSphereRadius {
			get {
				R3DVector3D center = new R3DVector3D();
				float radius = 10;
				bool worldspace = false;
				setPointer();
				Interop._instance.Meshbuilder.Mesh_GetBoundingSphere( ref center, ref radius, ref worldspace );
				return radius;
			}
		}

		public int AnimationSequence {
			set {
				string sequence;		
				switch( value ) {
					case 1:
						sequence = "deadback";
						break;
					case 2:
						sequence = "deadstomach";
						break;
					case 3:
						sequence = "deadsitting";
						break;
					case 4:
						sequence = "crouch_idle";
						break;
					case 5:
						sequence = "deep_idle";
						break;
					case 6:
						sequence = "walk2handed";
						break;
					case 7:
						sequence = "running";
						break;
					case 8:
						sequence = "ref_shoot_crowbar";
						break;
					default:
						throw new Exception( "Unknown sequence" );
				}
				Interop._instance.MdlSystem.MDL_SequenceSet( sequence );
				frame = 0;
				frame_count = Interop._instance.MdlSystem.MDL_SequenceGetFrameCount();
				Interop._instance.MdlSystem.MDL_SequenceSetFrame( frame );
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
				case ModelFormat.MDL:
				{
					setPointer();
					try
					{
						Interop._instance.MdlSystem.MDL_SetPosition(newPosition.X, newPosition.Y, newPosition.Z);
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
				case ModelFormat.MDL:
				{
					setPointer();
					try
					{
						Interop._instance.MdlSystem.MDL_SetRotation(newRotation.X, newRotation.Y, newRotation.Z);
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
				switch(_format) {
					case ModelFormat.MDL: {
						setPointer();
						try {
							Interop._instance.MdlSystem.MDL_SetPosition(value.X, value.Y, value.Z);
						}
						catch(Exception e) {
							throw new ModelException("Could not set position '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for model '" + this.Key + "'", e);
						}
						break;
					}
					default: {
						setPointer();
						try {
							Interop._instance.Meshbuilder.Mesh_SetPosition(value.X, value.Y, value.Z);
						}
						catch(Exception e) {
							throw new ModelException("Could not set position '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for model '" + this.Key + "'", e);
						}
						break;
					}
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
				switch(_format) {
					case ModelFormat.MDL: {
						setPointer();
						try {
							Interop._instance.MdlSystem.MDL_SetRotation(-value.X, -value.Y, -value.Z);
						}
						catch(Exception e) {
							throw new ModelException("Could not set rotation '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for model '" + this.Key + "'", e);
						}
						break;
					}
					case ModelFormat._3DS: {
						setPointer();
						Interop._instance.Meshbuilder.Mesh_SetRotation( -value.X, -value.Y, -value.Z );
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
