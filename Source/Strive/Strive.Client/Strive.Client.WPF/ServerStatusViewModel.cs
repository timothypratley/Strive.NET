using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Strive.Server.Model;

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
