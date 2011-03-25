using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AvalonDock;
using Strive.Network.Messaging;

namespace Strive.Client.WPF
{
    public partial class ChatLogView : DockableContent
    {
        private ServerConnection _serverConnection;
        public ChatLogView(ServerConnection serverConnection)
        {
            _serverConnection = serverConnection;
            InitializeComponent();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                _serverConnection.Chat("Chat", textBox1.Text);
                textBox1.Clear();
            }
        }
    }
}
