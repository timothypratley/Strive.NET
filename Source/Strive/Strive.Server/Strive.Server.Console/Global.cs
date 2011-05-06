using System;
using System.Net;
using System.Threading;
using Strive.Common;
using Strive.Network.Messaging;
using Strive.Server.Logic;

namespace Strive.Server.Console
{
    class Global
    {
        static readonly Listener Listener = new Listener(
            new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], Constants.DefaultPort));
        static readonly Engine ServerEngine = new Engine(
            new MessageProcessor(
                new World(Listener, 0),
                Listener));

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
