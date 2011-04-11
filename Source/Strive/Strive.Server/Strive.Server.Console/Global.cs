using System;
using Strive.Server.Logic;
using Strive.Network.Messaging;
using System.Net;
using Strive.Common;
using System.Threading;

namespace Strive.Server.Console
{
    class Global
    {
        static readonly Engine ServerEngine = new Engine(
            new MessageProcessor(
                new World(1),
                new Listener(new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], Constants.DefaultPort))));

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
            Thread.Sleep(100);
        }
    }
}
