using System;
using System.Windows;
using System.Net;
using AvalonDock;
using Strive.Common;
using Strive.Network.Messaging;


namespace Strive.Client.WPF
{
    /// <summary>
    /// Interaction logic for ResourceList.xaml
    /// </summary>
    public partial class ConnectView : DockableContent
    {
        public ConnectView()
        {
            InitializeComponent();
            hostTextBox.Text = Dns.GetHostName();
            portTextBox.Text = Constants.DefaultPort.ToString();

            // TODO: make a viewmodel and expose connected status; holding a serverconnection instead of global
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
            int port;
            if (!int.TryParse(portTextBox.Text, out port))
            {
                // TODO: use input validation instead
                port = Constants.DefaultPort;
            }
            App.ServerConnection.Start(new IPEndPoint(Dns.GetHostEntry(hostTextBox.Text).AddressList[0], port));
            App.ServerConnection.Connect += (s, ev) =>
                                                {
                var sc = ((ServerConnection) s);
                sc.Login("bob@smith.com", "bob");
                sc.PossessMobile(1);
            };
        }
    }
}
