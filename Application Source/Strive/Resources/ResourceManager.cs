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

		public static Model LoadModel(int InstanceID, int ModelID)
		{
			// check MDL first:
			
			if(System.IO.File.Exists(System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".mdl"))) {
				return Model.Load(InstanceID.ToString(), System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".mdl"), ModelFormat.MDL);
			}
			else if (System.IO.File.Exists(System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds"))) {
				return Model.Load(InstanceID.ToString(), System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds"), ModelFormat._3DS);
			}
			else if (System.IO.File.Exists(System.IO.Path.Combine(_texturePath, ModelID.ToString() + ".bmp"))) {
				string texture = LoadTexture( ModelID );
				return Model.CreatePlane(InstanceID.ToString(),
					new Vector3D(-50,0,-50), 
					new Vector3D(-50,0,50), 
					new Vector3D(50,0,50), 
					new Vector3D(50,0,-50), texture, "");
			}
			else {
				throw new ResourceNotLoadedException(ModelID, ResourceType.Model);
			}
		}

		public static string LoadTexture( int TextureID ) {
			Texture.LoadTexture( TextureID.ToString(), System.IO.Path.Combine( _texturePath, TextureID.ToString() + ".bmp" ) );			
			return TextureID.ToString();
		}
	}
}
