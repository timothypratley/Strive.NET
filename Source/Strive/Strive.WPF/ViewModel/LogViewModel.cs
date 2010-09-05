using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Strive.WPF.Model;

namespace Strive.WPF.ViewModel
{
    public class LogViewModel : TraceListener
    {
        public LogModel LogModel { get; set; }

        public LogViewModel()
        {
            LogModel = new LogModel();
            Trace.Listeners.Add(this);
        }

        private string messageSoFar = String.Empty;
        public override void Write(string message)
        {
            messageSoFar += message;
        }

        public override void WriteLine(string message)
        {
            LogModel.NewLogEntry(messageSoFar + message);
            messageSoFar = String.Empty;
        }
    }
}
