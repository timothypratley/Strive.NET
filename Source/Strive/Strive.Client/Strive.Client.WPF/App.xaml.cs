using System;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using Strive.Client.Logic;
using Strive.Client.ViewModel;
using Strive.Common;
using Strive.Model;
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

        // Server side components
        public static readonly History History = new History();
        public static readonly Listener Listener = new Listener(
            new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], Constants.DefaultPort));
        public static readonly MessageProcessor MessageProcessor = new MessageProcessor(
            new World(Listener, Global.WorldId), Listener);

        // Client side components
        public static readonly ServerConnection ServerConnection = new ServerConnection();
        public static readonly ClientSideMessageProcessor ClientSideMessageProcessor = new ClientSideMessageProcessor(
            ServerConnection);
        public static readonly WorldViewModel WorldViewModel = new WorldViewModel(
            ServerConnection, History, new WorldNavigation(), new InputBindings());

        public static readonly LogModel LogModel = new LogModel();
        public static readonly LogModel ChatLogModel = new LogModel(ClientSideMessageProcessor.ChatListeners);

        public static readonly Engine ServerEngine = new Engine(MessageProcessor);

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
