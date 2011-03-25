using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
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

            // TODO: make a view model and expose connected status; holding a server connection instead of global
            DataContext = App.ServerConnection;
        }

        // TODO: validation not hooked up
        string _port;
        public string Port
        {
            get { return _port; }
            set
            {
                _port = value;
                int result;
                if (!int.TryParse(_port, out result) || result < 1024 || result > 65535)
                    throw new ApplicationException("Port must be between 1024 and 65535");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int port;
            if (!int.TryParse(portTextBox.Text, out port))
                port = Constants.DefaultPort;
            App.ServerConnection.Start(new IPEndPoint(Dns.GetHostEntry(hostTextBox.Text).AddressList[0], port));

            // TODO: Are there any encryption APIs that hash from a secure password directly? calling ToString is bad
            var service = new MD5CryptoServiceProvider();
            var bytes = service.ComputeHash(Encoding.Default.GetBytes(password.SecurePassword.ToString()));
            var hashString = Convert.ToBase64String(bytes);
            var userString = username.Text;

            App.ServerConnection.Connect += (s, ev) =>
                                                {
                                                    var sc = ((ServerConnection)s);
                                                    sc.Login(userString, hashString);
                                                    sc.PossessMobile(App.Rand.Next());
                                                };
        }
    }
}
