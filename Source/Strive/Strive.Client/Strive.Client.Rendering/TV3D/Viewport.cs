using System;

using TrueVision3D;

namespace Strive.Rendering.TV3D
{
	/// <summary>
	/// Summary description for Viewport.
	/// </summary>
	public class Viewport : IViewport
	{
		public TVViewport viewport;
		public TVCamera camera;
		public Camera strivecamera;
		
		public Viewport( System.Windows.Forms.IWin32Window window, string name ) {
			viewport = Engine.TV3DEngine.CreateViewport( window.Handle.ToInt32(), name );
			viewport.SetAutoResize( true );
			camera = viewport.GetCamera();
			strivecamera = new Camera( camera );
		}

		public ICamera Camera {
			get {
				return strivecamera;
			}
		}

		public void SetFocus() {
			Engine.TV3DEngine.SetViewport( ref viewport, true );
		}
	}
}
