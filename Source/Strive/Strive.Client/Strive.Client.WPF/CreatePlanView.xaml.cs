using AvalonDock;


namespace Strive.Client.WPF
{
    public partial class CreatePlanView : DockableContent
    {
        public CreatePlanView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.WorldViewModel.CreateMission.Execute(null);
        }
    }
}
