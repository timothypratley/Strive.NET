using System;
using Strive.Math3D;

using Revolution3D8087b;
using Strive.Rendering;

namespace Strive.Rendering.Models
{
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public class Model : IManeuverable
	{

		#region "Fields"
		private ModelFormat _format = ModelFormat.Unspecified;
		private string _key;
		private Vector3D _position;
		private Vector3D _rotation;
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
		public static Model Load(string key, string path, ModelFormat format)
		{
			
			if(!System.IO.File.Exists(path))
			{
				throw new System.IO.FileNotFoundException("Could not load model", path);
			}
			
			// 1.0  Initialise object
			
			// 1.1 Load return type
			Model loadedModel = new Model();

			// 1.2 Initialise fields
			loadedModel._format = format;
			loadedModel._key = key;

			// 2.0 Create appropriate model in interop layer
			switch(format)
			{
				case ModelFormat.MDL:
				{
					try
					{
						Interop._instance.MdlSystem.MDL_Load(key, path);
					}
					catch(Exception e)
					{
						throw new ModelNotLoadedException(path, format, e);
					}
					loadedModel.setPointer();
					break;
				}
				case ModelFormat._3DS:
				{
					try
					{
						Interop._instance.Meshbuilder.Mesh_Create(key);
						Interop._instance.Meshbuilder.Mesh_SetPointer(key);
						//Interop._instance.Meshbuilder.Mesh_Add3DS(path, (bool)Type.Missing, (bool)Type.Missing, (bool)Type.Missing, false);
						Interop._instance.Meshbuilder.Mesh_Add3DS(path,true,true,true,false);
						
					}
					catch(Exception e)
					{
						throw new ModelNotLoadedException(path, format, e);
					}
					break;
				}

				default:
				{
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
		protected void setPointer()
		{
			try
			{
				Interop._instance.MdlSystem.MDL_SetPointer(this.Key);
			}
			catch(Exception e)
			{
				throw new ModelException("Could not set pointer to '" + this.Key + "'.", e);
			}
		}


		#endregion

		#region "Properties"
		/// <summary>
		/// The format of the underlying model
		/// </summary>
		public ModelFormat ModelFormat
		{
			get
			{
				return _format;
			}
		}
		/// <summary>
		/// The key of the underlying model
		/// </summary>
		public string Key
		{
			get
			{
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
							Interop._instance.MdlSystem.MDL_SetRotation(value.X, value.Y, value.Z);
						}
						catch(Exception e) {
							throw new ModelException("Could not set rotation '" + value.X + "' '" + value.Y + "' '" + value.Z + "' for model '" + this.Key + "'", e);
						}
						break;
					}

				}
				_rotation = value;
			}
		}
		#endregion
	}

}
