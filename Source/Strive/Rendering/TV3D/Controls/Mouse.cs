using System;
using TrueVision3D;

using Strive.Rendering.Controls;

namespace Strive.Rendering.TV3D.Controls
{
	/// <summary>
	/// Summary description for Mouse.
	/// </summary>
	public class Mouse : IMouse
	{
		int x=0, y=0;
		short button1down=0, button2down=0, button3down=0, button4down=0;
		int intellimouseroll=0;
		
		public void GetState() {
			Engine.Input.GetMouseState(ref x, ref y, ref button1down, ref button2down, ref button3down, ref intellimouseroll );
		}

		public void ShowCursor( bool showCursor ) {
			//Engine.Tools.ShowCursor( ref showCursor );
		}

		public int X {
			get { return x; }
		}
		public int Y {
			get { return y; }
		}
		public bool Button1down {
			get { return button1down!=0; }
		}
		public bool Button2down {
			get { return button2down!=0; }
		}
		public bool Button3down {
			get { return button3down!=0; }
		}
		public bool Button4down {
			get { return button4down!=0; }
		}
	}
}
