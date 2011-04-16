namespace Strive.Client.WPF
{
    public class ServerStatusViewModel
    {
        public ServerStatusModel ServerStatusModel { get; private set; }

        public ServerStatusViewModel(ServerStatusModel serverStatusModel)
        {
            ServerStatusModel = serverStatusModel;
        }
    }
}
