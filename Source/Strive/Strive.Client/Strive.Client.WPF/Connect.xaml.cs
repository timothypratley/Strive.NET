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
using System.Windows.Shapes;
using System.Net;

using AvalonDock;
using Strive.Network.Client;


namespace Strive.Client.WPF
{
    /// <summary>
    /// Interaction logic for ResourceList.xaml
    /// </summary>
    public partial class Connect : DockableContent
    {
        public Connect()
        {
            InitializeComponent();
            hostTextBox.Text = Dns.GetHostName();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int port=8888;
            int.TryParse(portTextBox.Text, out port);
            App.ServerConnection.Start(new IPEndPoint(Dns.GetHostEntry(hostTextBox.Text).AddressList[0], port));
        }
    }
}
