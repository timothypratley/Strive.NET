using System;
using System.Windows.Forms;

using Strive.Rendering.Controls;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Math3D;

namespace Strive.Rendering
{

	/// <summary>
	/// From this class, all other classes in rendering are produced.
	/// Thus to use rendering you should instanciate a Factory
	/// from your favourite implementation and go from there.
	/// </summary>
	public interface IEngine
	{
		IScene CreateScene();

		ITerrain CreateTerrain( string name, ITexture texture, float y, float xy, float zy, float xzy );
		IModel LoadStaticModel(string name, string path);
		IActor LoadActor(string name, string path);

		ITexture LoadTexture(string name, string path);

		IMouse Mouse { get; }
		IKeyboard Keyboard{ get; }
		
		void Initialise(IWin32Window window, EnumRenderTarget target, Resolution resolution);
		void Terminate();
		IWin32Window RenderTarget { get; }

	}
}
