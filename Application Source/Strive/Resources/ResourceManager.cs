using System;
using System.IO;
using Strive.Rendering;
using Strive.Rendering.Models;


namespace Strive.Resources
{
	/// <summary>
	/// Summary description for ModelLoader.
	/// </summary>
	public class ResourceManager
	{

		public static string _modelPath = "";

		public static void SetPath( string path ) {
			_modelPath = path;
		}

		public static Model LoadModel(int spawnID, int modelID)
		{
			// check MDL first:
			
			if(System.IO.File.Exists(System.IO.Path.Combine(_modelPath, modelID.ToString() + ".mdl")))
			{
				return Model.Load(spawnID.ToString(), System.IO.Path.Combine(_modelPath, modelID.ToString() + ".mdl"), ModelFormat.MDL);
			}
			else if (System.IO.File.Exists(System.IO.Path.Combine(_modelPath, modelID.ToString() + ".3ds")))
			{
				return Model.Load(spawnID.ToString(), System.IO.Path.Combine(_modelPath, modelID.ToString() + ".3ds"), ModelFormat._3DS);
			}
			else
			{
				throw new ResourceNotLoadedException(modelID, ResourceType.Model);
			}

		}
	}
}
