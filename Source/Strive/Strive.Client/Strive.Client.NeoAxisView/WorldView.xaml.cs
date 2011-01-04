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
using Engine;
using Engine.EntitySystem;
using Engine.MapSystem;
using Engine.Renderer;
using Engine.MathEx;
using Engine.SoundSystem;
using Engine.UISystem;
using WPFAppFramework;


namespace Strive.Client.NeoAxisView
{
    /// <summary>
    /// Interaction logic for WorldView.xaml
    /// </summary>
    public partial class WorldView : DockableContent
    {
        public WorldView()
        {
            InitializeComponent();
        }
    }
}
