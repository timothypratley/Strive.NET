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

		string _modelPath = "";
		string _texturePath = "";
		string _cursorPath = "";
		string _resourceServer = "";
		Hashtable _textures = new Hashtable();
		Hashtable _cursors = new Hashtable();
		Hashtable _models = new Hashtable();
		Hashtable _actors = new Hashtable();
		IEngine factory;

		IActor _placeHolderActor;
		IModel _placeHolderModel;

		public ResourceManager( IEngine factory ) {
			this.factory = factory;
		}


		public void SetPath( string path ) {
			_modelPath = System.IO.Path.Combine( path, "models" );
			_texturePath = System.IO.Path.Combine( path, "textures" );
			_cursorPath = System.IO.Path.Combine( path, "cursors" );
			_resourceServer = System.Configuration.ConfigurationSettings.AppSettings["ResourceServer"];

			// TODO:
			// try to load the placeholder models... this is a good sign if things are in order or not
			//_placeHolderModel = GetModel( 0, 0, 10 );
			//_placeHolderActor = GetActor( 0, 0, 10 );
		}

		IModel LoadModel( int InstanceID, int ModelID, float height ) {
			string _3dsFile = System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds");

			if ( System.IO.File.Exists(_3dsFile) ) {
				return factory.LoadStaticModel( InstanceID.ToString(), _3dsFile, height );
			}

			// download resource
			if (makeModelExist(System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds"))) {
				return factory.LoadStaticModel(InstanceID.ToString(), System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".3ds"), height);
			} else {
				throw new ResourceNotLoadedException(ModelID, ResourceType.Model);
			}
		}

		public IModel GetModel( int InstanceID, int ModelID, float height ) {
			IModel m = (IModel)_models[ModelID];
			if ( m == null ) {
				m = LoadModel( InstanceID, ModelID, height );
				_models.Add( ModelID, m );
			}
			return m.Duplicate( InstanceID.ToString(), height );
		}

		IActor LoadActor( int InstanceID, int ModelID, float height ) {
			string actorFile = System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".mdl");

			// check for local file first:
			if ( System.IO.File.Exists( actorFile ) ) {
				IActor m = factory.LoadActor( InstanceID.ToString(), actorFile, height );

				// for md2 have to apply a texture
				//ITexture t = LoadTexture( ModelID );
				//m.applyTexture( t );
				return m;
			} 

			// download resource
			if(makeModelExist(System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".mdl"))) {
				return factory.LoadActor(InstanceID.ToString(), System.IO.Path.Combine(_modelPath, ModelID.ToString() + ".mdl"), height);
			} else {
				throw new ResourceNotLoadedException(ModelID, ResourceType.Model);
			}
		}

		public IActor GetActor( int InstanceID, int ModelID, float height ) {
			IActor a = (IActor)_actors[ModelID];
			if ( a == null ) {
				a = LoadActor( InstanceID, ModelID, height );
				_actors.Add( ModelID, a );
			}
			return (IActor)a.Duplicate( InstanceID.ToString(), height );
		}


		#region Automatic Resource Downloading Stuffs

		private bool makeModelExist(string modelPath)
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

		private string getDownloadUrlFromModelPath(string modelPath)
		{
			string modelFragment  = modelPath.Substring(modelPath.IndexOf("\\models"));
			modelFragment = modelFragment.Replace("\\", "/");
			return _resourceServer + modelFragment;
		}

		private bool makeTextureExist(string TexturePath)
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

		private string getDownloadUrlFromTexturePath(string TexturePath)
		{
			string TextureFragment  = TexturePath.Substring(TexturePath.IndexOf("\\textures"));
			TextureFragment = TextureFragment.Replace("\\", "/");
			return _resourceServer + TextureFragment;

		}

		#endregion

		ITexture LoadTexture( int TextureID ) {
			// load from file
			string filename = System.IO.Path.Combine( _texturePath, TextureID.ToString() + ".bmp" );
			if ( !System.IO.File.Exists( filename ) && !makeTextureExist( filename ) ) {
				filename = System.IO.Path.Combine( _texturePath, TextureID.ToString() + ".dds" );
				if ( !System.IO.File.Exists( filename ) && !makeTextureExist( filename ) ) {
					throw new ResourceNotLoadedException( TextureID, ResourceType.Texture );
				}
			}
			return factory.LoadTexture( TextureID.ToString(), filename );
		}

		public ITexture GetTexture( int TextureID ) {
			ITexture t = (ITexture)_textures[TextureID];
			if ( t == null ) {
				t = LoadTexture( TextureID );
				_textures.Add( TextureID, t );
			}
			return t;
		}


		/// <summary>
		/// LoadCursor just loads a texture, but from the cursor directory
		/// </summary>
		/// <param name="CursorID"></param>
		/// <returns></returns>
		ITexture LoadCursor( int CursorID ) {
			// load from file
			string filename = System.IO.Path.Combine( _cursorPath, CursorID.ToString() + ".bmp" );
			if ( !System.IO.File.Exists( filename ) && !makeTextureExist( filename )) {
				filename = System.IO.Path.Combine( _cursorPath, CursorID.ToString() + ".dds" );
				if ( !System.IO.File.Exists( filename ) && !makeTextureExist( filename ) ) {
					throw new ResourceNotLoadedException( CursorID, ResourceType.Texture );
				}
			}
			return factory.LoadTexture( "cursor"+CursorID.ToString(), filename );
		}

		public ITexture GetCursor( int CursorID ) {
			ITexture t = (ITexture)_cursors[CursorID];
			if ( t == null ) {
				t = LoadTexture( CursorID );
				_cursors.Add( CursorID, t );
			}
			return t;
		}


		// TODO: umg this is a bit ghey...
		// really should explicitly destroy them all... though there is a chance they will be garbage collected
		public void DropAll() {
			_textures.Clear();
			_cursors.Clear();
			_models.Clear();
			_actors.Clear();
		}
	}
}
