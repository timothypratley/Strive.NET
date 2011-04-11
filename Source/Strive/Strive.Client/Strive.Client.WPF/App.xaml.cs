using System;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using Strive.Client.ViewModel;
using Strive.Common;
using Strive.Network.Messaging;
using Strive.Server.Logic;
using Strive.WPF;


namespace Strive.Client.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly Random Rand = new Random();
        public static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static readonly ServerConnection ServerConnection = new ServerConnection();
        public static readonly WorldViewModel WorldViewModel = new WorldViewModel(ServerConnection);
        public static readonly LogModel LogModel = new LogModel();
        public static readonly LogModel ChatLogModel = new LogModel(ServerConnection.ChatListeners);

        public static readonly Engine ServerEngine = new Engine(
            new MessageProcessor(
                new World(Global.WorldId),
                new Listener(new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], Constants.DefaultPort))));

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, args) =>
                ReportException(args.ExceptionObject as Exception);
            Log.Info("Starting " + Assembly.GetExecutingAssembly().GetName().FullName);
        }

        private static bool ReportException(Exception ex)
        {
            Log.Fatal("Uncaught Exception", ex);
            using (var d = new System.Windows.Forms.ThreadExceptionDialog(ex))
            {
                return d.ShowDialog() == System.Windows.Forms.DialogResult.Abort;
            }
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = ReportException(e.Exception);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ServerEngine.Stop();
        }
    }
}
