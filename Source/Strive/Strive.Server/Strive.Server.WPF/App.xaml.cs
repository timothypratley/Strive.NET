﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Net;

using Common.Logging;

using Strive.Network.Server;
using Strive.WPF.ViewModel;


namespace Strive.Server.WPF
{
    public partial class App : Application
    {
        static ILog Log = LogManager.GetCurrentClassLogger();
        public static Listener Listener;
        public static LogViewModel LogViewModel;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LogViewModel = new LogViewModel();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Log.Info("Starting " + Assembly.GetExecutingAssembly().GetName().FullName);
            Listener = new Listener(new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], 8888));
            Listener.Start();
        }

        private bool ReportException(Exception ex)
        {
            Log.Fatal("Uncaught Exception", ex);
            using (var d = new System.Windows.Forms.ThreadExceptionDialog(ex))
            {
                return d.ShowDialog() == System.Windows.Forms.DialogResult.Abort;
            }
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (ReportException(e.Exception))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ReportException(e.ExceptionObject as Exception);
        }
    }
}
