using System;

using Strive.Server.Logic;

namespace Strive.Server.Console
{
	class Global
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static Engine serverEngine = new Engine();

		[STAThread]
		static void Main(string[] args)
		{
            System.Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
			serverEngine.Start();
		}

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
			System.Console.WriteLine("Cancel requested...");
			serverEngine.Stop();
            System.Console.WriteLine("Ready to terminate");
            System.Threading.Thread.Sleep(10000);
		}
	}
}
