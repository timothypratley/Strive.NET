using System;

using DxVBLibA;
using R3D089_VBasic;

namespace Strive.Rendering.Controls
{
	/// <summary>
	/// Deals with keyboard input
	/// </summary>
	public class Keyboard
	{
		/// <summary>
		/// Determines if the key was pressed
		/// </summary>
		/// <param name="Key">The Key to check for</param>
		/// <returns>A true/false indicating that the Key was pressed</returns>
		public static bool GetKeyState(Key Key)
		{
			R3DKey r = (R3DKey)Key;
			return Interop._instance.Control.Keyboard_GetKeyState(ref r);
		}

		public static void ReadKeys() {
			// the following line of code made redundant by 8088C
			//Interop._instance.Control..Keyboard_ReceiveKeys();
		}
	}
}
