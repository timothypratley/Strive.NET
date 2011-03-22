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
            CommandBinding cb = new CommandBinding(ApplicationCommands.Copy, CopyCmdExecuted, CopyCmdCanExecute);
            listView1.CommandBindings.Add(cb);
        }

        // TODO: re-factor this with LogView "don't repeat yourself"
        void CopyCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            ListBox lb = e.OriginalSource as ListView;
            string copyContent = String.Empty;
            foreach (var item in lb.SelectedItems)
                copyContent += item.ToString() + Environment.NewLine;
            Clipboard.SetText(copyContent);
        }

        void CopyCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            ListBox lb = e.OriginalSource as ListView;
            e.CanExecute = (lb.SelectedItems.Count > 0);
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
