using System;

namespace Strive.Rendering
{
	/// <summary>
	/// Summary description for IViewport.
	/// </summary>
	public interface IViewport
	{
		ICamera Camera { get; }
		void SetFocus();
	}
}
