using System;
using Revolution3D8088c;

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
			R3DMouseState_Type ms = Interop._instance.Control.Mouse_GetState( true );
			m.x = ms.x;
			m.y = ms.y;
			m.button1down = ms.iButton[0] != 0;
			m.button2down = ms.iButton[1] != 0;
			m.button3down = ms.iButton[2] != 0;
			m.button4down = ms.iButton[3] != 0;
			return m;
		}

		public static void ShowCursor( bool showCursor ) {
			//Interop._instance.Tools.ShowCursor( ref showCursor );
		}
	}
}
