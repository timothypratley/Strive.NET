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
