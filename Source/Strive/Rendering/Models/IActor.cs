using System;
using Strive.Math3D;

using Strive.Rendering;
using Strive.Rendering.Textures;

namespace Strive.Rendering.Models {
	/// <summary>
	/// Represents a wireframe with textures
	/// </summary>
	/// <remarks>This class is designed to shield clients from the internal workings of how models are stored and represented.</remarks>
	public interface IActor : IModel {
		#region "Methods"
		// todo: remove render, its crap to have here
		void Render();
		void playAnimation();
		void stopAnimation();
		void applyTexture( ITexture texture );
		#endregion

		#region "Properties"
		string AnimationSequence { set; }
		#endregion
	}

}
