using System;
using System.IO;
using Strive.Rendering;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Math3D;


namespace Strive.Resources
{
	/// <summary>
	/// Summary description for ModelLoader.
	/// </summary>
	public class ResourceManager
	{

		public static string _modelPath = "";
		public static string _texturePath = "";

		public static void SetPath( string path ) {
			_modelPath = System.IO.Path.Combine( path, "models" );
			_texturePath = System.IO.Path.Combine( path, "textures" );
		}

		public static Model LoadModel(int spawnID, int modelID)
		{
			// check MDL first:
			
			if(System.IO.File.Exists(System.IO.Path.Combine(_modelPath, modelID.ToString() + ".mdl"))) {
				return Model.Load(spawnID.ToString(), System.IO.Path.Combine(_modelPath, modelID.ToString() + ".mdl"), ModelFormat.MDL);
			}
			else if (System.IO.File.Exists(System.IO.Path.Combine(_modelPath, modelID.ToString() + ".3ds"))) {
				return Model.Load(spawnID.ToString(), System.IO.Path.Combine(_modelPath, modelID.ToString() + ".3ds"), ModelFormat._3DS);
			}
			else if (System.IO.File.Exists(System.IO.Path.Combine(_texturePath, modelID.ToString() + ".bmp"))) {
				string texture = LoadTexture( modelID );
				return Model.CreatePlane(modelID.ToString() + spawnID.ToString(),
					new Vector3D(-50,0,-50), 
					new Vector3D(-50,0,50), 
					new Vector3D(50,0,50), 
					new Vector3D(50,0,-50), texture, "");
			}
			else {
				throw new ResourceNotLoadedException(modelID, ResourceType.Model);
			}
		}

		public static string LoadTexture( int TextureID ) {
			Texture.LoadTexture( TextureID.ToString(), System.IO.Path.Combine( _texturePath, TextureID.ToString() + ".bmp" ) );			
			return TextureID.ToString();
		}
	}
}
