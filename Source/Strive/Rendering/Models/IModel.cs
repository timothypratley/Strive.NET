using System;
using Strive.Math3D;

using Strive.Rendering;

namespace Strive.Rendering.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public interface IModel : IManeuverable {
		#region "Methods"

		void Delete();
		IModel Duplicate( string name, float height );

		void GetBoundingBox( Vector3D boxmin, Vector3D boxmax );

		void SetLOD( float distance );

		#endregion

		#region "Properties"
		string Name { get; }
		int ID { get; }
		string Label { get; set; }	// a label appears above the 3d object
		float RadiusSquared { get; }
		bool Visible { get; set; }
		float Height { get; }

		#endregion
	}

}
