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
		public static string _cursorPath = "";
		public static string _resourceServer = "";
		public static Hashtable _textures = new Hashtable();
		public static IEngine factory;

		public static void SetPath( string path ) {
			_modelPath = System.IO.Path.Combine( path, "models" );
			_texturePath = System.IO.Path.Combine( path, "textures" );
			_cursorPath = System.IO.Path.Combine( path, "cursors" );
			_resourceServer = System.Configuration.ConfigurationSettings.AppSettings["ResourceServer"];
		}

		public static IModel LoadModel( int InstanceID, int ModelID, float height )
		{
			string actorFile = System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".mdl");
			string _3dsFile = System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds");

			// check for local file first:
			if ( System.IO.File.Exists( actorFile ) ) {
				IActor m = factory.LoadActor( InstanceID.ToString(), actorFile, height );

				// for md2 have to apply a texture
				//ITexture t = LoadTexture( ModelID );
				//m.applyTexture( t );
				return m;
			} else if ( System.IO.File.Exists(_3dsFile) ) {
				return factory.LoadStaticModel( InstanceID.ToString(), _3dsFile, height );
			}

			// download resource
			if(makeModelExist(System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".mdl"))) {
				return factory.LoadActor(InstanceID.ToString(), System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".mdl"), height);
			}
			else if (makeModelExist(System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds"))) {
				return factory.LoadStaticModel(InstanceID.ToString(), System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds"), height);
			}
			else {
				throw new ResourceNotLoadedException(ModelID, ResourceType.Model);
			}
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
				catch(Exception )
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

		public static ITexture LoadTexture( int TextureID ) {
			// see if its already loaded
			ITexture texture = (ITexture)_textures[TextureID];
			if ( texture == null ) {
				// load from file
				string filename = System.IO.Path.Combine( _texturePath, TextureID.ToString() + ".bmp" );
				if ( !System.IO.File.Exists( filename ) && !makeTextureExist( filename ) ) {
					filename = System.IO.Path.Combine( _texturePath, TextureID.ToString() + ".dds" );
					if ( !System.IO.File.Exists( filename ) && !makeTextureExist( filename ) ) {
						throw new ResourceNotLoadedException( TextureID, ResourceType.Texture );
					}
				}
				texture = factory.LoadTexture( TextureID.ToString(), filename );
				_textures.Add( TextureID, texture );
			}
			return texture;
		}

		/// <summary>
		/// LoadCursor just loads a texture, but from the cursor directory
		/// </summary>
		/// <param name="CursorID"></param>
		/// <returns></returns>
		public static ITexture LoadCursor( int CursorID ) {
			// see if its already loaded
			ITexture texture = (ITexture)_textures[CursorID];
			if ( texture == null ) {
				// load from file
				string filename = System.IO.Path.Combine( _cursorPath, CursorID.ToString() + ".dds" );
				if ( !System.IO.File.Exists( filename ) && !makeTextureExist( filename ) ) {
					filename = System.IO.Path.Combine( _cursorPath, CursorID.ToString() + ".dds" );
					if ( !System.IO.File.Exists( filename ) && !makeTextureExist( filename ) ) {
						throw new ResourceNotLoadedException( CursorID, ResourceType.Texture );
					}
				}
				texture = factory.LoadTexture( CursorID.ToString(), filename );
				_textures.Add( CursorID, texture );
			}
			return texture;
		}

		// TODO: umg this is a bit ghey...
		// maybe resourcemanager should be in rendering
		public static void DropAll() {
			_textures.Clear();
		}
	}
}
