using System;
using System.Threading;

using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Math3D;

namespace Strive.Rendering
{

	/// <summary>
	/// Represent a Scene.  A scene contains everything that will be rendered.
	/// </summary>
	public interface IScene
	{
		#region "Methods"
		void DropAll();
		void SetTimeOfDay( float hour );
		void SetSky( ITexture texture );
		void SetSky( ITexture night, ITexture day, ITexture cusp, ITexture sun );
		void SetClouds( ITexture texture );
		void SetLighting( float level );
		void SetFog( float level );
		void DrawText( Vector3D location, string message );
		void Clear();
		void Render();
		void RenderAtmosphere();
		void RenderToOtherWindow( System.Windows.Forms.IWin32Window hwnd );
		void Display();
		int RayCollision( Vector3D start_point, Vector3D end_point, int collision_type );
		IModel MousePick( int x, int y );
		IModel MousePick();
		void SetCursor( ITexture texture );

		#endregion
 
		#region "Properties"
		/// <summary>
		/// Indiactes whether the scene is being rendered
		/// </summary>
		bool IsRendering { get; }

		/// <summary>
		/// Model collection
		/// </summary>
		IModelCollection Models { get; }

		/// <summary>
		/// Returns the default view
		/// </summary>
		ICamera Camera { get; }
		

		#endregion

	}
}
