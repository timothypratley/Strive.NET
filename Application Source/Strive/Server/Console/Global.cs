using System;
using Strive.Server.Shared;

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
		[STAThread]
		static void Main(string[] args)
		{
			Engine serverEngine = new Engine();
			serverEngine.Start();
		}
	}
}
