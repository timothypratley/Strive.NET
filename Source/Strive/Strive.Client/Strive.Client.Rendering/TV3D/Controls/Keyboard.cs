using System;

using Strive.Rendering.Controls;
using TrueVision3D;

namespace Strive.Rendering.TV3D.Controls
{
	/// <summary>
	/// Deals with keyboard input
	/// </summary>
	public class Keyboard : IKeyboard
	{
		/// <summary>
		/// Determines if the key was pressed
		/// </summary>
		/// <param name="Key">The Key to check for</param>
		/// <returns>A true/false indicating that the Key was pressed</returns>
		public bool GetKeyState(Strive.Rendering.Controls.Key Key)
		{
			return Engine.Input.IsKeyPressed( Keys.getTVKeyFromKey(Key) );
		}
	}
}
