using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using Strive.Client.Model;
using Strive.Client.ViewModel;
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
        static readonly ILog Log = LogManager.GetCurrentClassLogger();
        public static WorldViewModel WorldViewModel;
        public static ServerConnection ServerConnection;
        public static History WorldModel;
        public static LogModel LogModel;
        public static LogModel ChatLogModel;
        public static Engine ServerEngine = new Engine();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LogModel = new LogModel();
            Log.Info("Starting " + Assembly.GetExecutingAssembly().GetName().FullName);

            ServerConnection = new ServerConnection();
            WorldViewModel = new WorldViewModel(ServerConnection);
            ChatLogModel = new LogModel(ServerConnection.ChatListeners);
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

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ReportException(e.ExceptionObject as Exception);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ServerEngine.Stop();
        }
    }
}
