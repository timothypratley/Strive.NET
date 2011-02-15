using System;
using System.Windows;
using System.Windows.Threading;
using System.Reflection;

using Common.Logging;
using Strive.Client.ViewModel;
using Strive.Client.Model;
using Strive.WPF;
using Strive.Server.Logic;
using Strive.Network.Messaging;


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
        public static DictionaryModel<string, EntityModel> WorldModel;
        public static LogModel LogModel;
        public static Engine ServerEngine = new Engine();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LogModel = new LogModel();
            Log.Info("Starting " + Assembly.GetExecutingAssembly().GetName().FullName);

            ServerConnection = new ServerConnection();
            WorldViewModel = new WorldViewModel(ServerConnection);
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
