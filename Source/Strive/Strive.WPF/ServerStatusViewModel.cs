using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Strive.WPF
{
    public class ServerStatusViewModel
    {
        public ServerStatusModel ServerStatusModel { get; set; }

        public ServerStatusViewModel()
        {
            ServerStatusModel = new ServerStatusModel();
        }
    }
}
