using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Strive.WPF.Model;

namespace Strive.WPF.ViewModel
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
