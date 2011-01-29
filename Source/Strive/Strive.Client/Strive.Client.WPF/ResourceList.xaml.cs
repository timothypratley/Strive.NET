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
using UpdateControls.XAML;

using Strive.Client.Model;
using Strive.Client.NavigationModel;
using Strive.Client.ViewModel;

namespace Strive.Client.WPF
{
    /// <summary>
    /// Interaction logic for ResourceList.xaml
    /// </summary>
    public partial class ResourceList : DockableContent
    {
        public ResourceList()
        {
            InitializeComponent();
        }
    }
}
