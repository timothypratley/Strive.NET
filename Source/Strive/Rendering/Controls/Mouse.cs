using System;
using R3D089_VBasic;

using Strive.Rendering.Controls;

namespace Strive.Rendering.R3D.Controls
{
	/// <summary>
	/// Summary description for Mouse.
	/// </summary>
	public class Mouse : IMouse
	{
		public int x, y;
		public bool button1down, button2down, button3down, button4down;
		public void GetState() {
			R3DMouseState ms = Engine.Control.Mouse_GetState( true );
			x = ms.x;
			y = ms.y;
			button1down = ms.iButton[0] != 0;
			button2down = ms.iButton[1] != 0;
			button3down = ms.iButton[2] != 0;
			button4down = ms.iButton[3] != 0;
		}

		public void ShowCursor( bool showCursor ) {
			//Engine.Tools.ShowCursor( ref showCursor );
		}

		public int X {
			get { return y; }
		}
		public int Y {
			get { return y; }
		}
		public bool Button1down {
			get { return button1down; }
		}
		public bool Button2down {
			get { return button2down; }
		}
		public bool Button3down {
			get { return button3down; }
		}
		public bool Button4down {
			get { return button4down; }
		}
	}
}
