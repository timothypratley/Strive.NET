using System;

namespace Strive.Rendering.Controls
{
	/// <summary>
	/// Summary description for Mouse.
	/// </summary>
	public class Mouse
	{
		public int x, y;
		public bool button1down, button2down, button3down, button4down;
		public static Mouse GetState() {
			Mouse m = new Mouse();
			Interop._instance.Control.Mouse_GetState( ref m.x, ref m.y, ref m.button1down, ref m.button2down, ref m.button3down, ref m.button4down, true );
			return m;
		}

		public static void ShowCursor( bool showCursor ) {
			//Interop._instance.Tools.ShowCursor( ref showCursor );
		}
	}
}
