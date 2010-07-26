using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.Integration;

using AvalonDock;
using Strive.Client.NeoAxisView;
using Strive.Client.ViewModel;
using Strive.Network.Client;

namespace Strive.Client.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WorldViewModel worldViewModel;
        ServerConnection serverConnection;
        
        public MainWindow()
        {
            serverConnection = new ServerConnection();
            worldViewModel = new WorldViewModel(serverConnection);
            WorldViewControl.Init(worldViewModel);
            InitializeComponent();
        }
        private void CloseCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Opening");
        }

        private void NewCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var view = new DockableContent();
            view.Title = "World View";

            var c = new WorldViewControl();
            var host = new WindowsFormsHost();
            host.Child = c;

            view.Content = host;
            view.ShowAsDocument(dockManager);
            view.Focus();
        }
    }
}
