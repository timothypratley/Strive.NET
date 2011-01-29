using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


namespace Strive.WPF
{
    public class LogViewModel
    {
        public LogModel LogModel { get; private set; }

        public LogViewModel(LogModel logModel)
        {
            LogModel = logModel;
        }
    }
}
