using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

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
		public static ArrayList _knownmodels = new ArrayList();

		public static void SetPath( string path ) {
			_modelPath = System.IO.Path.Combine( path, "models" );
			_texturePath = System.IO.Path.Combine( path, "textures" );
			_resourceServer = System.Configuration.ConfigurationSettings.AppSettings["ResourceServer"];
		}

		public static Model LoadModel(int InstanceID, int ModelID)
		{
			string md2File = System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".md2");
			string _3dsFile = System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds");
			string textureFile = System.IO.Path.Combine(_texturePath, ModelID.ToString() + ".bmp");

			// check for local file first:
			if ( System.IO.File.Exists(md2File) ) {
				Model m = Model.Load(InstanceID.ToString(), md2File, ModelFormat.MD2);
				LoadTexture( ModelID );
				m.applyTexture( ModelID.ToString() );
				return m;
			} else if ( System.IO.File.Exists(_3dsFile) ) {
				return Model.Load(InstanceID.ToString(), _3dsFile, ModelFormat.Mesh);
			} else if ( System.IO.File.Exists(textureFile) ) {
				string texture = LoadTexture( ModelID );
				return Model.CreateTerrain( InstanceID.ToString(), texture );
			}

			// download resource
			if(makeModelExist(System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".md2"))) {
				return Model.Load(InstanceID.ToString(), System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".md2"), ModelFormat.MD2);
			}
			else if (makeModelExist(System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds"))) {
				return Model.Load(InstanceID.ToString(), System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds"), ModelFormat.Mesh);
			}
			else if (makeTextureExist(System.IO.Path.Combine(_texturePath, ModelID.ToString() + ".bmp"))) {
				string texture = LoadTexture( ModelID );
				return Model.CreateTerrain( InstanceID.ToString(), texture );
			}
			else {
				throw new ResourceNotLoadedException(ModelID, ResourceType.Model);
			}
		}

		public static string LoadTexture( int TextureID ) {
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
