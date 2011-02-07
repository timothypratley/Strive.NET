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
using Strive.Common;


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
            portTextBox.Text = Constants.DefaultPort.ToString();

            // TODO: make a viewmodel and expose connected status
            DataContext = App.ServerConnection;
        }

        // validation not hooked up
        string _port;
        public string Port
        {
            get { return _port; }
            set
            {
                _port = value;
                int result;
                if (!int.TryParse(_port, out result) || result < 1024 || result > 65535)
                {
                    throw new ApplicationException("Must be between 1024 and 65535");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int port = Constants.DefaultPort;
            int.TryParse(portTextBox.Text, out port);
            App.ServerConnection.Start(new IPEndPoint(Dns.GetHostEntry(hostTextBox.Text).AddressList[0], port));
            App.ServerConnection.Connect += (object s, EventArgs ev) =>
            {
                App.ServerConnection.Login("bob@smith.com", "bob");
                App.ServerConnection.PossessMobile(1);
            };
        }
    }
}
