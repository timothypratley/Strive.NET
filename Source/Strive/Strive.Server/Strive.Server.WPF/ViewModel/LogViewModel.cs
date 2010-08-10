using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Strive.Server.WPF.ViewModel
{
    class LogViewModel
    {
        public ObservableCollection<string> LogEntries { get; private set; }

        public LogViewModel()
        {
            LogEntries = new ObservableCollection<string>();
            LogEntries.Add("Test");
            LogEntries.Add("FOo");
        }
    }
}
