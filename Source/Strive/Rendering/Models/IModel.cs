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

		void Hide();

		void Show();

		void GetBoundingBox( Vector3D boxmin, Vector3D boxmax );

		#endregion

		#region "Properties"
		string Name { get; }
		int ID { get; }
		float RadiusSquared { get; }

		#endregion
	}

}
