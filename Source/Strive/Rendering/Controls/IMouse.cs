using System;

namespace Strive.Rendering.Controls
{
	/// <summary>
	/// Summary description for Mouse.
	/// </summary>
	public interface IMouse
	{
		int X { get; }
		int Y { get; }
		bool Button1down { get; }
		bool Button2down { get; }
		bool Button3down { get; }
		bool Button4down { get; }

		void GetState();
		void ShowCursor( bool showCursor );
	}
}
