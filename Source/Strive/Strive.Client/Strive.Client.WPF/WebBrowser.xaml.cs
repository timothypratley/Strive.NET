using System;
using AvalonDock;

namespace Strive.Client.WPF
{
    /// <summary>
    /// Interaction logic for WebBrowser.xaml
    /// </summary>
    public partial class WebBrowser : DockableContent
    {
        public WebBrowser()
        {
            InitializeComponent();
            myBrowser.Navigate(new Uri("http://gr1d.org"));
        }
    }
}
