using System;

using Strive.Server.Logic;

namespace Strive.Server.Console
{
	class Global
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static readonly Engine ServerEngine = new Engine();

		[STAThread]
		static void Main()
		{
            System.Console.CancelKeyPress += Console_CancelKeyPress;
			ServerEngine.Start();
		}

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
			System.Console.WriteLine("Cancel requested...");
			ServerEngine.Stop();
            System.Console.WriteLine("Ready to terminate");
            System.Threading.Thread.Sleep(10000);
		}
	}
}
