using System;
using System.IO;
using System.Net;

using Strive.Common;
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
		public static string _resourceServer = "";

		public static void SetPath( string path ) {
			_modelPath = System.IO.Path.Combine( path, "models" );
			_texturePath = System.IO.Path.Combine( path, "textures" );
			_resourceServer = System.Configuration.ConfigurationSettings.AppSettings["ResourceServer"];
		}

		public static Model LoadModel(int InstanceID, int ModelID)
		{
			// check MDL first:
			
			if(makeModelExist(System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".mdl"))) {
				return Model.Load(InstanceID.ToString(), System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".mdl"), ModelFormat.MDL);
			}
			else if (makeModelExist(System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds"))) {
				return Model.Load(InstanceID.ToString(), System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds"), ModelFormat._3DS);
			}
			else if (makeTextureExist(System.IO.Path.Combine(_texturePath, ModelID.ToString() + ".bmp"))) {
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
			// omg SOMEONE didn't add TEXTURES.CS to cvs
			Texture.LoadTexture( TextureID.ToString(), System.IO.Path.Combine( _texturePath, TextureID.ToString() + ".bmp" ) );			
			return TextureID.ToString();
		}

		#region Automatic Resource Downloading Stuffs

		private static bool makeModelExist(string modelPath)
		{
			if(System.IO.File.Exists(modelPath))
			{
				return true;
			}

			string downloadUrl = getDownloadUrlFromModelPath(modelPath);

			if(Http.UrlTargetExists(new Uri(downloadUrl)))
			{
				try
				{
					Http.SaveUrlTargetToDisk(new Uri(downloadUrl), modelPath);
					return true;
				}
				catch(Exception e)
				{
				}
			}

			return false;
			
		}

		private static string getDownloadUrlFromModelPath(string modelPath)
		{
			string modelFragment  = modelPath.Substring(modelPath.IndexOf("\\models"));
			modelFragment = modelFragment.Replace("\\", "/");
			return _resourceServer + modelFragment;

		}

		private static bool makeTextureExist(string TexturePath)
		{
			if(System.IO.File.Exists(TexturePath))
			{
				return true;
			}

			string downloadUrl = getDownloadUrlFromTexturePath(TexturePath);

			if(Http.UrlTargetExists(new Uri(downloadUrl)))
			{
				try
				{
					Http.SaveUrlTargetToDisk(new Uri(downloadUrl), TexturePath);
					return true;
				}
				catch(Exception)
				{
				}
			}

			return false;
			
		}

		private static string getDownloadUrlFromTexturePath(string TexturePath)
		{
			string TextureFragment  = TexturePath.Substring(TexturePath.IndexOf("\\textures"));
			TextureFragment = TextureFragment.Replace("\\", "/");
			return _resourceServer + TextureFragment;

		}

		#endregion


	}
}
