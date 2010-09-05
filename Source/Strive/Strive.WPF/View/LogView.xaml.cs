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

using AvalonDock;
using Strive.WPF.ViewModel;


namespace Strive.WPF.View
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : DockableContent
    {
        public LogView(LogViewModel logViewModel)
        {
            InitializeComponent();
            DataContext = logViewModel;
        }
    }
}
