using System;

using Strive.Server.Logic;
using Strive.Common;

namespace Strive.Server.Console
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Global
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static Engine serverEngine = new Engine();
		static Win32.HandlerRoutine hr;

		[STAThread]
		static void Main(string[] args)
		{
			hr = new Win32.HandlerRoutine(Handler);
			Win32.SetConsoleCtrlHandler( hr, true );
			serverEngine.Start();
		}

		static Boolean Handler(Win32.CtrlTypes CtrlType) {
			String message = "This message should never be seen!";

			// A switch to handle the event type.
			switch(CtrlType) {
				case Win32.CtrlTypes.CTRL_C_EVENT:
					message = "A CTRL_C_EVENT was raised by the user.";
					break;
				case Win32.CtrlTypes.CTRL_BREAK_EVENT:
					message = "A CTRL_BREAK_EVENT was raised by the user.";
					break;
				case Win32.CtrlTypes.CTRL_CLOSE_EVENT:   
					message = "A CTRL_CLOSE_EVENT was raised by the user.";
					break;
				case Win32.CtrlTypes.CTRL_LOGOFF_EVENT:
					message = "A CTRL_LOGOFF_EVENT was raised by the user.";
					break;
				case Win32.CtrlTypes.CTRL_SHUTDOWN_EVENT:
					message = "A CTRL_SHUTDOWN_EVENT was raised by the user.";
					break;
			}

			// Use interop to display a message for the type of event.
			System.Console.WriteLine(message);
			serverEngine.Stop();
			GC.KeepAlive( hr );
			return true;
		}
	}
}
