using System;

using Strive.Rendering.Controls;
using DxVBLibA;
using R3D089_VBasic;

namespace Strive.Rendering.R3D.Controls
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
			R3DKey r = (R3DKey)Key;
			return Engine.Control.Keyboard_GetKeyState(ref r);
		}
	}
}
