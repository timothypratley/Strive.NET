using System;


namespace Strive.Rendering.Controls
{
	/// <summary>
	/// Deals with keyboard input
	/// </summary>
	public interface IKeyboard
	{
		/// <summary>
		/// Determines if the key was pressed
		/// </summary>
		/// <param name="Key">The Key to check for</param>
		/// <returns>A true/false indicating that the Key was pressed</returns>
		bool GetKeyState(Key Key);
	}
}
