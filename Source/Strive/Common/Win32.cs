using System;
using System.Runtime.InteropServices;

namespace Strive.Common
{
	/// <summary>
	/// A simple class that exposes two static Win32 functions.
	/// One is a delegate type and the other is an enumerated type.
	/// </summary>
	public class Win32 {
		// Declare the SetConsoleCtrlHandler function 
		// as external and receiving a delegate.   
		[DllImport("Kernel32")] 
		public static extern Boolean SetConsoleCtrlHandler(
			HandlerRoutine Handler, Boolean Add);

		// A delegate type to be used as the handler routine 
		// for SetConsoleCtrlHandler.
		public delegate Boolean HandlerRoutine(CtrlTypes CtrlType);

		// An enumerated type for the control messages 
		// sent to the handler routine.
		public enum CtrlTypes {
			CTRL_C_EVENT = 0,
			CTRL_BREAK_EVENT,
			CTRL_CLOSE_EVENT,   
			CTRL_LOGOFF_EVENT = 5,
			CTRL_SHUTDOWN_EVENT
		}
	}
}
