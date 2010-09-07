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
using System.IO;

using Strive.WPF.View;


namespace Strive.Client.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NewCmdExecuted(null, null);
        }

        const string LayoutFileName = "StriveLayout.xml";

        private void SaveLayout(object sender, RoutedEventArgs e)
        {
            dockManager.SaveLayout(LayoutFileName);
        }

        private void RestoreLayout(object sender, RoutedEventArgs e)
        {
            if (File.Exists(LayoutFileName))
                dockManager.RestoreLayout(LayoutFileName);
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
            var log = new LogView(App.LogViewModel);
            log.ShowAsDocument(dockManager);
            log.Focus();
        }

        private void NewCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var view = new WorldView();
            view.ShowAsDocument(dockManager);
            view.Focus();
        }

        private void SearchCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SearchCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var resourceList = new ResourceList(App.WorldViewModel);
            resourceList.ShowAsDocument(dockManager);
            resourceList.Focus();
        }

        private void BrowseHomeCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void BrowseHomeCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var connect = new Connect();
            connect.ShowAsDocument(dockManager);
            connect.Focus();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var wb = new WebBrowser();
            wb.ShowAsDocument(dockManager);
            wb.Focus();
        }
    }
}
